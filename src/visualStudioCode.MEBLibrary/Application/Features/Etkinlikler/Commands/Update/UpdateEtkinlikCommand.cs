using Application.Features.Etkinlikler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Etkinlikler.Commands.Update;

public class UpdateEtkinlikCommand : IRequest<UpdatedEtkinlikResponse>
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public string? Konum { get; set; }
    public string? AfisDosyasi { get; set; }

    public class UpdateEtkinlikCommandHandler : IRequestHandler<UpdateEtkinlikCommand, UpdatedEtkinlikResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly EtkinlikBusinessRules _etkinlikBusinessRules;

        public UpdateEtkinlikCommandHandler(IMapper mapper, IEtkinlikRepository etkinlikRepository,
                                             EtkinlikBusinessRules etkinlikBusinessRules)
        {
            _mapper = mapper;
            _etkinlikRepository = etkinlikRepository;
            _etkinlikBusinessRules = etkinlikBusinessRules;
        }

        public async Task<UpdatedEtkinlikResponse> Handle(UpdateEtkinlikCommand request, CancellationToken cancellationToken)
        {
            Etkinlik? etkinlik = await _etkinlikRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _etkinlikBusinessRules.EtkinlikShouldExistWhenSelected(etkinlik);
            etkinlik = _mapper.Map(request, etkinlik);

            await _etkinlikRepository.UpdateAsync(etkinlik!);

            UpdatedEtkinlikResponse response = _mapper.Map<UpdatedEtkinlikResponse>(etkinlik);
            return response;
        }
    }
}
