namespace Customer.API.Middlewares;

using Microsoft.AspNetCore.Http;
using Application.Services;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISecurityService securityService)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization header missing.");
            return;
        }

        var token = authHeader.ToString().Replace("Bearer ", "");

        if (!await securityService.ValidateTokenAsync(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or expired token.");
            return;
        }

        await _next(context);
    }
}
