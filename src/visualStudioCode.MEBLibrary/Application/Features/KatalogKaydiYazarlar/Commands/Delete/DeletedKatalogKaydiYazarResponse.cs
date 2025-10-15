using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Delete;

public class DeletedKatalogKaydiYazarResponse : IResponse
{
    public Guid Id { get; set; }
}
