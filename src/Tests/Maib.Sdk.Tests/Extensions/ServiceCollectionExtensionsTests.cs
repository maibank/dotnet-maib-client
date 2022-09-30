using System;
using System.Collections.Generic;
using System.IO;
using Maib.Sdk.Abstractions;
using Maib.Sdk.Extensions;
using Maib.Sdk.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Maib.Sdk.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMaibClient_Using_IConfiguration()
    {
        //Arrange

        var certificatePath = Path.Combine(AppContext.BaseDirectory, "Assets", "0149583.pfx");
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"MaibClient:{nameof(MaibClientConfiguration.BaseUrl)}", MaibConstants.MAIB_TEST_BASE_URI },
                { $"MaibClient:{nameof(MaibClientConfiguration.RedirectBaseUrl)}", MaibConstants.MAIB_TEST_REDIRECT_URL },
                { $"MaibClient:{nameof(MaibClientConfiguration.CertificatePath)}",  certificatePath},
                { $"MaibClient:{nameof(MaibClientConfiguration.CertificatePassword)}", MaibConstants.MAIB_TEST_CERT_PASS },
            })
            .Build();

        var services = new ServiceCollection();
        services.AddMaibClient(configuration);
        var serviceProvider = services.BuildServiceProvider();

        //Act
        var maibClient = serviceProvider.GetRequiredService<IMaibClient>();

        //Assert

        Assert.NotNull(maibClient);
    }

    [Fact]
    public void AddMaibClient_Using_Object_Configuration()
    {
        //Arrange

        var certificatePath = Path.Combine(AppContext.BaseDirectory, "Assets", "0149583.pfx");

        var services = new ServiceCollection();
        services.AddMaibClient(new MaibClientConfiguration
        {
            BaseUrl = MaibConstants.MAIB_TEST_BASE_URI,
            RedirectBaseUrl = MaibConstants.MAIB_TEST_REDIRECT_URL,
            CertificatePath = certificatePath,
            CertificatePassword = MaibConstants.MAIB_TEST_CERT_PASS
        });

        var serviceProvider = services.BuildServiceProvider();

        //Act
        var maibClient = serviceProvider.GetRequiredService<IMaibClient>();

        //Assert

        Assert.NotNull(maibClient);
    }

    [Fact]
    public void AddMaibClient_Using_Delegate_Configuration()
    {
        //Arrange

        var certificatePath = Path.Combine(AppContext.BaseDirectory, "Assets", "0149583.pfx");

        var services = new ServiceCollection();
        services.AddMaibClient(options =>
        {
            options.BaseUrl = MaibConstants.MAIB_TEST_BASE_URI;
            options.RedirectBaseUrl = MaibConstants.MAIB_TEST_REDIRECT_URL;
            options.CertificatePath = certificatePath;
            options.CertificatePassword = MaibConstants.MAIB_TEST_CERT_PASS;
        });

        var serviceProvider = services.BuildServiceProvider();

        //Act
        var maibClient = serviceProvider.GetRequiredService<IMaibClient>();

        //Assert

        Assert.NotNull(maibClient);
    }
}