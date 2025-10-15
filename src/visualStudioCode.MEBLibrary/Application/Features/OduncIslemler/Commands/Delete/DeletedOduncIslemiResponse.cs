using NArchitecture.Core.Application.Responses;

namespace Application.Features.OduncIslemler.Commands.Delete;

public class DeletedOduncIslemiResponse : IResponse
{
    public Guid Id { get; set; }
}
