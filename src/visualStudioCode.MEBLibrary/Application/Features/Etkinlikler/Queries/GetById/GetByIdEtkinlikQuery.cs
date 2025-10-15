using Application.Features.Etkinlikler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Etkinlikler.Queries.GetById;

public class GetByIdEtkinlikQuery : IRequest<GetByIdEtkinlikResponse>
{
    public Guid Id { get; set; }

    public class GetByIdEtkinlikQueryHandler : IRequestHandler<GetByIdEtkinlikQuery, GetByIdEtkinlikResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly EtkinlikBusinessRules _etkinlikBusinessRules;

        public GetByIdEtkinlikQueryHandler(IMapper mapper, IEtkinlikRepository etkinlikRepository, EtkinlikBusinessRules etkinlikBusinessRules)
        {
            _mapper = mapper;
            _etkinlikRepository = etkinlikRepository;
            _etkinlikBusinessRules = etkinlikBusinessRules;
        }

        public async Task<GetByIdEtkinlikResponse> Handle(GetByIdEtkinlikQuery request, CancellationToken cancellationToken)
        {
            Etkinlik? etkinlik = await _etkinlikRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _etkinlikBusinessRules.EtkinlikShouldExistWhenSelected(etkinlik);

            GetByIdEtkinlikResponse response = _mapper.Map<GetByIdEtkinlikResponse>(etkinlik);
            return response;
        }
    }
}
