using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Customer.API.Extensions;
using Customer.API.Middlewares;
using Customer.Application;
using Customer.Application.DTOs;
using Customer.Application.Mappings;
using Customer.Application.Validators;
using Customer.Domain.Repositories;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// Serilog
builder.Logging.ClearProviders();

var loggingConfig = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level: u3}] {Username} {Message:lj}{NewLine}{Exception}")
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.AddSerilog(loggingConfig);

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Customer.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Validators
builder.Services.AddScoped<IValidator<CreateCustomerRequestDto>, CreateCustomerDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerRequestDto>, UpdateCustomerDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));

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
        var dbContext = services.GetRequiredService<CustomerDbContext>();
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