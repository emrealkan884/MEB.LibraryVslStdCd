using Application.Features.KatalogKonulari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKonulari.Commands.Delete;

public class DeleteKatalogKonuCommand : IRequest<DeletedKatalogKonuResponse>
{
    public Guid Id { get; set; }

    public class DeleteKatalogKonuCommandHandler : IRequestHandler<DeleteKatalogKonuCommand, DeletedKatalogKonuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly KatalogKonuBusinessRules _katalogKonuBusinessRules;

        public DeleteKatalogKonuCommandHandler(IMapper mapper, IKatalogKonuRepository katalogKonuRepository,
                                             KatalogKonuBusinessRules katalogKonuBusinessRules)
        {
            _mapper = mapper;
            _katalogKonuRepository = katalogKonuRepository;
            _katalogKonuBusinessRules = katalogKonuBusinessRules;
        }

        public async Task<DeletedKatalogKonuResponse> Handle(DeleteKatalogKonuCommand request, CancellationToken cancellationToken)
        {
            KatalogKonu? katalogKonu = await _katalogKonuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKonuBusinessRules.KatalogKonuShouldExistWhenSelected(katalogKonu);

            await _katalogKonuRepository.DeleteAsync(katalogKonu!);

            DeletedKatalogKonuResponse response = _mapper.Map<DeletedKatalogKonuResponse>(katalogKonu);
            return response;
        }
    }
}
