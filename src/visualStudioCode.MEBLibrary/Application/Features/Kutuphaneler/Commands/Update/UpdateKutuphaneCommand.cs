using Application.Features.Kutuphaneler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Kutuphaneler.Commands.Update;

public class UpdateKutuphaneCommand : IRequest<UpdatedKutuphaneResponse>
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Ad { get; set; }
    public KutuphaneTipi Tip { get; set; }
    public required string Adres { get; set; }
    public string? Telefon { get; set; }
    public string? EPosta { get; set; }
    public bool Aktif { get; set; }

    public class UpdateKutuphaneCommandHandler : IRequestHandler<UpdateKutuphaneCommand, UpdatedKutuphaneResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly KutuphaneBusinessRules _kutuphaneBusinessRules;

        public UpdateKutuphaneCommandHandler(IMapper mapper, IKutuphaneRepository kutuphaneRepository,
                                             KutuphaneBusinessRules kutuphaneBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneRepository = kutuphaneRepository;
            _kutuphaneBusinessRules = kutuphaneBusinessRules;
        }

        public async Task<UpdatedKutuphaneResponse> Handle(UpdateKutuphaneCommand request, CancellationToken cancellationToken)
        {
            Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBusinessRules.KutuphaneShouldExistWhenSelected(kutuphane);
            kutuphane = _mapper.Map(request, kutuphane);

            await _kutuphaneRepository.UpdateAsync(kutuphane!);

            UpdatedKutuphaneResponse response = _mapper.Map<UpdatedKutuphaneResponse>(kutuphane);
            return response;
        }
    }
}
