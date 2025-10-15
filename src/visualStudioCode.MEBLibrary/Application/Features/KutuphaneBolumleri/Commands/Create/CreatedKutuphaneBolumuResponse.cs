using NArchitecture.Core.Application.Responses;

namespace Application.Features.KutuphaneBolumleri.Commands.Create;

public class CreatedKutuphaneBolumuResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }
}
