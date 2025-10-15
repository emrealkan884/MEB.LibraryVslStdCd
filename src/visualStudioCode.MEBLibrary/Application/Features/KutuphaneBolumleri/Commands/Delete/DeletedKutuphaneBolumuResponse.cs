using NArchitecture.Core.Application.Responses;

namespace Application.Features.KutuphaneBolumleri.Commands.Delete;

public class DeletedKutuphaneBolumuResponse : IResponse
{
    public Guid Id { get; set; }
}
