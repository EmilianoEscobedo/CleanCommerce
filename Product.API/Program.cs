using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.API.Extensions;
using Product.Application.DTOs;
using Product.Application.Mappings;
using Product.Application.Services;
using Product.Application.Validators;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;
using Product.Infrastructure.Services;
using Product.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Product.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Application services
builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<ISecurityService, SecurityService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddScoped<IProductService, ProductService>();

// Validators
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductStockDto>, UpdateProductStockDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run required db migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ProductDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();