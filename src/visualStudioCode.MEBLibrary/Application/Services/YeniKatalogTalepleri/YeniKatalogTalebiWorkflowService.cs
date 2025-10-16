using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.YeniKatalogTalepleri;

public class YeniKatalogTalebiWorkflowService : IYeniKatalogTalebiWorkflowService
{
    private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
    private readonly IKatalogKaydiRepository _katalogKaydiRepository;
    private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

    public YeniKatalogTalebiWorkflowService(
        IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
        IKatalogKaydiRepository katalogKaydiRepository,
        YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules
    )
    {
        _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
        _katalogKaydiRepository = katalogKaydiRepository;
        _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
    }

    public async Task<YeniKatalogTalebi> ApproveAsync(
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

        talep.KatalogKaydinaBagla(addedKatalogKaydi.Id);

        await _yeniKatalogTalebiRepository.UpdateAsync(talep);

        return talep;
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
}
