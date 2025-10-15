using NArchitecture.Core.Application.Responses;

namespace Application.Features.KutuphaneBolumleri.Commands.Update;

public class UpdatedKutuphaneBolumuResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }
}
