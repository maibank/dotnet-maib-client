using System;
using System.Net.Http;
using Maib.Sdk.Abstractions;
using Maib.Sdk.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maib.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaibClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MaibClientConfiguration>(configuration.GetSection("MaibClient"));
            services.AddTransient<MaibHttpMessageHandler>();
            services.AddHttpClient(MaibConstants.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler<MaibHttpMessageHandler>();

            services.AddSingleton<IMaibClient>(sp =>
                new MaibClient
                    (
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetRequiredService<IOptions<MaibClientConfiguration>>(),
                    sp.GetRequiredService<ILogger<MaibClient>>()
                    )
            );
            return services;
        }

        public static IServiceCollection AddMaibClient(this IServiceCollection services, MaibClientConfiguration configuration)
        {
            services.Configure<MaibClientConfiguration>(options =>
            {
                options.BaseUrl = configuration.BaseUrl;
                options.RedirectBaseUrl = configuration.RedirectBaseUrl;
                options.CertificatePath = configuration.CertificatePath;
                options.CertificatePassword = configuration.CertificatePassword;
            });

            services.AddTransient<MaibHttpMessageHandler>();
            services.AddHttpClient(MaibConstants.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler<MaibHttpMessageHandler>();

            services.AddSingleton<IMaibClient>(sp =>
                new MaibClient
                    (
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetRequiredService<IOptions<MaibClientConfiguration>>(),
                    sp.GetRequiredService<ILogger<MaibClient>>()
                    ));
            return services;
        }

        public static IServiceCollection AddMaibClient(this IServiceCollection services, Action<MaibClientConfiguration> options)
        {
            services.Configure(options);

            services.AddTransient<MaibHttpMessageHandler>();
            services.AddHttpClient(MaibConstants.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler<MaibHttpMessageHandler>();

            services.AddSingleton<IMaibClient>(sp =>
                new MaibClient
                (
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetRequiredService<IOptions<MaibClientConfiguration>>(),
                    sp.GetRequiredService<ILogger<MaibClient>>()
                ));
            return services;
        }
    }
}