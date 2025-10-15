using Application.Features.KatalogKonulari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKonulari.Queries.GetById;

public class GetByIdKatalogKonuQuery : IRequest<GetByIdKatalogKonuResponse>
{
    public Guid Id { get; set; }

    public class GetByIdKatalogKonuQueryHandler : IRequestHandler<GetByIdKatalogKonuQuery, GetByIdKatalogKonuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly KatalogKonuBusinessRules _katalogKonuBusinessRules;

        public GetByIdKatalogKonuQueryHandler(IMapper mapper, IKatalogKonuRepository katalogKonuRepository, KatalogKonuBusinessRules katalogKonuBusinessRules)
        {
            _mapper = mapper;
            _katalogKonuRepository = katalogKonuRepository;
            _katalogKonuBusinessRules = katalogKonuBusinessRules;
        }

        public async Task<GetByIdKatalogKonuResponse> Handle(GetByIdKatalogKonuQuery request, CancellationToken cancellationToken)
        {
            KatalogKonu? katalogKonu = await _katalogKonuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKonuBusinessRules.KatalogKonuShouldExistWhenSelected(katalogKonu);

            GetByIdKatalogKonuResponse response = _mapper.Map<GetByIdKatalogKonuResponse>(katalogKonu);
            return response;
        }
    }
}
