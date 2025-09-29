using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.API.Extensions;
using Product.API.Middlewares;
using Product.Application;
using Product.Application.DTOs;
using Product.Application.Mappings;
using Product.Application.Validators;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// Serilog
builder.Logging.ClearProviders();

var logsOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level: u3}] {Username} {Message:lj}{NewLine}{Exception}";
var loggingConfig = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: logsOutputTemplate)
    .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: logsOutputTemplate)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.AddSerilog(loggingConfig);

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Product.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Validators
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductStockDto>, UpdateProductStockDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

// MediatR
builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        config.RegisterServicesFromAssembly(typeof(Register).Assembly);
    }
);

var app = builder.Build();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();