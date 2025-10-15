using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Rezervasyonlar.Queries.GetList;

public class GetListRezervasyonListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid MateryalId { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? HazirlanmaTarihi { get; set; }
    public DateTime? TamamlanmaTarihi { get; set; }
    public RezervasyonDurumu Durumu { get; set; }
    public string? Not { get; set; }
}
