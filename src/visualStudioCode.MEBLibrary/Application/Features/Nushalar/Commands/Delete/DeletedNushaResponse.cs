using NArchitecture.Core.Application.Responses;

namespace Application.Features.Nushalar.Commands.Delete;

public class DeletedNushaResponse : IResponse
{
    public Guid Id { get; set; }
}
