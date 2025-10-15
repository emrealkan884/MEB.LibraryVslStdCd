using Application.Features.MateryalFormatDetaylari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.MateryalFormatDetaylari.Commands.Update;

public class UpdateMateryalFormatDetayCommand : IRequest<UpdatedMateryalFormatDetayResponse>
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public MateryalTuru MateryalTuru { get; set; }
    public string? FizikselTanimi { get; set; }
    public string? SureBilgisi { get; set; }
    public string? FormatBilgisi { get; set; }
    public string? Dil { get; set; }
    public string? ErişimBilgisi { get; set; }

    public class UpdateMateryalFormatDetayCommandHandler : IRequestHandler<UpdateMateryalFormatDetayCommand, UpdatedMateryalFormatDetayResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly MateryalFormatDetayBusinessRules _materyalFormatDetayBusinessRules;

        public UpdateMateryalFormatDetayCommandHandler(IMapper mapper, IMateryalFormatDetayRepository materyalFormatDetayRepository,
                                             MateryalFormatDetayBusinessRules materyalFormatDetayBusinessRules)
        {
            _mapper = mapper;
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _materyalFormatDetayBusinessRules = materyalFormatDetayBusinessRules;
        }

        public async Task<UpdatedMateryalFormatDetayResponse> Handle(UpdateMateryalFormatDetayCommand request, CancellationToken cancellationToken)
        {
            MateryalFormatDetay? materyalFormatDetay = await _materyalFormatDetayRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalFormatDetayBusinessRules.MateryalFormatDetayShouldExistWhenSelected(materyalFormatDetay);
            materyalFormatDetay = _mapper.Map(request, materyalFormatDetay);

            await _materyalFormatDetayRepository.UpdateAsync(materyalFormatDetay!);

            UpdatedMateryalFormatDetayResponse response = _mapper.Map<UpdatedMateryalFormatDetayResponse>(materyalFormatDetay);
            return response;
        }
    }
}
