using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Rezervasyonlar.Commands.Update;

public class UpdatedRezervasyonResponse : IResponse
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
