using Application.Features.Nushalar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Nushalar.Commands.Delete;

public class DeleteNushaCommand : IRequest<DeletedNushaResponse>
{
    public Guid Id { get; set; }

    public class DeleteNushaCommandHandler : IRequestHandler<DeleteNushaCommand, DeletedNushaResponse>
    {
        private readonly IMapper _mapper;
        private readonly INushaRepository _nushaRepository;
        private readonly NushaBusinessRules _nushaBusinessRules;

        public DeleteNushaCommandHandler(IMapper mapper, INushaRepository nushaRepository,
                                             NushaBusinessRules nushaBusinessRules)
        {
            _mapper = mapper;
            _nushaRepository = nushaRepository;
            _nushaBusinessRules = nushaBusinessRules;
        }

        public async Task<DeletedNushaResponse> Handle(DeleteNushaCommand request, CancellationToken cancellationToken)
        {
            Nusha? nusha = await _nushaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _nushaBusinessRules.NushaShouldExistWhenSelected(nusha);

            await _nushaRepository.DeleteAsync(nusha!);

            DeletedNushaResponse response = _mapper.Map<DeletedNushaResponse>(nusha);
            return response;
        }
    }
}
