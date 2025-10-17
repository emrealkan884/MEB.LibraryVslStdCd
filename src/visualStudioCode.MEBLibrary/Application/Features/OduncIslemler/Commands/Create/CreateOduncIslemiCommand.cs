using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Performance;

namespace Application.Features.OduncIslemler.Commands.Create;

public class CreateOduncIslemiCommand : IRequest<CreatedOduncIslemiResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, IIntervalRequest
{
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public int? OduncSuresiGun { get; set; } // Null ise standart süre kullanılır
    public string? Not { get; set; }

    public string[] Roles => ["Users.Admin", "Users.Write"]; // Admin ve Write yetkisi olanlar ödünç verebilir

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetOduncIslemler"];
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 30; // 30 saniye performans izleme aralığı

    public class CreateOduncIslemiCommandHandler : IRequestHandler<CreateOduncIslemiCommand, CreatedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public CreateOduncIslemiCommandHandler(
            IMapper mapper,
            IOduncIslemiRepository oduncIslemiRepository,
            OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<CreatedOduncIslemiResponse> Handle(CreateOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            // İş kurallarını kontrol et
            await _oduncIslemiBusinessRules.NushaShouldBeAvailableForLoan(request.NushaId, cancellationToken);
            await _oduncIslemiBusinessRules.UserShouldBeEligibleForLoan(request.KullaniciId, request.KutuphaneId, cancellationToken);

            // Ödünç süresini belirle (null ise standart süre)
            int oduncSuresiGun = request.OduncSuresiGun ?? 14; // Standart 14 gün
            await _oduncIslemiBusinessRules.ValidateLoanPeriod(oduncSuresiGun);

            // Ödünç işlemini oluştur
            var oduncIslemi = new OduncIslemi
            {
                KutuphaneId = request.KutuphaneId,
                KullaniciId = request.KullaniciId,
                NushaId = request.NushaId,
                AlinmaTarihi = DateTime.UtcNow,
                SonTeslimTarihi = DateTime.UtcNow.AddDays(oduncSuresiGun),
                Durumu = OduncDurumu.Aktif,
                Not = request.Not
            };

            await _oduncIslemiRepository.AddAsync(oduncIslemi);

            CreatedOduncIslemiResponse response = _mapper.Map<CreatedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}
