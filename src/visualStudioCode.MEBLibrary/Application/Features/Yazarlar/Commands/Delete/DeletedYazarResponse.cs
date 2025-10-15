using NArchitecture.Core.Application.Responses;

namespace Application.Features.Yazarlar.Commands.Delete;

public class DeletedYazarResponse : IResponse
{
    public Guid Id { get; set; }
}