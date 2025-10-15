using Application.Features.Etkinlikler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Etkinlikler.Commands.Delete;

public class DeleteEtkinlikCommand : IRequest<DeletedEtkinlikResponse>
{
    public Guid Id { get; set; }

    public class DeleteEtkinlikCommandHandler : IRequestHandler<DeleteEtkinlikCommand, DeletedEtkinlikResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly EtkinlikBusinessRules _etkinlikBusinessRules;

        public DeleteEtkinlikCommandHandler(IMapper mapper, IEtkinlikRepository etkinlikRepository,
                                             EtkinlikBusinessRules etkinlikBusinessRules)
        {
            _mapper = mapper;
            _etkinlikRepository = etkinlikRepository;
            _etkinlikBusinessRules = etkinlikBusinessRules;
        }

        public async Task<DeletedEtkinlikResponse> Handle(DeleteEtkinlikCommand request, CancellationToken cancellationToken)
        {
            Etkinlik? etkinlik = await _etkinlikRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _etkinlikBusinessRules.EtkinlikShouldExistWhenSelected(etkinlik);

            await _etkinlikRepository.DeleteAsync(etkinlik!);

            DeletedEtkinlikResponse response = _mapper.Map<DeletedEtkinlikResponse>(etkinlik);
            return response;
        }
    }
}
