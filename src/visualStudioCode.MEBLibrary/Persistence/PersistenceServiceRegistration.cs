using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Persistence.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("BaseDb");
        services.AddDbContext<BaseDbContext>(options =>
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                options.UseInMemoryDatabase("BaseDb");
            }
            else
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }
        });
        services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());

        services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IOtpAuthenticatorRepository, OtpAuthenticatorRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();

        services.AddScoped<IYazarRepository, YazarRepository>();
        services.AddScoped<IDeweySiniflamaRepository, DeweySiniflamaRepository>();
        services.AddScoped<IEtkinlikRepository, EtkinlikRepository>();
        services.AddScoped<IKatalogKaydiRepository, KatalogKaydiRepository>();
        services.AddScoped<IKatalogKaydiYazarRepository, KatalogKaydiYazarRepository>();
        services.AddScoped<IKatalogKonuRepository, KatalogKonuRepository>();
        services.AddScoped<IKutuphaneRepository, KutuphaneRepository>();
        services.AddScoped<IKutuphaneBolumuRepository, KutuphaneBolumuRepository>();
        services.AddScoped<IMateryalRepository, MateryalRepository>();
        services.AddScoped<IMateryalEtiketRepository, MateryalEtiketRepository>();
        services.AddScoped<IMateryalFormatDetayRepository, MateryalFormatDetayRepository>();
        services.AddScoped<INushaRepository, NushaRepository>();
        services.AddScoped<IOduncIslemiRepository, OduncIslemiRepository>();
        services.AddScoped<IOtoriteKaydiRepository, OtoriteKaydiRepository>();
        services.AddScoped<IRafRepository, RafRepository>();
        services.AddScoped<IRezervasyonRepository, RezervasyonRepository>();
        services.AddScoped<IYeniKatalogTalebiRepository, YeniKatalogTalebiRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        return services;
    }
}
