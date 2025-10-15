using Application.Features.OtoriteKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.OtoriteKayitlari.Commands.Update;

public class UpdateOtoriteKaydiCommand : IRequest<UpdatedOtoriteKaydiResponse>
{
    public Guid Id { get; set; }
    public required string YetkiliBaslik { get; set; }
    public OtoriteTuru OtoriteTuru { get; set; }
    public string? AlternatifBasliklar { get; set; }
    public string? BagliTerimler { get; set; }
    public string? Aciklama { get; set; }
    public string? HariciKayitNo { get; set; }

    public class UpdateOtoriteKaydiCommandHandler : IRequestHandler<UpdateOtoriteKaydiCommand, UpdatedOtoriteKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly OtoriteKaydiBusinessRules _otoriteKaydiBusinessRules;

        public UpdateOtoriteKaydiCommandHandler(IMapper mapper, IOtoriteKaydiRepository otoriteKaydiRepository,
                                             OtoriteKaydiBusinessRules otoriteKaydiBusinessRules)
        {
            _mapper = mapper;
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _otoriteKaydiBusinessRules = otoriteKaydiBusinessRules;
        }

        public async Task<UpdatedOtoriteKaydiResponse> Handle(UpdateOtoriteKaydiCommand request, CancellationToken cancellationToken)
        {
            OtoriteKaydi? otoriteKaydi = await _otoriteKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _otoriteKaydiBusinessRules.OtoriteKaydiShouldExistWhenSelected(otoriteKaydi);
            await _otoriteKaydiBusinessRules.OtoriteKaydiYetkiliBaslikShouldBeUnique(
                request.YetkiliBaslik,
                request.OtoriteTuru,
                cancellationToken,
                request.Id
            );
            otoriteKaydi = _mapper.Map(request, otoriteKaydi);

            await _otoriteKaydiRepository.UpdateAsync(otoriteKaydi!);

            UpdatedOtoriteKaydiResponse response = _mapper.Map<UpdatedOtoriteKaydiResponse>(otoriteKaydi);
            return response;
        }
    }
}
