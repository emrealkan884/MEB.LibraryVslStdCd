using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Queries.GetById;

public class GetByIdYeniKatalogTalebiQuery : IRequest<GetByIdYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }

    public class GetByIdYeniKatalogTalebiQueryHandler : IRequestHandler<GetByIdYeniKatalogTalebiQuery, GetByIdYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public GetByIdYeniKatalogTalebiQueryHandler(IMapper mapper, IYeniKatalogTalebiRepository yeniKatalogTalebiRepository, YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules)
        {
            _mapper = mapper;
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
        }

        public async Task<GetByIdYeniKatalogTalebiResponse> Handle(GetByIdYeniKatalogTalebiQuery request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);

            GetByIdYeniKatalogTalebiResponse response = _mapper.Map<GetByIdYeniKatalogTalebiResponse>(yeniKatalogTalebi);
            return response;
        }
    }
}
