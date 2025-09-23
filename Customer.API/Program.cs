using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Customer.API.Extensions;
using Customer.API.Middlewares;
using Customer.Application.DTOs;
using Customer.Application.Mappings;
using Customer.Application.Services;
using Customer.Application.Validators;
using Customer.Domain.Repositories;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Repositories;
using Customer.Infrastructure.Services;
using Customer.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Customer.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Application services
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<ISecurityService, SecurityService>()
    .ConfigureDevCertificateValidation(builder.Environment);

// Validators
builder.Services.AddScoped<IValidator<CreateCustomerRequestDto>, CreateCustomerDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerRequestDto>, UpdateCustomerDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));

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
        var dbContext = services.GetRequiredService<CustomerDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();