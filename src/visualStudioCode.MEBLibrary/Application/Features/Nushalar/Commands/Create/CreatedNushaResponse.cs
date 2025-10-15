using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Nushalar.Commands.Create;

public class CreatedNushaResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public Guid? RafId { get; set; }
    public required string Barkod { get; set; }
    public NushaDurumu Durumu { get; set; }
    public DateTime EklenmeTarihi { get; set; }
}
