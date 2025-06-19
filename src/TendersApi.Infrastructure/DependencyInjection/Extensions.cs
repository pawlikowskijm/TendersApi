using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using TendersApi.Application.Interfaces.Repositories;
using TendersApi.Infrastructure.TendersWebApi.Options;
using TendersApi.Infrastructure.TendersWebApi.Repositories;

namespace TendersApi.Infrastructure.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton<ITenderRepository, TendersGuruApiService>();
        services.AddSingleton<IRestClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TendersGuruApiOptions>>().Value;
            return new RestClient(options.Host);
        });

        return services;
    }
}
