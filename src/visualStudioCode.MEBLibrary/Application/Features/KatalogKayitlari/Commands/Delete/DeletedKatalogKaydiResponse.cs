using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKayitlari.Commands.Delete;

public class DeletedKatalogKaydiResponse : IResponse
{
    public Guid Id { get; set; }
}
