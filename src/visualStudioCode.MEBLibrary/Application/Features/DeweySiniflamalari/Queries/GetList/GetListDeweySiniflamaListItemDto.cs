using NArchitecture.Core.Application.Dtos;

namespace Application.Features.DeweySiniflamalari.Queries.GetList;

public class GetListDeweySiniflamaListItemDto : IDto
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public Guid? UstSinifId { get; set; }
}
