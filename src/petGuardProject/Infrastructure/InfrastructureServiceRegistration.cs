using Application.Services.ImageService;
using Application.Services.MernisService;
using Infrastructure.Adapters.ImageService;
using Infrastructure.Adapters.MernisService;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;


public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddScoped<MernisServiceBase, NVIMernisServiceAdapter>();

        return services;
    }
}