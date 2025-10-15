using NArchitecture.Core.Application.Responses;

namespace Application.Features.Kutuphaneler.Commands.Delete;

public class DeletedKutuphaneResponse : IResponse
{
    public Guid Id { get; set; }
}
