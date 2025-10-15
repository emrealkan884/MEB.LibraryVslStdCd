using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalEtiketleri.Commands.Update;

public class UpdatedMateryalEtiketResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }
}
