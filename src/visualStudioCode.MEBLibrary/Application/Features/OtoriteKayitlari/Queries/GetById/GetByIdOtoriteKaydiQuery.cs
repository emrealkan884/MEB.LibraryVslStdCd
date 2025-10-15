using Application.Features.OtoriteKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.OtoriteKayitlari.Queries.GetById;

public class GetByIdOtoriteKaydiQuery : IRequest<GetByIdOtoriteKaydiResponse>
{
    public Guid Id { get; set; }

    public class GetByIdOtoriteKaydiQueryHandler : IRequestHandler<GetByIdOtoriteKaydiQuery, GetByIdOtoriteKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly OtoriteKaydiBusinessRules _otoriteKaydiBusinessRules;

        public GetByIdOtoriteKaydiQueryHandler(IMapper mapper, IOtoriteKaydiRepository otoriteKaydiRepository, OtoriteKaydiBusinessRules otoriteKaydiBusinessRules)
        {
            _mapper = mapper;
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _otoriteKaydiBusinessRules = otoriteKaydiBusinessRules;
        }

        public async Task<GetByIdOtoriteKaydiResponse> Handle(GetByIdOtoriteKaydiQuery request, CancellationToken cancellationToken)
        {
            OtoriteKaydi? otoriteKaydi = await _otoriteKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _otoriteKaydiBusinessRules.OtoriteKaydiShouldExistWhenSelected(otoriteKaydi);

            GetByIdOtoriteKaydiResponse response = _mapper.Map<GetByIdOtoriteKaydiResponse>(otoriteKaydi);
            return response;
        }
    }
}
