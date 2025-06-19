using TendersApi.Application.DependencyInjection;
using TendersApi.Infrastructure.DependencyInjection;
using TendersApi.WebApi.DependencyInjection;
using TendersApi.WebApi.HostedServices;
using TendersApi.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.AddDistributedMemoryCache();

builder.Services
    .AddInfrastructureDependencies()
    .AddApplicationDependencies()
    .AddWebApiDependencies(builder.Configuration);

builder.Services.AddHostedService<DataInitializationService>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
