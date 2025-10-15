using Application.Features.MateryalFormatDetaylari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.MateryalFormatDetaylari.Commands.Create;

public class CreateMateryalFormatDetayCommand : IRequest<CreatedMateryalFormatDetayResponse>
{
    public Guid KatalogKaydiId { get; set; }
    public MateryalTuru MateryalTuru { get; set; }
    public string? FizikselTanimi { get; set; }
    public string? SureBilgisi { get; set; }
    public string? FormatBilgisi { get; set; }
    public string? Dil { get; set; }
    public string? ErişimBilgisi { get; set; }

    public class CreateMateryalFormatDetayCommandHandler : IRequestHandler<CreateMateryalFormatDetayCommand, CreatedMateryalFormatDetayResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly MateryalFormatDetayBusinessRules _materyalFormatDetayBusinessRules;

        public CreateMateryalFormatDetayCommandHandler(IMapper mapper, IMateryalFormatDetayRepository materyalFormatDetayRepository,
                                             MateryalFormatDetayBusinessRules materyalFormatDetayBusinessRules)
        {
            _mapper = mapper;
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _materyalFormatDetayBusinessRules = materyalFormatDetayBusinessRules;
        }

        public async Task<CreatedMateryalFormatDetayResponse> Handle(CreateMateryalFormatDetayCommand request, CancellationToken cancellationToken)
        {
            MateryalFormatDetay materyalFormatDetay = _mapper.Map<MateryalFormatDetay>(request);

            await _materyalFormatDetayRepository.AddAsync(materyalFormatDetay);

            CreatedMateryalFormatDetayResponse response = _mapper.Map<CreatedMateryalFormatDetayResponse>(materyalFormatDetay);
            return response;
        }
    }
}
