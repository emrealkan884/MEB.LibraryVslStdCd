using Application.Features.MateryalFormatDetaylari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalFormatDetaylari.Commands.Delete;

public class DeleteMateryalFormatDetayCommand : IRequest<DeletedMateryalFormatDetayResponse>
{
    public Guid Id { get; set; }

    public class DeleteMateryalFormatDetayCommandHandler : IRequestHandler<DeleteMateryalFormatDetayCommand, DeletedMateryalFormatDetayResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly MateryalFormatDetayBusinessRules _materyalFormatDetayBusinessRules;

        public DeleteMateryalFormatDetayCommandHandler(IMapper mapper, IMateryalFormatDetayRepository materyalFormatDetayRepository,
                                             MateryalFormatDetayBusinessRules materyalFormatDetayBusinessRules)
        {
            _mapper = mapper;
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _materyalFormatDetayBusinessRules = materyalFormatDetayBusinessRules;
        }

        public async Task<DeletedMateryalFormatDetayResponse> Handle(DeleteMateryalFormatDetayCommand request, CancellationToken cancellationToken)
        {
            MateryalFormatDetay? materyalFormatDetay = await _materyalFormatDetayRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalFormatDetayBusinessRules.MateryalFormatDetayShouldExistWhenSelected(materyalFormatDetay);

            await _materyalFormatDetayRepository.DeleteAsync(materyalFormatDetay!);

            DeletedMateryalFormatDetayResponse response = _mapper.Map<DeletedMateryalFormatDetayResponse>(materyalFormatDetay);
            return response;
        }
    }
}
