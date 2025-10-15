using Application.Features.Kutuphaneler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Kutuphaneler.Queries.GetById;

public class GetByIdKutuphaneQuery : IRequest<GetByIdKutuphaneResponse>
{
    public Guid Id { get; set; }

    public class GetByIdKutuphaneQueryHandler : IRequestHandler<GetByIdKutuphaneQuery, GetByIdKutuphaneResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly KutuphaneBusinessRules _kutuphaneBusinessRules;

        public GetByIdKutuphaneQueryHandler(IMapper mapper, IKutuphaneRepository kutuphaneRepository, KutuphaneBusinessRules kutuphaneBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneRepository = kutuphaneRepository;
            _kutuphaneBusinessRules = kutuphaneBusinessRules;
        }

        public async Task<GetByIdKutuphaneResponse> Handle(GetByIdKutuphaneQuery request, CancellationToken cancellationToken)
        {
            Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBusinessRules.KutuphaneShouldExistWhenSelected(kutuphane);

            GetByIdKutuphaneResponse response = _mapper.Map<GetByIdKutuphaneResponse>(kutuphane);
            return response;
        }
    }
}
