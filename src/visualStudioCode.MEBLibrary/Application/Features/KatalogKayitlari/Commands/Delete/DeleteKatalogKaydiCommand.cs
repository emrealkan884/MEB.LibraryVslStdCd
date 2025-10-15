using Application.Features.KatalogKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKayitlari.Commands.Delete;

public class DeleteKatalogKaydiCommand : IRequest<DeletedKatalogKaydiResponse>
{
    public Guid Id { get; set; }

    public class DeleteKatalogKaydiCommandHandler : IRequestHandler<DeleteKatalogKaydiCommand, DeletedKatalogKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly KatalogKaydiBusinessRules _katalogKaydiBusinessRules;

        public DeleteKatalogKaydiCommandHandler(IMapper mapper, IKatalogKaydiRepository katalogKaydiRepository,
                                             KatalogKaydiBusinessRules katalogKaydiBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiRepository = katalogKaydiRepository;
            _katalogKaydiBusinessRules = katalogKaydiBusinessRules;
        }

        public async Task<DeletedKatalogKaydiResponse> Handle(DeleteKatalogKaydiCommand request, CancellationToken cancellationToken)
        {
            KatalogKaydi? katalogKaydi = await _katalogKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiBusinessRules.KatalogKaydiShouldExistWhenSelected(katalogKaydi);

            await _katalogKaydiRepository.DeleteAsync(katalogKaydi!);

            DeletedKatalogKaydiResponse response = _mapper.Map<DeletedKatalogKaydiResponse>(katalogKaydi);
            return response;
        }
    }
}
