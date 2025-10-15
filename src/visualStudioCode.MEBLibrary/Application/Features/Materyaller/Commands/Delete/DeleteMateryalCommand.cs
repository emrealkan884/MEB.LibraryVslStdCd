using Application.Features.Materyaller.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Materyaller.Commands.Delete;

public class DeleteMateryalCommand : IRequest<DeletedMateryalResponse>
{
    public Guid Id { get; set; }

    public class DeleteMateryalCommandHandler : IRequestHandler<DeleteMateryalCommand, DeletedMateryalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalRepository _materyalRepository;
        private readonly MateryalBusinessRules _materyalBusinessRules;

        public DeleteMateryalCommandHandler(IMapper mapper, IMateryalRepository materyalRepository,
                                             MateryalBusinessRules materyalBusinessRules)
        {
            _mapper = mapper;
            _materyalRepository = materyalRepository;
            _materyalBusinessRules = materyalBusinessRules;
        }

        public async Task<DeletedMateryalResponse> Handle(DeleteMateryalCommand request, CancellationToken cancellationToken)
        {
            Materyal? materyal = await _materyalRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalBusinessRules.MateryalShouldExistWhenSelected(materyal);

            await _materyalRepository.DeleteAsync(materyal!);

            DeletedMateryalResponse response = _mapper.Map<DeletedMateryalResponse>(materyal);
            return response;
        }
    }
}
