using Application.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Configurations;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddLibraryAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthorizationPolicies.RequireMinistry,
                policy => policy.RequireRole(ApplicationRoles.BakanlikYetkilisi)
            );

            options.AddPolicy(
                AuthorizationPolicies.RequireProvinceOrAbove,
                policy => policy.RequireRole(ApplicationRoles.BakanlikYetkilisi, ApplicationRoles.IlYetkilisi)
            );

            options.AddPolicy(
                AuthorizationPolicies.RequireDistrictOrAbove,
                policy => policy.RequireRole(
                    ApplicationRoles.BakanlikYetkilisi,
                    ApplicationRoles.IlYetkilisi,
                    ApplicationRoles.IlceYetkilisi
                )
            );

            options.AddPolicy(
                AuthorizationPolicies.RequireSchoolOrAbove,
                policy => policy.RequireRole(
                    ApplicationRoles.BakanlikYetkilisi,
                    ApplicationRoles.IlYetkilisi,
                    ApplicationRoles.IlceYetkilisi,
                    ApplicationRoles.OkulKutuphaneYoneticisi
                )
            );
        });

        return services;
    }
}
