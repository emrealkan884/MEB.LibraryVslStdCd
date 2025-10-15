using Application.Features.Kutuphaneler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Kutuphaneler.Commands.Create;

public class CreateKutuphaneCommand : IRequest<CreatedKutuphaneResponse>
{
    public required string Kod { get; set; }
    public required string Ad { get; set; }
    public KutuphaneTipi Tip { get; set; }
    public required string Adres { get; set; }
    public string? Telefon { get; set; }
    public string? EPosta { get; set; }
    public bool Aktif { get; set; }

    public class CreateKutuphaneCommandHandler : IRequestHandler<CreateKutuphaneCommand, CreatedKutuphaneResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly KutuphaneBusinessRules _kutuphaneBusinessRules;

        public CreateKutuphaneCommandHandler(IMapper mapper, IKutuphaneRepository kutuphaneRepository,
                                             KutuphaneBusinessRules kutuphaneBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneRepository = kutuphaneRepository;
            _kutuphaneBusinessRules = kutuphaneBusinessRules;
        }

        public async Task<CreatedKutuphaneResponse> Handle(CreateKutuphaneCommand request, CancellationToken cancellationToken)
        {
            Kutuphane kutuphane = _mapper.Map<Kutuphane>(request);

            await _kutuphaneRepository.AddAsync(kutuphane);

            CreatedKutuphaneResponse response = _mapper.Map<CreatedKutuphaneResponse>(kutuphane);
            return response;
        }
    }
}
