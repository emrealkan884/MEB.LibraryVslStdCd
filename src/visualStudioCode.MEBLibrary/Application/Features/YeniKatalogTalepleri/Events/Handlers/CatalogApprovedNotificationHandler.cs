using Application.Features.YeniKatalogTalepleri.Events;
using Application.Services.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.YeniKatalogTalepleri.Events.Handlers;

public class CatalogApprovedNotificationHandler : INotificationHandler<CatalogApprovedNotification>
{
    private readonly ILogger<CatalogApprovedNotificationHandler> _logger;
    private readonly IKutuphaneRepository _kutuphaneRepository;

    public CatalogApprovedNotificationHandler(
        ILogger<CatalogApprovedNotificationHandler> logger,
        IKutuphaneRepository kutuphaneRepository
    )
    {
        _logger = logger;
        _kutuphaneRepository = kutuphaneRepository;
    }

    public async Task Handle(CatalogApprovedNotification notification, CancellationToken cancellationToken)
    {
        var kutuphane = await _kutuphaneRepository.GetAsync(
            predicate: k => k.Id == notification.TalepEdenKutuphaneId,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        string kutuphaneAdi = kutuphane?.Ad ?? notification.TalepEdenKutuphaneId.ToString();

        _logger.LogInformation(
            "Yeni katalog talebi onaylandı. TalepId: {TalepId}, KatalogKaydiId: {KatalogKaydiId}, MateryalId: {MateryalId}, TalepEdenKutuphane: {Kutuphane}",
            notification.TalepId,
            notification.KatalogKaydiId,
            notification.MateryalId,
            kutuphaneAdi
        );
    }
}
