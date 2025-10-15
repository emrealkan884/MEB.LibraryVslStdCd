using Application.Features.Yazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Yazarlar.Commands.Update;

public class UpdateYazarCommand : IRequest<UpdatedYazarResponse>
{
    public Guid Id { get; set; }
    public required string AdSoyad { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? Uyruk { get; set; }
    public string? Aciklama { get; set; }

    public class UpdateYazarCommandHandler : IRequestHandler<UpdateYazarCommand, UpdatedYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYazarRepository _yazarRepository;
        private readonly YazarBusinessRules _yazarBusinessRules;

        public UpdateYazarCommandHandler(IMapper mapper, IYazarRepository yazarRepository,
                                         YazarBusinessRules yazarBusinessRules)
        {
            _mapper = mapper;
            _yazarRepository = yazarRepository;
            _yazarBusinessRules = yazarBusinessRules;
        }

        public async Task<UpdatedYazarResponse> Handle(UpdateYazarCommand request, CancellationToken cancellationToken)
        {
            Yazar? yazar = await _yazarRepository.GetAsync(predicate: y => y.Id == request.Id, cancellationToken: cancellationToken);
            await _yazarBusinessRules.YazarShouldExistWhenSelected(yazar);
            yazar = _mapper.Map(request, yazar);

            await _yazarRepository.UpdateAsync(yazar!);

            UpdatedYazarResponse response = _mapper.Map<UpdatedYazarResponse>(yazar);
            return response;
        }
    }
}