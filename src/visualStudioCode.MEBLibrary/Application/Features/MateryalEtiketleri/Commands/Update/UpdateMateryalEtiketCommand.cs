using Application.Features.MateryalEtiketleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalEtiketleri.Commands.Update;

public class UpdateMateryalEtiketCommand : IRequest<UpdatedMateryalEtiketResponse>
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }

    public class UpdateMateryalEtiketCommandHandler : IRequestHandler<UpdateMateryalEtiketCommand, UpdatedMateryalEtiketResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly MateryalEtiketBusinessRules _materyalEtiketBusinessRules;

        public UpdateMateryalEtiketCommandHandler(IMapper mapper, IMateryalEtiketRepository materyalEtiketRepository,
                                             MateryalEtiketBusinessRules materyalEtiketBusinessRules)
        {
            _mapper = mapper;
            _materyalEtiketRepository = materyalEtiketRepository;
            _materyalEtiketBusinessRules = materyalEtiketBusinessRules;
        }

        public async Task<UpdatedMateryalEtiketResponse> Handle(UpdateMateryalEtiketCommand request, CancellationToken cancellationToken)
        {
            MateryalEtiket? materyalEtiket = await _materyalEtiketRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalEtiketBusinessRules.MateryalEtiketShouldExistWhenSelected(materyalEtiket);
            materyalEtiket = _mapper.Map(request, materyalEtiket);

            await _materyalEtiketRepository.UpdateAsync(materyalEtiket!);

            UpdatedMateryalEtiketResponse response = _mapper.Map<UpdatedMateryalEtiketResponse>(materyalEtiket);
            return response;
        }
    }
}
