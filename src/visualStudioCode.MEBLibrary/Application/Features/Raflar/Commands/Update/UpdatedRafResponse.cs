using NArchitecture.Core.Application.Responses;

namespace Application.Features.Raflar.Commands.Update;

public class UpdatedRafResponse : IResponse
{
    public Guid Id { get; set; }
    public int KutuphaneBolumuId { get; set; }
    public required string Kod { get; set; }
    public string? Aciklama { get; set; }
}
