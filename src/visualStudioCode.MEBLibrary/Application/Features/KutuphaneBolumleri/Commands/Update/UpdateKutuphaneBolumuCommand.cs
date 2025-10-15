using Application.Features.KutuphaneBolumleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KutuphaneBolumleri.Commands.Update;

public class UpdateKutuphaneBolumuCommand : IRequest<UpdatedKutuphaneBolumuResponse>
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }

    public class UpdateKutuphaneBolumuCommandHandler : IRequestHandler<UpdateKutuphaneBolumuCommand, UpdatedKutuphaneBolumuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly KutuphaneBolumuBusinessRules _kutuphaneBolumuBusinessRules;

        public UpdateKutuphaneBolumuCommandHandler(IMapper mapper, IKutuphaneBolumuRepository kutuphaneBolumuRepository,
                                             KutuphaneBolumuBusinessRules kutuphaneBolumuBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _kutuphaneBolumuBusinessRules = kutuphaneBolumuBusinessRules;
        }

        public async Task<UpdatedKutuphaneBolumuResponse> Handle(UpdateKutuphaneBolumuCommand request, CancellationToken cancellationToken)
        {
            KutuphaneBolumu? kutuphaneBolumu = await _kutuphaneBolumuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBolumuBusinessRules.KutuphaneBolumuShouldExistWhenSelected(kutuphaneBolumu);
            kutuphaneBolumu = _mapper.Map(request, kutuphaneBolumu);

            await _kutuphaneBolumuRepository.UpdateAsync(kutuphaneBolumu!);

            UpdatedKutuphaneBolumuResponse response = _mapper.Map<UpdatedKutuphaneBolumuResponse>(kutuphaneBolumu);
            return response;
        }
    }
}
