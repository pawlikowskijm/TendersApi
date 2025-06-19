using TendersApi.Application.Options;
using TendersApi.Infrastructure.TendersWebApi.Options;
using TendersApi.WebApi.Middlewares;

namespace TendersApi.WebApi.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddWebApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TendersGuruApiOptions>(configuration.GetSection(nameof(TendersGuruApiOptions)));
        services.Configure<TendersQueryingOptions>(configuration.GetSection(nameof(TendersQueryingOptions)));
        services.AddTransient<ExceptionHandlingMiddleware>();

        return services;
    }
}
