using NArchitecture.Core.Application.Responses;

namespace Application.Features.DeweySiniflamalari.Commands.Create;

public class CreatedDeweySiniflamaResponse : IResponse
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public int? UstSinifId { get; set; }
}
