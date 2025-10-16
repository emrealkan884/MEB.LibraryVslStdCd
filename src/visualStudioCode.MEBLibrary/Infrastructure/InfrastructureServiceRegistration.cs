using Application.Services.ImageService;
using Infrastructure.Adapters.ImageService;
using Application.Services.Reporting;
using Infrastructure.Adapters.Reporting;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddSingleton<IReportExportService, CsvReportExportService>();

        return services;
    }
}
