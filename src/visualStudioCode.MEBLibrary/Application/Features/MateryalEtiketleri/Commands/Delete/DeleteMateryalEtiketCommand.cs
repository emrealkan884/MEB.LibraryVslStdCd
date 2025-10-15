using Application.Features.MateryalEtiketleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalEtiketleri.Commands.Delete;

public class DeleteMateryalEtiketCommand : IRequest<DeletedMateryalEtiketResponse>
{
    public Guid Id { get; set; }

    public class DeleteMateryalEtiketCommandHandler : IRequestHandler<DeleteMateryalEtiketCommand, DeletedMateryalEtiketResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly MateryalEtiketBusinessRules _materyalEtiketBusinessRules;

        public DeleteMateryalEtiketCommandHandler(IMapper mapper, IMateryalEtiketRepository materyalEtiketRepository,
                                             MateryalEtiketBusinessRules materyalEtiketBusinessRules)
        {
            _mapper = mapper;
            _materyalEtiketRepository = materyalEtiketRepository;
            _materyalEtiketBusinessRules = materyalEtiketBusinessRules;
        }

        public async Task<DeletedMateryalEtiketResponse> Handle(DeleteMateryalEtiketCommand request, CancellationToken cancellationToken)
        {
            MateryalEtiket? materyalEtiket = await _materyalEtiketRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalEtiketBusinessRules.MateryalEtiketShouldExistWhenSelected(materyalEtiket);

            await _materyalEtiketRepository.DeleteAsync(materyalEtiket!);

            DeletedMateryalEtiketResponse response = _mapper.Map<DeletedMateryalEtiketResponse>(materyalEtiket);
            return response;
        }
    }
}
