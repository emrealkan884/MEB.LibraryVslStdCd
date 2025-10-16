using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Raporlama.Queries.GetOverdueLoans;

public class OverdueLoanReportDto : IDto
{
    public Guid OduncIslemiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public Guid? MateryalId { get; set; }
    public string? MateryalBaslik { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public int GecikenGun { get; set; }
}
