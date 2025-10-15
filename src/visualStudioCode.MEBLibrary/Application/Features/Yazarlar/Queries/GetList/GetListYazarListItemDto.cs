using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Yazarlar.Queries.GetList;

public class GetListYazarListItemDto : IDto
{
    public Guid Id { get; set; }
    public string AdSoyad { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? Uyruk { get; set; }
    public string? Aciklama { get; set; }
}