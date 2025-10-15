using NArchitecture.Core.Application.Dtos;

namespace Application.Features.KutuphaneBolumleri.Queries.GetList;

public class GetListKutuphaneBolumuListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }
}
