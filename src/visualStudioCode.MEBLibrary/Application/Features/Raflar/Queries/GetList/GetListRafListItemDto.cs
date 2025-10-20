using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Raflar.Queries.GetList;

public class GetListRafListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KutuphaneBolumuId { get; set; }
    public required string Kod { get; set; }
    public string? Aciklama { get; set; }
}
