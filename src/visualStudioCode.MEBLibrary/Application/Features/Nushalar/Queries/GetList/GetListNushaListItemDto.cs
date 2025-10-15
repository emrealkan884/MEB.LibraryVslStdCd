using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Nushalar.Queries.GetList;

public class GetListNushaListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid MateryalId { get; set; }
    public Guid? RafId { get; set; }
    public required string Barkod { get; set; }
    public NushaDurumu Durumu { get; set; }
    public DateTime EklenmeTarihi { get; set; }
}
