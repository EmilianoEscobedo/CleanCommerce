using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.API.Extensions;
using Order.API.Middlewares;
using Order.Application;
using Order.Application.DTOs;
using Order.Application.Mapping;
using Order.Application.Services;
using Order.Application.Validators;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using Order.Infrastructure.Services;
using Order.Infrastructure.Settings;
using Order.Infrastructure.UnitOfWork;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Order.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// External services
builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICustomerService, CustomerService>();

// Validators
builder.Services.AddScoped<IValidator<CreateOrderRequestDto>, CreateOrderDtoValidator>();
builder.Services.AddScoped<IValidator<CreateOrderItemRequestDto>, OrderItemDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));

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
        var dbContext = services.GetRequiredService<OrderDbContext>();
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