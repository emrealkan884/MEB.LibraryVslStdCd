using NArchitecture.Core.Application.Responses;

namespace Application.Features.Materyaller.Commands.Delete;

public class DeletedMateryalResponse : IResponse
{
    public Guid Id { get; set; }
}
