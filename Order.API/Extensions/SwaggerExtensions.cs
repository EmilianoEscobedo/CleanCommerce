namespace Order.API.Extensions;

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
                Title = "Order API",
                Version = "v1",
                Description = "Order Management API for CleanCommerce. Handles creation, tracking, and management of customer orders.",
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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
        });
    }
}
