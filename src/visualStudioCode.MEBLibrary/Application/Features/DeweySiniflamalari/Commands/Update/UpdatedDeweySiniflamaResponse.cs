using NArchitecture.Core.Application.Responses;

namespace Application.Features.DeweySiniflamalari.Commands.Update;

public class UpdatedDeweySiniflamaResponse : IResponse
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public Guid? UstSinifId { get; set; }
}
