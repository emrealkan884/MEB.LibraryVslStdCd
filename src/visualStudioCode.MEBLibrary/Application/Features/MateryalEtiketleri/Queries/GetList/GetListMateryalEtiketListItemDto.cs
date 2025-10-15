using NArchitecture.Core.Application.Dtos;

namespace Application.Features.MateryalEtiketleri.Queries.GetList;

public class GetListMateryalEtiketListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }
}
