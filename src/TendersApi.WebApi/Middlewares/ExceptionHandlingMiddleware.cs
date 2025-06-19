using FluentValidation;
using TendersApi.Application.Models;

namespace TendersApi.WebApi.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException vex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var message = string.Join(Environment.NewLine, vex.Errors.Select(x => x.ErrorMessage));

            if (string.IsNullOrWhiteSpace(message))
            {
                message = vex.Message;
            }

            await context.Response.WriteAsJsonAsync(new ExceptionModel(message));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new ExceptionModel(ex.Message));
        }
    }
}
