using Application.Features.Raflar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Raflar.Commands.Update;

public class UpdateRafCommand : IRequest<UpdatedRafResponse>
{
    public Guid Id { get; set; }
    public int KutuphaneBolumuId { get; set; }
    public required string Kod { get; set; }
    public string? Aciklama { get; set; }

    public class UpdateRafCommandHandler : IRequestHandler<UpdateRafCommand, UpdatedRafResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRafRepository _rafRepository;
        private readonly RafBusinessRules _rafBusinessRules;

        public UpdateRafCommandHandler(IMapper mapper, IRafRepository rafRepository,
                                             RafBusinessRules rafBusinessRules)
        {
            _mapper = mapper;
            _rafRepository = rafRepository;
            _rafBusinessRules = rafBusinessRules;
        }

        public async Task<UpdatedRafResponse> Handle(UpdateRafCommand request, CancellationToken cancellationToken)
        {
            Raf? raf = await _rafRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rafBusinessRules.RafShouldExistWhenSelected(raf);
            raf = _mapper.Map(request, raf);

            await _rafRepository.UpdateAsync(raf!);

            UpdatedRafResponse response = _mapper.Map<UpdatedRafResponse>(raf);
            return response;
        }
    }
}
