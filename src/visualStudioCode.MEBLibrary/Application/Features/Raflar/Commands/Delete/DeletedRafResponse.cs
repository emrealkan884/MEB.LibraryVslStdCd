using NArchitecture.Core.Application.Responses;

namespace Application.Features.Raflar.Commands.Delete;

public class DeletedRafResponse : IResponse
{
    public Guid Id { get; set; }
}
