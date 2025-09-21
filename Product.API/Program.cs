using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs;
using Product.Application.Mappings;
using Product.Application.Services;
using Product.Application.Validators;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext with migrations in Infrastructure
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Product.Infrastructure")
    ));

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Application services
builder.Services.AddScoped<IProductService, ProductService>();

// Validators
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProductStockDto>, UpdateProductStockDtoValidator>();

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

app.Run();