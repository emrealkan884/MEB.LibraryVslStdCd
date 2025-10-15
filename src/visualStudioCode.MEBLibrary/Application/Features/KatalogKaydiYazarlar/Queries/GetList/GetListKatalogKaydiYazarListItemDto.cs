using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.KatalogKaydiYazarlar.Queries.GetList;

public class GetListKatalogKaydiYazarListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid YazarId { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
    public YazarRolu Rol { get; set; }
    public int Sira { get; set; }
}
