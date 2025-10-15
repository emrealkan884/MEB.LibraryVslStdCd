using NArchitecture.Core.Application.Responses;

namespace Application.Features.Yazarlar.Commands.Create;

public class CreatedYazarResponse : IResponse
{
    public Guid Id { get; set; }
    public string AdSoyad { get; set; }
    public DateTime? DogumTarihi { get; set; }
    public string? Uyruk { get; set; }
    public string? Aciklama { get; set; }
}