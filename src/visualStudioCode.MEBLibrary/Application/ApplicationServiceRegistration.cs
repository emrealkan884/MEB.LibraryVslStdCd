using System.Reflection;
using Application.Pipelines.Audit;
using Application.Services.AuthenticatorService;
using Application.Services.AuthService;
using Application.Services.DeweySiniflamalari;
using Application.Services.Etkinlikler;
using Application.Services.UsersService;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Application.Pipelines.Validation;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Abstraction;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Configurations;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Serilog.File;
using NArchitecture.Core.ElasticSearch;
using NArchitecture.Core.ElasticSearch.Models;
using NArchitecture.Core.Localization.Resource.Yaml.DependencyInjection;
using NArchitecture.Core.Mailing;
using NArchitecture.Core.Mailing.MailKit;
using NArchitecture.Core.Security.DependencyInjection;
using NArchitecture.Core.Security.JWT;
using Application.Services.KatalogKaydiYazarlar;
using Application.Services.KatalogKayitlari;
using Application.Services.KatalogKonulari;
using Application.Services.KutuphaneBolumleri;
using Application.Services.Kutuphaneler;
using Application.Services.MateryalEtiketleri;
using Application.Services.MateryalFormatDetaylari;
using Application.Services.Materyaller;
using Application.Services.Nushalar;
using Application.Services.OduncIslemleri;
using Application.Services.OtoriteKayitlari;
using Application.Services.Raflar;
using Application.Services.Rezervasyonlar;
using Application.Services.Yazarlar;
using Application.Services.YeniKatalogTalepleri;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        MailSettings mailSettings,
        FileLogConfiguration fileLogConfiguration,
        ElasticSearchConfig elasticSearchConfig,
        TokenOptions tokenOptions
    )
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
            configuration.AddOpenBehavior(typeof(AuditLoggingBehavior<,>));
        });

        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMailService, MailKitMailService>(_ => new MailKitMailService(mailSettings));
        services.AddSingleton<ILogger, SerilogFileLogger>(_ => new SerilogFileLogger(fileLogConfiguration));
        services.AddSingleton<IElasticSearch, ElasticSearchManager>(_ => new ElasticSearchManager(elasticSearchConfig));

        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IAuthenticatorService, AuthenticatorManager>();
        services.AddScoped<IUserService, UserManager>();

        services.AddYamlResourceLocalization();

        services.AddSecurityServices<Guid, int, Guid>(tokenOptions);

        services.AddScoped<IYazarService, YazarManager>();
        services.AddScoped<IDeweySiniflamaService, DeweySiniflamaManager>();
        services.AddScoped<IEtkinlikService, EtkinlikManager>();
        services.AddScoped<IKatalogKaydiService, KatalogKaydiManager>();
        services.AddScoped<IKatalogKaydiYazarService, KatalogKaydiYazarManager>();
        services.AddScoped<IKatalogKonuService, KatalogKonuManager>();
        services.AddScoped<IKutuphaneService, KutuphaneManager>();
        services.AddScoped<IKutuphaneBolumuService, KutuphaneBolumuManager>();
        services.AddScoped<IMateryalService, MateryalManager>();
        services.AddScoped<IMateryalEtiketService, MateryalEtiketManager>();
        services.AddScoped<IMateryalFormatDetayService, MateryalFormatDetayManager>();
        services.AddScoped<INushaService, NushaManager>();
        services.AddScoped<IOduncIslemiService, OduncIslemiManager>();
        services.AddScoped<IOtoriteKaydiService, OtoriteKaydiManager>();
        services.AddScoped<IRafService, RafManager>();
        services.AddScoped<IRezervasyonService, RezervasyonManager>();
        services.AddScoped<IYeniKatalogTalebiService, YeniKatalogTalebiManager>();
        services.AddScoped<IYeniKatalogTalebiWorkflowService, YeniKatalogTalebiWorkflowService>();
        return services;
    }

    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
    )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (Type? item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, item);
        return services;
    }
}
