using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalEtiketleri.Queries.GetById;

public class GetByIdMateryalEtiketResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }
}
