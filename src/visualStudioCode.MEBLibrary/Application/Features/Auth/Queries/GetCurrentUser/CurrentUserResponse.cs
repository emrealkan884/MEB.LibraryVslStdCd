using System;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Auth.Queries.GetCurrentUser;

public class CurrentUserResponse : IResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
    public string LibraryType { get; set; } = string.Empty;
    public string? SchoolCode { get; set; }
}
