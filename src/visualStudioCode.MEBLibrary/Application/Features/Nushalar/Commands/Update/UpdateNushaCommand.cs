using Application.Features.Nushalar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Nushalar.Commands.Update;

public class UpdateNushaCommand : IRequest<UpdatedNushaResponse>
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public Guid? RafId { get; set; }
    public required string Barkod { get; set; }
    public NushaDurumu Durumu { get; set; }
    public DateTime EklenmeTarihi { get; set; }

    public class UpdateNushaCommandHandler : IRequestHandler<UpdateNushaCommand, UpdatedNushaResponse>
    {
        private readonly IMapper _mapper;
        private readonly INushaRepository _nushaRepository;
        private readonly NushaBusinessRules _nushaBusinessRules;

        public UpdateNushaCommandHandler(IMapper mapper, INushaRepository nushaRepository,
                                             NushaBusinessRules nushaBusinessRules)
        {
            _mapper = mapper;
            _nushaRepository = nushaRepository;
            _nushaBusinessRules = nushaBusinessRules;
        }

        public async Task<UpdatedNushaResponse> Handle(UpdateNushaCommand request, CancellationToken cancellationToken)
        {
            Nusha? nusha = await _nushaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _nushaBusinessRules.NushaShouldExistWhenSelected(nusha);
            nusha = _mapper.Map(request, nusha);

            await _nushaRepository.UpdateAsync(nusha!);

            UpdatedNushaResponse response = _mapper.Map<UpdatedNushaResponse>(nusha);
            return response;
        }
    }
}
