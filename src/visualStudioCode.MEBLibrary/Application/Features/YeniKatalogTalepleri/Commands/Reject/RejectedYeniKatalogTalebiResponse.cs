using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.YeniKatalogTalepleri.Commands.Reject;

public class RejectedYeniKatalogTalebiResponse : IResponse
{
    public Guid Id { get; set; }
    public TalepDurumu Durum { get; set; }
    public string? RedGerekcesi { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
}
