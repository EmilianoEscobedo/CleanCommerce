using Blazored.Toast;
using Serilog;
using WebClient.API.Components;
using WebClient.API.Extensions;
using WebClient.Application.Services;
using WebClient.Infrastructure.Services;
using WebClient.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Logging.ClearProviders();

var loggingConfig = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level: u3}] {Username} {Message:lj}{NewLine}{Exception}")
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.AddSerilog(loggingConfig);

// Add services to the container.
builder.Services.AddBlazoredToast();
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
