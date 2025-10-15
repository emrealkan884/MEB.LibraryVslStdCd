using Application.Features.Yazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Yazarlar.Commands.Create;

public class CreateYazarCommand : IRequest<CreatedYazarResponse>
{
    public required string AdSoyad { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? Uyruk { get; set; }
    public string? Aciklama { get; set; }

    public class CreateYazarCommandHandler : IRequestHandler<CreateYazarCommand, CreatedYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYazarRepository _yazarRepository;
        private readonly YazarBusinessRules _yazarBusinessRules;

        public CreateYazarCommandHandler(IMapper mapper, IYazarRepository yazarRepository,
                                         YazarBusinessRules yazarBusinessRules)
        {
            _mapper = mapper;
            _yazarRepository = yazarRepository;
            _yazarBusinessRules = yazarBusinessRules;
        }

        public async Task<CreatedYazarResponse> Handle(CreateYazarCommand request, CancellationToken cancellationToken)
        {
            Yazar yazar = _mapper.Map<Yazar>(request);

            await _yazarRepository.AddAsync(yazar);

            CreatedYazarResponse response = _mapper.Map<CreatedYazarResponse>(yazar);
            return response;
        }
    }
}