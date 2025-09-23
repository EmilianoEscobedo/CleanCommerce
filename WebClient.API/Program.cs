using WebClient.API.Components;
using WebClient.API.Extensions;
using WebClient.Application.Services;
using WebClient.Infrastructure.Services;
using WebClient.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Application services
builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<ICustomerService, CustomerService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddHttpClient<IProductService, ProductService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddHttpClient<IOrderService, OrderService>()
    .ConfigureDevCertificateValidation(builder.Environment);

builder.Services.AddHttpClient<ISecurityService, SecurityService>()
    .ConfigureDevCertificateValidation(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();