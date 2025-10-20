using NArchitecture.Core.Application.Responses;

namespace Application.Features.Raflar.Queries.GetById;

public class GetByIdRafResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KutuphaneBolumuId { get; set; }
    public required string Kod { get; set; }
    public string? Aciklama { get; set; }
}
