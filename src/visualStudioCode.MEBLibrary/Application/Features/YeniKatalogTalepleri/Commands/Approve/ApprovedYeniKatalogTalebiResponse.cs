using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.YeniKatalogTalepleri.Commands.Approve;

public class ApprovedYeniKatalogTalebiResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid OnaylayanKutuphaneId { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid MateryalId { get; set; }
    public TalepDurumu Durum { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
}
