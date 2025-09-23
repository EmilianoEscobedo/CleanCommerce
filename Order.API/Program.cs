using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.API.Extensions;
using Order.API.Middlewares;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

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
builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<IProductService, ProductService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddHttpClient<ICustomerService, CustomerService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddHttpClient<ISecurityService, SecurityService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddScoped<IOrderService, OrderService>();

// Validators
builder.Services.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
builder.Services.AddScoped<IValidator<OrderItemDto>, OrderItemDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();
app.UseMiddleware<JwtValidationMiddleware>();
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