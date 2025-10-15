using Application.Features.KatalogKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKayitlari.Queries.GetById;

public class GetByIdKatalogKaydiQuery : IRequest<GetByIdKatalogKaydiResponse>
{
    public Guid Id { get; set; }

    public class GetByIdKatalogKaydiQueryHandler : IRequestHandler<GetByIdKatalogKaydiQuery, GetByIdKatalogKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly KatalogKaydiBusinessRules _katalogKaydiBusinessRules;

        public GetByIdKatalogKaydiQueryHandler(IMapper mapper, IKatalogKaydiRepository katalogKaydiRepository, KatalogKaydiBusinessRules katalogKaydiBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiRepository = katalogKaydiRepository;
            _katalogKaydiBusinessRules = katalogKaydiBusinessRules;
        }

        public async Task<GetByIdKatalogKaydiResponse> Handle(GetByIdKatalogKaydiQuery request, CancellationToken cancellationToken)
        {
            KatalogKaydi? katalogKaydi = await _katalogKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiBusinessRules.KatalogKaydiShouldExistWhenSelected(katalogKaydi);

            GetByIdKatalogKaydiResponse response = _mapper.Map<GetByIdKatalogKaydiResponse>(katalogKaydi);
            return response;
        }
    }
}
