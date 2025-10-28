using Application.Authorization;
using Application.Features.Raporlama.Queries.Common;
using Application.Services.Repositories;
using Domain.Entities.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.Raporlama.Queries.GetUserSummary;

public class GetUserSummaryQuery : IRequest<UserSummaryDto>
{
    /// <summary>
    /// Dahil edilecek kullanıcı metrikleri. Null/boş ise hepsi. Anahtarlar: totalUsers, activeUsers, inactiveUsers.
    /// </summary>
    public IList<string>? Metrics { get; set; }

    public class GetUserSummaryQueryHandler : IRequestHandler<GetUserSummaryQuery, UserSummaryDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IKullaniciYetkiServisi _kullaniciYetkiServisi;

        public GetUserSummaryQueryHandler(
            IUserRepository userRepository,
            IUserOperationClaimRepository userOperationClaimRepository,
            IKullaniciYetkiServisi kullaniciYetkiServisi
        )
        {
            _userRepository = userRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _kullaniciYetkiServisi = kullaniciYetkiServisi;
        }

        public async Task<UserSummaryDto> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
        {
            KullaniciYetkiBilgisi yetki = await _kullaniciYetkiServisi.AktifKullaniciYetkisiAsync(cancellationToken);

            IQueryable<User> usersQuery = _userRepository.Query();

            if (yetki.TumUlkeGenelYetki is false)
            {
                if (yetki.OkulKutuphaneYoneticisi && yetki.TryGetKutuphaneId(out Guid? kutuphaneId))
                    usersQuery = usersQuery.Where(u => u.SorumluKutuphaneId == kutuphaneId);
                else if (yetki.IlceYetkilisi && yetki.TryGetIlKodu(out string? ilKodu) && yetki.TryGetIlceKodu(out string? ilceKodu))
                    usersQuery = usersQuery.Where(u => u.SorumluIlKodu == ilKodu && u.SorumluIlceKodu == ilceKodu);
                else if (yetki.IlYetkilisi && yetki.TryGetIlKodu(out string? ilKoduSadece))
                    usersQuery = usersQuery.Where(u => u.SorumluIlKodu == ilKoduSadece);
            }

            int totalUsers = await usersQuery.CountAsync(cancellationToken);
            int activeUsers = await usersQuery.Where(u => u.Status).CountAsync(cancellationToken);
            int inactiveUsers = totalUsers - activeUsers;

            Dictionary<string, SummaryMetricDto> metricMap = new()
            {
                ["totalUsers"] = new SummaryMetricDto("totalUsers", "Toplam Kullanıcı", totalUsers),
                ["activeUsers"] = new SummaryMetricDto("activeUsers", "Aktif Kullanıcı", activeUsers),
                ["inactiveUsers"] = new SummaryMetricDto("inactiveUsers", "Pasif Kullanıcı", inactiveUsers)
            };

            IEnumerable<string> selectedMetricKeys = request.Metrics is { Count: > 0 }
                ? request.Metrics.Where(metricMap.ContainsKey)
                : metricMap.Keys;

            IQueryable<Guid> filtrelenmisKullaniciIdleri = usersQuery.Select(u => u.Id);

            List<RoleCountDto> roleCounts = await _userOperationClaimRepository
                .Query()
                .Include(uoc => uoc.OperationClaim)
                .Where(uoc => uoc.OperationClaim.Name.StartsWith("Role."))
                .Where(uoc => filtrelenmisKullaniciIdleri.Contains(uoc.UserId))
                .GroupBy(uoc => uoc.OperationClaim.Name)
                .Select(group => new RoleCountDto
                {
                    RoleKey = group.Key,
                    UserCount = group
                        .Select(uoc => uoc.UserId)
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(rc => rc.UserCount)
                .ToListAsync(cancellationToken);

            foreach (RoleCountDto role in roleCounts)
            {
                role.RoleLabel = RoleLabelProvider.GetRoleLabel(role.RoleKey);
            }

            UserSummaryDto dto = new();
            foreach (string key in selectedMetricKeys)
                dto.Metrics.Add(metricMap[key]);
            dto.RoleBreakdown = roleCounts;

            return dto;
        }
    }
}
