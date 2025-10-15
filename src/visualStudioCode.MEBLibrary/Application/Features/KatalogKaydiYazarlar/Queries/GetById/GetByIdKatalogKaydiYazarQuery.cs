using Application.Features.KatalogKaydiYazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKaydiYazarlar.Queries.GetById;

public class GetByIdKatalogKaydiYazarQuery : IRequest<GetByIdKatalogKaydiYazarResponse>
{
    public Guid Id { get; set; }

    public class GetByIdKatalogKaydiYazarQueryHandler : IRequestHandler<GetByIdKatalogKaydiYazarQuery, GetByIdKatalogKaydiYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly KatalogKaydiYazarBusinessRules _katalogKaydiYazarBusinessRules;

        public GetByIdKatalogKaydiYazarQueryHandler(IMapper mapper, IKatalogKaydiYazarRepository katalogKaydiYazarRepository, KatalogKaydiYazarBusinessRules katalogKaydiYazarBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _katalogKaydiYazarBusinessRules = katalogKaydiYazarBusinessRules;
        }

        public async Task<GetByIdKatalogKaydiYazarResponse> Handle(GetByIdKatalogKaydiYazarQuery request, CancellationToken cancellationToken)
        {
            KatalogKaydiYazar? katalogKaydiYazar = await _katalogKaydiYazarRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiYazarBusinessRules.KatalogKaydiYazarShouldExistWhenSelected(katalogKaydiYazar);

            GetByIdKatalogKaydiYazarResponse response = _mapper.Map<GetByIdKatalogKaydiYazarResponse>(katalogKaydiYazar);
            return response;
        }
    }
}
