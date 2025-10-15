using Application.Features.KutuphaneBolumleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KutuphaneBolumleri.Commands.Delete;

public class DeleteKutuphaneBolumuCommand : IRequest<DeletedKutuphaneBolumuResponse>
{
    public Guid Id { get; set; }

    public class DeleteKutuphaneBolumuCommandHandler : IRequestHandler<DeleteKutuphaneBolumuCommand, DeletedKutuphaneBolumuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly KutuphaneBolumuBusinessRules _kutuphaneBolumuBusinessRules;

        public DeleteKutuphaneBolumuCommandHandler(IMapper mapper, IKutuphaneBolumuRepository kutuphaneBolumuRepository,
                                             KutuphaneBolumuBusinessRules kutuphaneBolumuBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _kutuphaneBolumuBusinessRules = kutuphaneBolumuBusinessRules;
        }

        public async Task<DeletedKutuphaneBolumuResponse> Handle(DeleteKutuphaneBolumuCommand request, CancellationToken cancellationToken)
        {
            KutuphaneBolumu? kutuphaneBolumu = await _kutuphaneBolumuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBolumuBusinessRules.KutuphaneBolumuShouldExistWhenSelected(kutuphaneBolumu);

            await _kutuphaneBolumuRepository.DeleteAsync(kutuphaneBolumu!);

            DeletedKutuphaneBolumuResponse response = _mapper.Map<DeletedKutuphaneBolumuResponse>(kutuphaneBolumu);
            return response;
        }
    }
}
