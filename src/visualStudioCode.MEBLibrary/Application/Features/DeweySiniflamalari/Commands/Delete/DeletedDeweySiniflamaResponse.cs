using NArchitecture.Core.Application.Responses;

namespace Application.Features.DeweySiniflamalari.Commands.Delete;

public class DeletedDeweySiniflamaResponse : IResponse
{
    public Guid Id { get; set; }
}
