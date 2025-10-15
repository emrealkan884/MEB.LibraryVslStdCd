using Application.Features.Yazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Yazarlar.Commands.Delete;

public class DeleteYazarCommand : IRequest<DeletedYazarResponse>
{
    public Guid Id { get; set; }

    public class DeleteYazarCommandHandler : IRequestHandler<DeleteYazarCommand, DeletedYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYazarRepository _yazarRepository;
        private readonly YazarBusinessRules _yazarBusinessRules;

        public DeleteYazarCommandHandler(IMapper mapper, IYazarRepository yazarRepository,
                                         YazarBusinessRules yazarBusinessRules)
        {
            _mapper = mapper;
            _yazarRepository = yazarRepository;
            _yazarBusinessRules = yazarBusinessRules;
        }

        public async Task<DeletedYazarResponse> Handle(DeleteYazarCommand request, CancellationToken cancellationToken)
        {
            Yazar? yazar = await _yazarRepository.GetAsync(predicate: y => y.Id == request.Id, cancellationToken: cancellationToken);
            await _yazarBusinessRules.YazarShouldExistWhenSelected(yazar);

            await _yazarRepository.DeleteAsync(yazar!);

            DeletedYazarResponse response = _mapper.Map<DeletedYazarResponse>(yazar);
            return response;
        }
    }
}