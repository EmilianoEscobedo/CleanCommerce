namespace WebClient.API.Extensions;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder ConfigureDevCertificateValidation(
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

        return builder; 
    }
}
