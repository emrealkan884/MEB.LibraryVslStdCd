using Application.Features.OtoriteKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.OtoriteKayitlari.Commands.Delete;

public class DeleteOtoriteKaydiCommand : IRequest<DeletedOtoriteKaydiResponse>
{
    public Guid Id { get; set; }

    public class DeleteOtoriteKaydiCommandHandler : IRequestHandler<DeleteOtoriteKaydiCommand, DeletedOtoriteKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly OtoriteKaydiBusinessRules _otoriteKaydiBusinessRules;

        public DeleteOtoriteKaydiCommandHandler(IMapper mapper, IOtoriteKaydiRepository otoriteKaydiRepository,
                                             OtoriteKaydiBusinessRules otoriteKaydiBusinessRules)
        {
            _mapper = mapper;
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _otoriteKaydiBusinessRules = otoriteKaydiBusinessRules;
        }

        public async Task<DeletedOtoriteKaydiResponse> Handle(DeleteOtoriteKaydiCommand request, CancellationToken cancellationToken)
        {
            OtoriteKaydi? otoriteKaydi = await _otoriteKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _otoriteKaydiBusinessRules.OtoriteKaydiShouldExistWhenSelected(otoriteKaydi);

            await _otoriteKaydiRepository.DeleteAsync(otoriteKaydi!);

            DeletedOtoriteKaydiResponse response = _mapper.Map<DeletedOtoriteKaydiResponse>(otoriteKaydi);
            return response;
        }
    }
}
