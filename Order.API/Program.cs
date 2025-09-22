using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Application.DTOs;
using Order.Application.Mapping;
using Order.Application.Services;
using Order.Application.Validators;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.ExternalService;
using Order.Infrastructure.Repositories;
using Order.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Order.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application services
builder.Services.Configure<ExternalServiceSettings>(
    builder.Configuration.GetSection("ExternalServices"));
builder.Services.AddHttpClient<IExternalServicesClient, ExternalServicesClient>()
    .ConfigurePrimaryHttpMessageHandler(() => 
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

builder.Services.AddScoped<IOrderService, OrderService>();

// Validators
builder.Services.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
builder.Services.AddScoped<IValidator<OrderItemDto>, OrderItemDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
        var dbContext = services.GetRequiredService<OrderDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();