using NArchitecture.Core.Application.Responses;

namespace Application.Features.Etkinlikler.Commands.Delete;

public class DeletedEtkinlikResponse : IResponse
{
    public Guid Id { get; set; }
}
