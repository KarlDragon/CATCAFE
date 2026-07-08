using BE.Services.Implementations;
using BE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BE.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegistrationFilter(this IServiceCollection services)
    {
        services.AddSingleton<IRegistrationFilterService, RegistrationFilterService>();
        services.AddHostedService<RegistrationFilterInitializationHostedService>();
        return services;
    }
}
