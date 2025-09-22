using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Customer.Application.DTOs;
using Customer.Application.Mappings;
using Customer.Application.Services;
using Customer.Application.Validators;
using Customer.Domain.Repositories;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Validators
builder.Services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline
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