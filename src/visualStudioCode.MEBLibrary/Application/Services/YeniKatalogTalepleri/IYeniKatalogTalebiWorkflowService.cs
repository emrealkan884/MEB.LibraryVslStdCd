using Domain.Entities;
using Domain.Enums;

namespace Application.Services.YeniKatalogTalepleri;

public interface IYeniKatalogTalebiWorkflowService
{
    Task<YeniKatalogTalebi> ApproveAsync(
        YeniKatalogTalebi talep,
        Guid onaylayanKutuphaneId,
        MateryalTuru materyalTuru,
        string? materyalAltTuru,
        Guid? deweySiniflamaId,
        string? marc21Verisi,
        bool rdaUyumlu,
        int? sayfaSayisi,
        string? notlar,
        CancellationToken cancellationToken
    );

    Task<YeniKatalogTalebi> RejectAsync(
        YeniKatalogTalebi talep,
        string gerekce,
        CancellationToken cancellationToken
    );
}
