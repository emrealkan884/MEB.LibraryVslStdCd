using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Materyaller.Commands.Create;

public class CreatedMateryalResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid? KutuphaneBolumuId { get; set; }
    public MateryalFormati Formati { get; set; }
    public bool RezervasyonaAcik { get; set; }
    public int MaksimumOduncSuresiGun { get; set; }
    public string? Not { get; set; }
}
