using Application.Features.Rezervasyonlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Rezervasyonlar.Commands.Delete;

public class DeleteRezervasyonCommand : IRequest<DeletedRezervasyonResponse>
{
    public Guid Id { get; set; }

    public class DeleteRezervasyonCommandHandler : IRequestHandler<DeleteRezervasyonCommand, DeletedRezervasyonResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly RezervasyonBusinessRules _rezervasyonBusinessRules;

        public DeleteRezervasyonCommandHandler(IMapper mapper, IRezervasyonRepository rezervasyonRepository,
                                             RezervasyonBusinessRules rezervasyonBusinessRules)
        {
            _mapper = mapper;
            _rezervasyonRepository = rezervasyonRepository;
            _rezervasyonBusinessRules = rezervasyonBusinessRules;
        }

        public async Task<DeletedRezervasyonResponse> Handle(DeleteRezervasyonCommand request, CancellationToken cancellationToken)
        {
            Rezervasyon? rezervasyon = await _rezervasyonRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rezervasyonBusinessRules.RezervasyonShouldExistWhenSelected(rezervasyon);

            await _rezervasyonRepository.DeleteAsync(rezervasyon!);

            DeletedRezervasyonResponse response = _mapper.Map<DeletedRezervasyonResponse>(rezervasyon);
            return response;
        }
    }
}
