using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Events;

public record CatalogApprovedNotification(
    Guid TalepId,
    Guid TalepEdenKutuphaneId,
    Guid KatalogKaydiId,
    Guid MateryalId
) : INotification;
