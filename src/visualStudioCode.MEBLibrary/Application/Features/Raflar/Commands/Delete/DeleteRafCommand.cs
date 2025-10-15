using Application.Features.Raflar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Raflar.Commands.Delete;

public class DeleteRafCommand : IRequest<DeletedRafResponse>
{
    public Guid Id { get; set; }

    public class DeleteRafCommandHandler : IRequestHandler<DeleteRafCommand, DeletedRafResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRafRepository _rafRepository;
        private readonly RafBusinessRules _rafBusinessRules;

        public DeleteRafCommandHandler(IMapper mapper, IRafRepository rafRepository,
                                             RafBusinessRules rafBusinessRules)
        {
            _mapper = mapper;
            _rafRepository = rafRepository;
            _rafBusinessRules = rafBusinessRules;
        }

        public async Task<DeletedRafResponse> Handle(DeleteRafCommand request, CancellationToken cancellationToken)
        {
            Raf? raf = await _rafRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rafBusinessRules.RafShouldExistWhenSelected(raf);

            await _rafRepository.DeleteAsync(raf!);

            DeletedRafResponse response = _mapper.Map<DeletedRafResponse>(raf);
            return response;
        }
    }
}
