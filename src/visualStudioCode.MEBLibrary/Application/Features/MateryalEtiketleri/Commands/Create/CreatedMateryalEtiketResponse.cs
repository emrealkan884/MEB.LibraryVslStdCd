using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalEtiketleri.Commands.Create;

public class CreatedMateryalEtiketResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }
}
