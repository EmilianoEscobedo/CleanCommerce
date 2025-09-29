using Blazored.Toast;
using Serilog;
using WebClient.API.Components;
using WebClient.Application.Services;
using WebClient.Infrastructure.Services;
using WebClient.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddBlazoredToast();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Application services
builder.Services.Configure<ClientSettings>(
    builder.Configuration.GetSection("ClientSettings"));

builder.Services.AddHttpClient<ICustomerService, CustomerService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
