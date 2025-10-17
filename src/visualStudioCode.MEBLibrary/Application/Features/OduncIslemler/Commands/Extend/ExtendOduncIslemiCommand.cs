using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Performance;

namespace Application.Features.OduncIslemler.Commands.Extend;

public class ExtendOduncIslemiCommand : IRequest<ExtendedOduncIslemiResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, IIntervalRequest
{
    public Guid Id { get; set; } // Ödünç işlemi ID'si
    public int EkGun { get; set; } // Kaç gün uzatılacağı
    public string? UzatmaNedeni { get; set; }

    public string[] Roles => ["Users.Admin", "Users.Write"];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetOduncIslemler"];
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 30;

    public class ExtendOduncIslemiCommandHandler : IRequestHandler<ExtendOduncIslemiCommand, ExtendedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public ExtendOduncIslemiCommandHandler(
            IMapper mapper,
            IOduncIslemiRepository oduncIslemiRepository,
            OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<ExtendedOduncIslemiResponse> Handle(ExtendOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            // Ödünç işlemini getir
            var oduncIslemi = await _oduncIslemiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                cancellationToken: cancellationToken
            );

            await _oduncIslemiBusinessRules.OduncIslemiShouldExistWhenSelected(oduncIslemi);

            // Uzatma kurallarını kontrol et
            await _oduncIslemiBusinessRules.LoanShouldBeEligibleForExtension(oduncIslemi!);

            // Ek gün sayısını doğrula
            if (request.EkGun <= 0 || request.EkGun > 7) // Maksimum 7 gün uzatma
                throw new InvalidOperationException("Uzatma süresi 1-7 gün arasında olmalıdır.");

            // Süreyi uzat
            oduncIslemi!.SureUzat(request.EkGun);

            // Uzatma nedenini notlara ekle
            if (!string.IsNullOrEmpty(request.UzatmaNedeni))
            {
                oduncIslemi.Not += $"{DateTime.UtcNow:yyyy-MM-dd}: Uzatma nedeni - {request.UzatmaNedeni}. ";
            }

            await _oduncIslemiRepository.UpdateAsync(oduncIslemi);

            ExtendedOduncIslemiResponse response = _mapper.Map<ExtendedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}