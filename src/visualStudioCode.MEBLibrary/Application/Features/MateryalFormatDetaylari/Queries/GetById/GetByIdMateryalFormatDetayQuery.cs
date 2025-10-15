using Application.Features.MateryalFormatDetaylari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalFormatDetaylari.Queries.GetById;

public class GetByIdMateryalFormatDetayQuery : IRequest<GetByIdMateryalFormatDetayResponse>
{
    public Guid Id { get; set; }

    public class GetByIdMateryalFormatDetayQueryHandler : IRequestHandler<GetByIdMateryalFormatDetayQuery, GetByIdMateryalFormatDetayResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly MateryalFormatDetayBusinessRules _materyalFormatDetayBusinessRules;

        public GetByIdMateryalFormatDetayQueryHandler(IMapper mapper, IMateryalFormatDetayRepository materyalFormatDetayRepository, MateryalFormatDetayBusinessRules materyalFormatDetayBusinessRules)
        {
            _mapper = mapper;
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _materyalFormatDetayBusinessRules = materyalFormatDetayBusinessRules;
        }

        public async Task<GetByIdMateryalFormatDetayResponse> Handle(GetByIdMateryalFormatDetayQuery request, CancellationToken cancellationToken)
        {
            MateryalFormatDetay? materyalFormatDetay = await _materyalFormatDetayRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalFormatDetayBusinessRules.MateryalFormatDetayShouldExistWhenSelected(materyalFormatDetay);

            GetByIdMateryalFormatDetayResponse response = _mapper.Map<GetByIdMateryalFormatDetayResponse>(materyalFormatDetay);
            return response;
        }
    }
}
