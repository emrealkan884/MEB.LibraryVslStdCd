using NArchitecture.Core.Application.Responses;

namespace Application.Features.KutuphaneBolumleri.Queries.GetById;

public class GetByIdKutuphaneBolumuResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Ad { get; set; }
    public string? Aciklama { get; set; }
}
