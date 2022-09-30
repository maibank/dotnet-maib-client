using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Maib.Sdk.Abstractions;
using Maib.Sdk.Enums;
using Maib.Sdk.Extensions;
using Maib.Sdk.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Maib.Sdk.Tests
{
    [ExcludeFromCodeCoverage]
    public class MaibClientIntegrationTests
    {
        private readonly IMaibClient _maibClient;

        public MaibClientIntegrationTests()
        {
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

            _maibClient = serviceProvider.GetRequiredService<IMaibClient>();
        }

        [Fact(Skip = "Can be run only with allowed ip address.")]
        public async Task RegisterSmsTransaction_IntegrationTest()
        {
            //Arrange

            var clientIpAddress = IPAddress.Parse("127.0.0.1");

            //Act & Assert
            var registerSmsTransactionResponse = await _maibClient.RegisterSmsTransactionAsync
                (
                1,
                "MDL",
                clientIpAddress,
                "en",
                "testing"
                );

            Assert.NotNull(registerSmsTransactionResponse);
            Assert.NotNull(registerSmsTransactionResponse.TransactionId);
            Assert.NotNull(registerSmsTransactionResponse.RedirectUrl);

            var transactionResult = await _maibClient.GetTransactionResultAsync(registerSmsTransactionResponse.TransactionId, clientIpAddress);
            Assert.NotNull(transactionResult);
            Assert.Equal(GetTransactionResultCode.CREATED, transactionResult.Result);
        }

        [Fact(Skip = "Can be run only with allowed ip address.")]
        public async Task RegisterDmsTransaction_IntegrationTest()
        {
            //Arrange

            var clientIpAddress = IPAddress.Parse("127.0.0.1");

            //Act & Assert
            var dmsAuthorizationResponse = await _maibClient.RegisterDmsAuthorizationAsync
            (
                1,
                "MDL",
                clientIpAddress,
                "en",
                "testing"
            );

            Assert.NotNull(dmsAuthorizationResponse);
            Assert.NotNull(dmsAuthorizationResponse.TransactionId);
            Assert.NotNull(dmsAuthorizationResponse.RedirectUrl);

            var transactionResult = await _maibClient.GetTransactionResultAsync(dmsAuthorizationResponse.TransactionId, clientIpAddress);
            Assert.NotNull(transactionResult);
            Assert.Equal(GetTransactionResultCode.CREATED, transactionResult.Result);
        }
    }
}