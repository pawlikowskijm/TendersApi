using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TendersApi.Application.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(opt => opt.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
