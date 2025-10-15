using Application.Features.KatalogKaydiYazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Delete;

public class DeleteKatalogKaydiYazarCommand : IRequest<DeletedKatalogKaydiYazarResponse>
{
    public Guid Id { get; set; }

    public class DeleteKatalogKaydiYazarCommandHandler : IRequestHandler<DeleteKatalogKaydiYazarCommand, DeletedKatalogKaydiYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly KatalogKaydiYazarBusinessRules _katalogKaydiYazarBusinessRules;

        public DeleteKatalogKaydiYazarCommandHandler(IMapper mapper, IKatalogKaydiYazarRepository katalogKaydiYazarRepository,
                                             KatalogKaydiYazarBusinessRules katalogKaydiYazarBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _katalogKaydiYazarBusinessRules = katalogKaydiYazarBusinessRules;
        }

        public async Task<DeletedKatalogKaydiYazarResponse> Handle(DeleteKatalogKaydiYazarCommand request, CancellationToken cancellationToken)
        {
            KatalogKaydiYazar? katalogKaydiYazar = await _katalogKaydiYazarRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiYazarBusinessRules.KatalogKaydiYazarShouldExistWhenSelected(katalogKaydiYazar);

            await _katalogKaydiYazarRepository.DeleteAsync(katalogKaydiYazar!);

            DeletedKatalogKaydiYazarResponse response = _mapper.Map<DeletedKatalogKaydiYazarResponse>(katalogKaydiYazar);
            return response;
        }
    }
}
