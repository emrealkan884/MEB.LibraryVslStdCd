using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Kutuphaneler.Queries.GetById;

public class GetByIdKutuphaneResponse : IResponse
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Ad { get; set; }
    public KutuphaneTipi Tip { get; set; }
    public required string Adres { get; set; }
    public string? Telefon { get; set; }
    public string? EPosta { get; set; }
    public bool Aktif { get; set; }
}
