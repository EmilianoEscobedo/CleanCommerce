namespace Security.API.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class SwaggerExtensions
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Security API",
                Version = "v1",
                Description = "Authentication and Authorization API for CleanCommerce. Manages user registration, login, and JWT validation.",
                Contact = new OpenApiContact
                {
                    Name = "Emiliano Escobedo",
                    Email = "emiliano.escobedo@istea.com.ar",
                    Url = new Uri("https://github.com/EmilianoEscobedo")
                }
            });
        });
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Security API v1");
        });
    }
}
