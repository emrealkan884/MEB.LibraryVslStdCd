using Application.Features.Kutuphaneler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Kutuphaneler.Commands.Delete;

public class DeleteKutuphaneCommand : IRequest<DeletedKutuphaneResponse>
{
    public Guid Id { get; set; }

    public class DeleteKutuphaneCommandHandler : IRequestHandler<DeleteKutuphaneCommand, DeletedKutuphaneResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly KutuphaneBusinessRules _kutuphaneBusinessRules;

        public DeleteKutuphaneCommandHandler(IMapper mapper, IKutuphaneRepository kutuphaneRepository,
                                             KutuphaneBusinessRules kutuphaneBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneRepository = kutuphaneRepository;
            _kutuphaneBusinessRules = kutuphaneBusinessRules;
        }

        public async Task<DeletedKutuphaneResponse> Handle(DeleteKutuphaneCommand request, CancellationToken cancellationToken)
        {
            Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBusinessRules.KutuphaneShouldExistWhenSelected(kutuphane);

            await _kutuphaneRepository.DeleteAsync(kutuphane!);

            DeletedKutuphaneResponse response = _mapper.Map<DeletedKutuphaneResponse>(kutuphane);
            return response;
        }
    }
}
