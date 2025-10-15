using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Materyaller.Queries.GetList;

public class GetListMateryalListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid? KutuphaneBolumuId { get; set; }
    public MateryalFormati Formati { get; set; }
    public bool RezervasyonaAcik { get; set; }
    public int MaksimumOduncSuresiGun { get; set; }
    public string? Not { get; set; }
}
