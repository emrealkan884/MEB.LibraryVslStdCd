using NArchitecture.Core.Application.Responses;

namespace Application.Features.YeniKatalogTalepleri.Commands.Delete;

public class DeletedYeniKatalogTalebiResponse : IResponse
{
    public Guid Id { get; set; }
}
