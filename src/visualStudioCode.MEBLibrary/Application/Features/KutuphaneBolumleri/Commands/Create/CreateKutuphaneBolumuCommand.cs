using Application.Features.KutuphaneBolumleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KutuphaneBolumleri.Commands.Create;

public class CreateKutuphaneBolumuCommand : IRequest<CreatedKutuphaneBolumuResponse>
{
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }

    public class CreateKutuphaneBolumuCommandHandler : IRequestHandler<CreateKutuphaneBolumuCommand, CreatedKutuphaneBolumuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly KutuphaneBolumuBusinessRules _kutuphaneBolumuBusinessRules;

        public CreateKutuphaneBolumuCommandHandler(IMapper mapper, IKutuphaneBolumuRepository kutuphaneBolumuRepository,
                                             KutuphaneBolumuBusinessRules kutuphaneBolumuBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _kutuphaneBolumuBusinessRules = kutuphaneBolumuBusinessRules;
        }

        public async Task<CreatedKutuphaneBolumuResponse> Handle(CreateKutuphaneBolumuCommand request, CancellationToken cancellationToken)
        {
            KutuphaneBolumu kutuphaneBolumu = _mapper.Map<KutuphaneBolumu>(request);

            await _kutuphaneBolumuRepository.AddAsync(kutuphaneBolumu);

            CreatedKutuphaneBolumuResponse response = _mapper.Map<CreatedKutuphaneBolumuResponse>(kutuphaneBolumu);
            return response;
        }
    }
}
