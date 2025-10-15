using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalFormatDetaylari.Commands.Delete;

public class DeletedMateryalFormatDetayResponse : IResponse
{
    public Guid Id { get; set; }
}
