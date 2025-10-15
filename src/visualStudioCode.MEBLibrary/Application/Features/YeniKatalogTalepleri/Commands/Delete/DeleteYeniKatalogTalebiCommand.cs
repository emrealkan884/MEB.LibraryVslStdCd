using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Delete;

public class DeleteYeniKatalogTalebiCommand : IRequest<DeletedYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }

    public class DeleteYeniKatalogTalebiCommandHandler : IRequestHandler<DeleteYeniKatalogTalebiCommand, DeletedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public DeleteYeniKatalogTalebiCommandHandler(IMapper mapper, IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
                                             YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules)
        {
            _mapper = mapper;
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
        }

        public async Task<DeletedYeniKatalogTalebiResponse> Handle(DeleteYeniKatalogTalebiCommand request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);

            await _yeniKatalogTalebiRepository.DeleteAsync(yeniKatalogTalebi!);

            DeletedYeniKatalogTalebiResponse response = _mapper.Map<DeletedYeniKatalogTalebiResponse>(yeniKatalogTalebi);
            return response;
        }
    }
}
