using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Update;

public class UpdatedKatalogKaydiYazarResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid YazarId { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
    public YazarRolu Rol { get; set; }
    public int Sira { get; set; }
}
