using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.YeniKatalogTalepleri.Commands.Review;

public class StartedReviewYeniKatalogTalebiResponse : IResponse
{
    public Guid Id { get; set; }
    public TalepDurumu Durum { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
}
