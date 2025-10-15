using Application.Features.Yazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Yazarlar.Queries.GetById;

public class GetByIdYazarQuery : IRequest<GetByIdYazarResponse>
{
    public Guid Id { get; set; }

    public class GetByIdYazarQueryHandler : IRequestHandler<GetByIdYazarQuery, GetByIdYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYazarRepository _yazarRepository;
        private readonly YazarBusinessRules _yazarBusinessRules;

        public GetByIdYazarQueryHandler(IMapper mapper, IYazarRepository yazarRepository, YazarBusinessRules yazarBusinessRules)
        {
            _mapper = mapper;
            _yazarRepository = yazarRepository;
            _yazarBusinessRules = yazarBusinessRules;
        }

        public async Task<GetByIdYazarResponse> Handle(GetByIdYazarQuery request, CancellationToken cancellationToken)
        {
            Yazar? yazar = await _yazarRepository.GetAsync(predicate: y => y.Id == request.Id, cancellationToken: cancellationToken);
            await _yazarBusinessRules.YazarShouldExistWhenSelected(yazar);

            GetByIdYazarResponse response = _mapper.Map<GetByIdYazarResponse>(yazar);
            return response;
        }
    }
}