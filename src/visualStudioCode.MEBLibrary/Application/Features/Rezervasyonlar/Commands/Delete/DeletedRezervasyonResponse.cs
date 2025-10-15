using NArchitecture.Core.Application.Responses;

namespace Application.Features.Rezervasyonlar.Commands.Delete;

public class DeletedRezervasyonResponse : IResponse
{
    public Guid Id { get; set; }
}
