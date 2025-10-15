using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalEtiketleri.Commands.Delete;

public class DeletedMateryalEtiketResponse : IResponse
{
    public Guid Id { get; set; }
}
