using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.OduncIslemler.Commands.Create;

public class CreatedOduncIslemiResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public OduncDurumu Durumu { get; set; }
    public string? Not { get; set; }
}
