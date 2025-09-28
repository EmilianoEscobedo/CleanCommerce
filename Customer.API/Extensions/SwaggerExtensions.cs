namespace Customer.API.Extensions;

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
                Title = "Customer API",
                Version = "v1",
                Description = "Customer Management API for CleanCommerce. Handles customer profiles and related operations.",
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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API v1");
        });
    }
}