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
                AuthorizationPolicies.BakanlikYetkisi,
                policy => policy.RequireRole(ApplicationRoles.SistemYoneticisi, ApplicationRoles.BakanlikYetkilisi)
            );

            options.AddPolicy(
                AuthorizationPolicies.IlYetkisiVeyaUstu,
                policy => policy.RequireRole(
                    ApplicationRoles.SistemYoneticisi,
                    ApplicationRoles.BakanlikYetkilisi,
                    ApplicationRoles.IlYetkilisi
                )
            );

            options.AddPolicy(
                AuthorizationPolicies.IlceYetkisiVeyaUstu,
                policy => policy.RequireRole(
                    ApplicationRoles.SistemYoneticisi,
                    ApplicationRoles.BakanlikYetkilisi,
                    ApplicationRoles.IlYetkilisi,
                    ApplicationRoles.IlceYetkilisi
                )
            );

            options.AddPolicy(
                AuthorizationPolicies.OkulYetkisiVeyaUstu,
                policy => policy.RequireRole(
                    ApplicationRoles.SistemYoneticisi,
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
