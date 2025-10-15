using Application.Features.Materyaller.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Materyaller.Commands.Update;

public class UpdateMateryalCommand : IRequest<UpdatedMateryalResponse>
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid? KutuphaneBolumuId { get; set; }
    public MateryalFormati Formati { get; set; }
    public bool RezervasyonaAcik { get; set; }
    public int MaksimumOduncSuresiGun { get; set; }
    public string? Not { get; set; }

    public class UpdateMateryalCommandHandler : IRequestHandler<UpdateMateryalCommand, UpdatedMateryalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalRepository _materyalRepository;
        private readonly MateryalBusinessRules _materyalBusinessRules;

        public UpdateMateryalCommandHandler(IMapper mapper, IMateryalRepository materyalRepository,
                                             MateryalBusinessRules materyalBusinessRules)
        {
            _mapper = mapper;
            _materyalRepository = materyalRepository;
            _materyalBusinessRules = materyalBusinessRules;
        }

        public async Task<UpdatedMateryalResponse> Handle(UpdateMateryalCommand request, CancellationToken cancellationToken)
        {
            Materyal? materyal = await _materyalRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalBusinessRules.MateryalShouldExistWhenSelected(materyal);
            materyal = _mapper.Map(request, materyal);

            await _materyalRepository.UpdateAsync(materyal!);

            UpdatedMateryalResponse response = _mapper.Map<UpdatedMateryalResponse>(materyal);
            return response;
        }
    }
}
