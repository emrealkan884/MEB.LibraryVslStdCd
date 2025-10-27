using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.YeniKatalogTalepleri;

public class YeniKatalogTalebiWorkflowService : IYeniKatalogTalebiWorkflowService
{
    private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
    private readonly IKatalogKaydiRepository _katalogKaydiRepository;
    private readonly IMateryalRepository _materyalRepository;
    private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

    public YeniKatalogTalebiWorkflowService(
        IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
        IKatalogKaydiRepository katalogKaydiRepository,
        IMateryalRepository materyalRepository,
        YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules
    )
    {
        _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
        _katalogKaydiRepository = katalogKaydiRepository;
        _materyalRepository = materyalRepository;
        _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
    }

    public async Task<YeniKatalogTalebiApprovalResult> ApproveAsync(
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
    )
    {
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldBeApprovable(talep);

        DateTime now = DateTime.UtcNow;

        KatalogKaydi katalogKaydi = new()
        {
            Id = Guid.NewGuid(),
            KutuphaneId = onaylayanKutuphaneId,
            Baslik = talep.Baslik,
            AltBaslik = talep.AltBaslik,
            Isbn = talep.Isbn,
            Yayinevi = talep.Yayinevi,
            YayinYeri = talep.YayinYeri,
            YayinYili = talep.YayinYili,
            Dil = talep.Dil,
            MateryalTuru = materyalTuru,
            MateryalAltTuru = materyalAltTuru ?? talep.MateryalAltTuru,
            Marc21Verisi = marc21Verisi,
            RdaUyumlu = rdaUyumlu,
            DeweySiniflamaId = deweySiniflamaId,
            SayfaSayisi = sayfaSayisi,
            Notlar = notlar,
            KayitTarihi = now,
            YeniKatalogTalebiId = talep.Id,
            CreatedDate = now
        };

        KatalogKaydi addedKatalogKaydi = await _katalogKaydiRepository.AddAsync(katalogKaydi);

        Materyal materyal = new()
        {
            Id = Guid.NewGuid(),
            KatalogKaydiId = addedKatalogKaydi.Id,
            KutuphaneId = talep.TalepEdenKutuphaneId,
            KutuphaneBolumuId = null,
            Formati = MapMateryalFormati(materyalTuru),
            RezervasyonaAcik = true,
            MaksimumOduncSuresiGun = materyalTuru == MateryalTuru.SureliYayin ? 7 : 14,
            Not = "Yeni katalog talebi onayı ile otomatik oluşturuldu",
            CreatedDate = now
        };

        Materyal addedMateryal = await _materyalRepository.AddAsync(materyal);

        talep.KatalogKaydinaBagla(addedKatalogKaydi.Id);

        await _yeniKatalogTalebiRepository.UpdateAsync(talep);

        return new YeniKatalogTalebiApprovalResult(talep, addedKatalogKaydi, addedMateryal);
    }

    public async Task<YeniKatalogTalebi> RejectAsync(
        YeniKatalogTalebi talep,
        string gerekce,
        CancellationToken cancellationToken
    )
    {
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldNotHaveCatalogRecord(talep);
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldNotBeFinalized(talep);
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiRejectReasonShouldBeProvided(gerekce);

        talep.TalebiReddet(gerekce);

        await _yeniKatalogTalebiRepository.UpdateAsync(talep);

        return talep;
    }

    public async Task<YeniKatalogTalebi> StartReviewAsync(
        YeniKatalogTalebi talep,
        CancellationToken cancellationToken
    )
    {
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldNotBeFinalized(talep);
        await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldBePendingForReview(talep);

        talep.TalebiIncelemeyeAl();

        await _yeniKatalogTalebiRepository.UpdateAsync(talep);

        return talep;
    }

    private static MateryalFormati MapMateryalFormati(MateryalTuru materyalTuru) =>
        materyalTuru switch
        {
            MateryalTuru.Kitap => MateryalFormati.Kitap,
            MateryalTuru.SureliYayin => MateryalFormati.SureliYayin,
            MateryalTuru.GorselMateryal => MateryalFormati.Diger,
            MateryalTuru.Multimedya => MateryalFormati.Video,
            MateryalTuru.EKitap => MateryalFormati.ElektronikKaynak,
            MateryalTuru.Harita => MateryalFormati.Diger,
            MateryalTuru.ElYazmasi => MateryalFormati.Diger,
            _ => MateryalFormati.Diger
        };
}
