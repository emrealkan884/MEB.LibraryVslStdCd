using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKonulari.Commands.Delete;

public class DeletedKatalogKonuResponse : IResponse
{
    public Guid Id { get; set; }
}
