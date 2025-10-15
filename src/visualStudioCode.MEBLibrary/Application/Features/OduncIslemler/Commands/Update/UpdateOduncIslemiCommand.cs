using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.OduncIslemler.Commands.Update;

public class UpdateOduncIslemiCommand : IRequest<UpdatedOduncIslemiResponse>
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public OduncDurumu Durumu { get; set; }
    public string? Not { get; set; }

    public class UpdateOduncIslemiCommandHandler : IRequestHandler<UpdateOduncIslemiCommand, UpdatedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public UpdateOduncIslemiCommandHandler(IMapper mapper, IOduncIslemiRepository oduncIslemiRepository,
                                             OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<UpdatedOduncIslemiResponse> Handle(UpdateOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            OduncIslemi? oduncIslemi = await _oduncIslemiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _oduncIslemiBusinessRules.OduncIslemiShouldExistWhenSelected(oduncIslemi);
            oduncIslemi = _mapper.Map(request, oduncIslemi);

            await _oduncIslemiRepository.UpdateAsync(oduncIslemi!);

            UpdatedOduncIslemiResponse response = _mapper.Map<UpdatedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}
