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

namespace Application.Features.OduncIslemler.Commands.Return;

public class ReturnOduncIslemiCommand : IRequest<ReturnedOduncIslemiResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, IIntervalRequest
{
    public Guid Id { get; set; } // Ödünç işlemi ID'si
    public string? IadeNotu { get; set; } // İade sırasında eklenecek not

    public string[] Roles => ["Users.Admin", "Users.Write"];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetOduncIslemler"];
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 30;

    public class ReturnOduncIslemiCommandHandler : IRequestHandler<ReturnOduncIslemiCommand, ReturnedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public ReturnOduncIslemiCommandHandler(
            IMapper mapper,
            IOduncIslemiRepository oduncIslemiRepository,
            OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<ReturnedOduncIslemiResponse> Handle(ReturnOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            // Ödünç işlemini getir
            var oduncIslemi = await _oduncIslemiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                cancellationToken: cancellationToken
            );

            await _oduncIslemiBusinessRules.OduncIslemiShouldExistWhenSelected(oduncIslemi);

            if (oduncIslemi!.Durumu != OduncDurumu.Aktif && oduncIslemi.Durumu != OduncDurumu.Gecikmis)
                throw new InvalidOperationException("Sadece aktif veya gecikmiş ödünç işlemleri iade edilebilir.");

            // İade işlemini gerçekleştir
            oduncIslemi.OdunciIadeEt();

            // İade notunu ekle
            if (!string.IsNullOrEmpty(request.IadeNotu))
            {
                oduncIslemi.Not += $"{DateTime.UtcNow:yyyy-MM-dd}: İade - {request.IadeNotu}. ";
            }

            await _oduncIslemiRepository.UpdateAsync(oduncIslemi);

            ReturnedOduncIslemiResponse response = _mapper.Map<ReturnedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}