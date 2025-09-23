namespace Product.API.Extensions;

public static class HttpClientExtensions
{
    public static void ConfigureDevCertificateValidation(
        this IHttpClientBuilder builder,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            builder.ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });
        }
    }
}