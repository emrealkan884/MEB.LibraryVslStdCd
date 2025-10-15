using Application.Features.KatalogKonulari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKonulari.Commands.Update;

public class UpdateKatalogKonuCommand : IRequest<UpdatedKatalogKonuResponse>
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public required string KonuBasligi { get; set; }
    public Guid? OtoriteKaydiId { get; set; }

    public class UpdateKatalogKonuCommandHandler : IRequestHandler<UpdateKatalogKonuCommand, UpdatedKatalogKonuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly KatalogKonuBusinessRules _katalogKonuBusinessRules;

        public UpdateKatalogKonuCommandHandler(IMapper mapper, IKatalogKonuRepository katalogKonuRepository,
                                             KatalogKonuBusinessRules katalogKonuBusinessRules)
        {
            _mapper = mapper;
            _katalogKonuRepository = katalogKonuRepository;
            _katalogKonuBusinessRules = katalogKonuBusinessRules;
        }

        public async Task<UpdatedKatalogKonuResponse> Handle(UpdateKatalogKonuCommand request, CancellationToken cancellationToken)
        {
            KatalogKonu? katalogKonu = await _katalogKonuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKonuBusinessRules.KatalogKonuShouldExistWhenSelected(katalogKonu);
            katalogKonu = _mapper.Map(request, katalogKonu);

            await _katalogKonuRepository.UpdateAsync(katalogKonu!);

            UpdatedKatalogKonuResponse response = _mapper.Map<UpdatedKatalogKonuResponse>(katalogKonu);
            return response;
        }
    }
}
