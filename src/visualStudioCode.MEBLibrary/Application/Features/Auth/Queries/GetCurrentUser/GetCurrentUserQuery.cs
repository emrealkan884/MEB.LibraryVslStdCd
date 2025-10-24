using System.Linq;
using Application.Authorization;
using Application.Features.Auth.Rules;
using Application.Services.Repositories;
using Application.Services.UsersService;
using Domain.Entities.Security;
using MediatR;

namespace Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<CurrentUserResponse>
{
    public Guid UserId { get; set; }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly AuthBusinessRules _authBusinessRules;

        public GetCurrentUserQueryHandler(
            IUserService userService,
            IUserOperationClaimRepository userOperationClaimRepository,
            AuthBusinessRules authBusinessRules
        )
        {
            _userService = userService;
            _userOperationClaimRepository = userOperationClaimRepository;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<CurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(
                predicate: u => u.Id == request.UserId,
                cancellationToken: cancellationToken
            );

            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);

            IList<OperationClaim> operationClaims =
                await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(request.UserId);
            string[] roleNames = operationClaims.Select(claim => claim.Name).ToArray();

            string libraryType = roleNames.Contains(ApplicationRoles.BakanlikYetkilisi)
                ? "Merkez"
                : roleNames.Contains(ApplicationRoles.OkulKutuphaneYoneticisi)
                    ? "Okul"
                    : "Unknown";

            return new CurrentUserResponse
            {
                Id = user!.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Roles = roleNames,
                LibraryType = libraryType,
                SchoolCode = null
            };
        }
    }
}
