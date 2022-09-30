using System;
using System.Collections.Generic;
using System.Net;
using Maib.Sdk.Abstractions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Maib.Sdk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;
using Maib.Sdk.Enums;

namespace Maib.Sdk.Tests;

public class MaibClientTests
{
    private readonly IMaibClient _maibClient;
    private readonly Mock<HttpMessageHandler> _httpClientHandlerMock;

    private static MaibClientConfiguration CreateConfiguration()
        => new()
        {
            BaseUrl = MaibConstants.MAIB_TEST_BASE_URI,
            RedirectBaseUrl = MaibConstants.MAIB_TEST_REDIRECT_URL,
            CertificatePath = string.Empty,
            CertificatePassword = MaibConstants.MAIB_TEST_CERT_PASS
        };

    public MaibClientTests()
    {
        _httpClientHandlerMock = new Mock<HttpMessageHandler>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClientMock = new HttpClient(_httpClientHandlerMock.Object);
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClientMock);

        _maibClient = new MaibClient(httpClientFactoryMock.Object, Options.Create(CreateConfiguration()), new Mock<ILogger<MaibClient>>().Object);
    }

    [Fact]
    public void CreateMaibClient_With_HttpClient()
    {
        //Act

        var maibClient = new MaibClient(new HttpClient(_httpClientHandlerMock.Object), CreateConfiguration(), new Mock<ILogger<MaibClient>>().Object);

        //Assert

        Assert.NotNull(maibClient);
    }

    [Fact]
    public async Task RegisterSmsTransaction_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "TRANSACTION_ID", Guid.NewGuid().ToString() }
                }))
            });

        //Act
        var maibResponse = await _maibClient.RegisterSmsTransactionAsync
        (
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.NotNull(maibResponse.TransactionId);
        Assert.NotNull(maibResponse.RedirectUrl);
    }

    [Fact]
    public async Task RegisterSmsTransaction_Error_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        const string error = "Error from Maib";

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "error",  error}
                }))
            });

        //Act
        var maibResponse = await _maibClient.RegisterSmsTransactionAsync
        (
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.False(maibResponse.Success);
        Assert.Equal(error, maibResponse.Error);
    }

    [Fact]
    public async Task RegisterSmsTransaction_Invalid_Currency_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        //Act & Assert

        await Assert.ThrowsAsync<NotSupportedException>(async () => await _maibClient.RegisterSmsTransactionAsync
        (
            1,
            "invalid",
            clientIpAddress,
            "en",
            "testing"
        ));
    }

    [Fact]
    public async Task RegisterSmsTransaction_HttpException_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        const string error = "Http error";

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Throws(() => new HttpRequestException(error));

        //Act
        var maibResponse = await _maibClient.RegisterSmsTransactionAsync
        (
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.False(maibResponse.Success);
        Assert.Equal(error, maibResponse.Error);
    }

    [Fact]
    public async Task RegisterSmsTransaction_Service_Unavailable_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        //Act
        var maibResponse = await _maibClient.RegisterSmsTransactionAsync
        (
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.False(maibResponse.Success);
    }

    [Fact]
    public async Task RegisterDmsAuthorization_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "TRANSACTION_ID", Guid.NewGuid().ToString() }
                }))
            });

        //Act
        var maibResponse = await _maibClient.RegisterDmsAuthorizationAsync
        (
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.NotNull(maibResponse.TransactionId);
        Assert.NotNull(maibResponse.RedirectUrl);
    }

    [Fact]
    public async Task MakeDmsTransaction_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "RESULT", MakeDmsTransactionResultCode.OK.ToString() },
                    { "CARD_NUMBER", "****12"}
                }))
            });

        //Act
        var maibResponse = await _maibClient.MakeDmsTransactionAsync
        (
            Guid.NewGuid().ToString(),
            1,
            "MDL",
            clientIpAddress,
            "en",
            "testing"
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.Equal(MakeDmsTransactionResultCode.OK, maibResponse.Result);
    }

    [Fact]
    public async Task GetTransactionResult_Test()
    {
        //Arrange

        var clientIpAddress = IPAddress.Parse("127.0.0.1");

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "RESULT", GetTransactionResultCode.CREATED.ToString() },
                    { "RESULT_PS", GetTransactionResultPsCode.ACTIVE.ToString()}
                }))
            });

        //Act
        var maibResponse = await _maibClient.GetTransactionResultAsync
        (
            Guid.NewGuid().ToString(),
            clientIpAddress
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.Equal(GetTransactionResultCode.CREATED, maibResponse.Result);
        Assert.Equal(GetTransactionResultPsCode.ACTIVE, maibResponse.ResultPs);
    }

    [Fact]
    public async Task RevertTransaction_Test()
    {
        //Arrange

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "RESULT", RevertTransactionResultCode.OK.ToString() }
                }))
            });

        //Act
        var maibResponse = await _maibClient.RevertTransactionAsync
        (
            Guid.NewGuid().ToString(),
            1
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.Equal(RevertTransactionResultCode.OK, maibResponse.Result);
    }

    [Fact]
    public async Task RefundTransaction_Test()
    {
        //Arrange

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "RESULT", RefundTransactionResultCode.OK.ToString() }
                }))
            });

        //Act
        var maibResponse = await _maibClient.RefundTransactionAsync
        (
            Guid.NewGuid().ToString(),
            1
        );

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.Equal(RefundTransactionResultCode.OK, maibResponse.Result);
    }

    [Fact]
    public async Task CloseDay_Test()
    {
        //Arrange

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildBodyResponse(new Dictionary<string, string>
                {
                    { "RESULT", CloseDayResultCode.OK.ToString() },
                    { "FLD_075", "12345" },
                }))
            });

        //Act
        var maibResponse = await _maibClient.CloseDayAsync();

        //Assert
        Assert.NotNull(maibResponse);
        Assert.True(maibResponse.Success);
        Assert.Equal(CloseDayResultCode.OK, maibResponse.Result);
        Assert.Equal("12345", maibResponse.FLD_075);
    }

    private static string BuildBodyResponse(IReadOnlyDictionary<string, string> body)
    {
        var builder = new StringBuilder();

        foreach (var item in body)
        {
            builder.Append($"{item.Key}: {item.Value}");
            builder.AppendLine();
        }

        return builder.ToString();
    }
}