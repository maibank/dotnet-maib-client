using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Maib.Sdk.Abstractions;
using Maib.Sdk.Enums;
using Maib.Sdk.Helpers;
using Maib.Sdk.Models;
using Maib.Sdk.Models.Requests;
using Maib.Sdk.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maib.Sdk
{
    /// <inheritdoc />
    public sealed class MaibClient : IMaibClient
    {
        private readonly HttpClient _httpClient;
        private readonly MaibClientConfiguration _configuration;
        private readonly ILogger<MaibClient> _logger;

        public MaibClient(IHttpClientFactory httpClientFactory, IOptions<MaibClientConfiguration> configuration, ILogger<MaibClient> logger)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient(MaibConstants.HttpClientName);
            _configuration = configuration.Value;
            ConfigureClient();
        }

        public MaibClient(HttpClient httpClient, MaibClientConfiguration configuration, ILogger<MaibClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            ConfigureClient();
        }

        private void ConfigureClient()
        {
            _httpClient.BaseAddress = new Uri(_configuration.BaseUrl);
        }

        /// <inheritdoc />
        public async Task<RegisterSmsTransactionResponse> RegisterSmsTransactionAsync(decimal amount, string currency, IPAddress clientIpAddress, string language,
            string description = "", CancellationToken cancellationToken = default)
        {
            var request = new RegisterTransactionRequest
            {
                Command = MaibCommands.RegisterSmsTransaction,
                Amount = NormalizeAmount(amount),
                Currency = GetCurrencyInfo(currency).Number.ToString(),
                ClientIpAddress = clientIpAddress.ToString(),
                MessageType = MaibMessageType.SMS,
                Language = language,
                Description = description
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new RegisterSmsTransactionResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.TransactionId = apiResponse.Data.GetValueOrDefault("TRANSACTION_ID");
            response.RedirectUrl = BuildTransactionRedirectUrl(response.TransactionId);
            return response;
        }

        /// <inheritdoc />
        public async Task<RegisterDmsAuthorizationResponse> RegisterDmsAuthorizationAsync(decimal amount, string currency, IPAddress clientIpAddress, string language,
            string description = "", CancellationToken cancellationToken = default)
        {
            var request = new RegisterTransactionRequest
            {
                Command = MaibCommands.RegisterDmsAuthorization,
                Amount = NormalizeAmount(amount),
                Currency = GetCurrencyInfo(currency).Number.ToString(),
                ClientIpAddress = clientIpAddress.ToString(),
                MessageType = MaibMessageType.DMS,
                Language = language,
                Description = description
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new RegisterDmsAuthorizationResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.TransactionId = apiResponse.Data.GetValueOrDefault("TRANSACTION_ID");
            response.RedirectUrl = BuildTransactionRedirectUrl(response.TransactionId);
            return response;
        }

        private string BuildTransactionRedirectUrl(string transactionId)
            => $"{new Uri(new Uri(_configuration.RedirectBaseUrl), "ecomm/ClientHandler")}?trans_id={transactionId}";

        /// <inheritdoc />
        public async Task<MakeDmsTransactionResponse> MakeDmsTransactionAsync(string authId, decimal amount, string currency, IPAddress clientIpAddress, string language,
            string description = "", CancellationToken cancellationToken = default)
        {
            var request = new MakeDmsTransactionResultRequest
            {
                Command = MaibCommands.MakeDmsTransaction,
                Amount = NormalizeAmount(amount),
                Currency = GetCurrencyInfo(currency).Number.ToString(),
                ClientIpAddress = clientIpAddress.ToString(),
                MessageType = MaibMessageType.DMS,
                Language = language,
                Description = description,
                TransactionId = authId
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new MakeDmsTransactionResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.Result = ReadEnumFromDictionary<MakeDmsTransactionResultCode>(apiResponse.Data, "RESULT");
            response.ResultCode = apiResponse.Data.GetValueOrDefault("RESULT_CODE");
            response.Brn = apiResponse.Data.GetValueOrDefault("BRN");
            response.Approval_Code = apiResponse.Data.GetValueOrDefault("APPROVAL_CODE");
            response.CardNumber = apiResponse.Data.GetValueOrDefault("CARD_NUMBER");

            return response;
        }

        /// <inheritdoc />
        public async Task<GetTransactionResultResponse> GetTransactionResultAsync(string transactionId, IPAddress clientIpAddress,
            CancellationToken cancellationToken = default)
        {
            var request = new GetTransactionResultRequest
            {
                Command = MaibCommands.GetTransactionResult,
                ClientIpAddress = clientIpAddress.ToString(),
                TransactionId = transactionId
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new GetTransactionResultResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.Result = ReadEnumFromDictionary<GetTransactionResultCode>(apiResponse.Data, "RESULT");
            response.ResultPs = ReadEnumFromDictionary<GetTransactionResultPsCode>(apiResponse.Data, "RESULT_PS");
            response.ResultCode = apiResponse.Data.GetValueOrDefault("RESULT_CODE");
            response.ThreeDSecure = ReadEnumFromDictionary<ThreeDSecureCode>(apiResponse.Data, "3DSECURE");
            response.Rrn = apiResponse.Data.GetValueOrDefault("RRN");
            response.ApprovalCode = apiResponse.Data.GetValueOrDefault("APPROVAL_CODE");
            response.CardNumber = apiResponse.Data.GetValueOrDefault("CARD_NUMBER");
            response.Aav = apiResponse.Data.GetValueOrDefault("AAV");
            response.ReccPmntId = apiResponse.Data.GetValueOrDefault("RECC_PMNT_ID");
            response.ReccPmntExpiry = apiResponse.Data.GetValueOrDefault("RECC_PMNT_EXPIRY");
            response.MrchTrabsactionId = apiResponse.Data.GetValueOrDefault("MRCH_TRANSACTION_ID");
            response.Warning = apiResponse.Data.GetValueOrDefault("warning");

            return response;
        }

        /// <inheritdoc />
        public async Task<RevertTransactionResponse> RevertTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default)
        {
            var request = new RevertTransactionResponseRequest
            {
                Command = MaibCommands.RevertTransaction,
                Amount = NormalizeAmount(amount),
                TransactionId = transactionId
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new RevertTransactionResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.Result = ReadEnumFromDictionary<RevertTransactionResultCode>(apiResponse.Data, "RESULT");
            response.ResultCode = apiResponse.Data.GetValueOrDefault("RESULT_CODE");
            response.Warning = apiResponse.Data.GetValueOrDefault("warning");

            return response;
        }

        /// <inheritdoc />
        public async Task<RefundTransactionResponse> RefundTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default)
        {
            var request = new RefundTransactionRequest
            {   
                Command = MaibCommands.RevertTransaction,
                Amount = NormalizeAmount(amount),
                TransactionId = transactionId
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new RefundTransactionResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.Result = ReadEnumFromDictionary<RefundTransactionResultCode>(apiResponse.Data, "RESULT");
            response.ResultCode = apiResponse.Data.GetValueOrDefault("RESULT_CODE");
            response.RefundTransactionId = apiResponse.Data.GetValueOrDefault("refund_transaction_id");
            response.Warning = apiResponse.Data.GetValueOrDefault("warning");

            return response;
        }

        /// <inheritdoc />
        public async Task<CloseDayResponse> CloseDayAsync(CancellationToken cancellationToken = default)
        {
            var request = new BaseMaibRequest
            {
                Command = MaibCommands.CloseDay
            };

            var apiResponse = await SendCommandAsync(request, cancellationToken);

            var response = new CloseDayResponse(apiResponse);
            if (!apiResponse.Success) return response;

            response.Result = ReadEnumFromDictionary<CloseDayResultCode>(apiResponse.Data, "RESULT");
            response.ResultCode = apiResponse.Data.GetValueOrDefault("RESULT_CODE");
            response.FLD_075 = apiResponse.Data.GetValueOrDefault("FLD_075");
            response.FLD_076 = apiResponse.Data.GetValueOrDefault("FLD_076");
            response.FLD_087 = apiResponse.Data.GetValueOrDefault("FLD_087");
            response.FLD_088 = apiResponse.Data.GetValueOrDefault("FLD_088");

            return response;
        }

        private static Iso4217Lookup.Iso4217Definition GetCurrencyInfo(string currency)
        {
            var currencyCode = Iso4217Lookup.LookupByCode(currency);
            if (!currencyCode.Found) throw new NotSupportedException($"{currency} not supported.");

            return currencyCode;
        }

        private static string NormalizeAmount(decimal originalAmount)
        {
            if (originalAmount <= 0) throw new ArgumentOutOfRangeException(nameof(originalAmount), originalAmount, "Amount must be greater than 0.");
            var roundedAmount = Math.Round(originalAmount, 2);
            var normalized = roundedAmount * 100;
            return ((int)normalized).ToString();
        }

        private static TEnum ReadEnumFromDictionary<TEnum>(IReadOnlyDictionary<string, string> data, string key)
            where TEnum : struct
        {
            var value = data.GetValueOrDefault(key);
            return value == null ? default : Enum.Parse<TEnum>(value, true);
        }

        private async Task<InternalMaibApiResponse> SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : BaseMaibRequest
        {
            var formData = GetFormData(request);

            _logger.LogInformation("[Maib] Start to send command {0} via body: {1}", request.Command, Serialize(request));

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "ecomm/MerchantHandler")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            return await SendRequestAsync(httpRequestMessage, cancellationToken);
        }

        private async Task<InternalMaibApiResponse> SendRequestAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken = default)
        {
            var response = new InternalMaibApiResponse();
            try
            {
                var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
                var rawResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                response.StatusCode = httpResponseMessage.StatusCode;
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[Maib] The request didn't succeed, status: {0}, body: {1}", httpResponseMessage.StatusCode, rawResponse);
                    response.Success = false;
                    return response;
                }

                var responseData = ParseResponse(rawResponse);

                response.Data = responseData;
                response.Success = true;

                if (!responseData.TryGetValue("error", out var errorMessage)) return response;
                response.Success = false;
                response.Error = errorMessage;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Maib] An error occurred on send request to maib.");
                response.Error = e.Message;
            }

            return response;
        }

        private Dictionary<string, string> GetFormData(object? data)
        {
            if (data == null) return new Dictionary<string, string>();

            var rawData = Serialize(data);

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(rawData)
                           ?.ToDictionary(k => k.Key, k => k.Value?.ToString() ?? string.Empty)
                       ?? new Dictionary<string, string>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[Maib] Cannot create dictionary from object.");
                return new Dictionary<string, string>();
            }
        }

        private static string Serialize(object data)
        {
            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var rawData = JsonSerializer.Serialize(data, jsonSettings);
            return rawData;
        }

        private static Dictionary<string, string> ParseResponse(string httpRawResponse)
        {
            if (string.IsNullOrEmpty(httpRawResponse)) return new Dictionary<string, string>();
            var lines = httpRawResponse.Split('\n');

            var data = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var columns = line.Split(':');
                if (columns.Length <= 1) continue;
                var key = columns[0];
                var value = line[(key.Length + 1)..].Trim();

                data.TryAdd(key, value);
            }

            return data;
        }
    }
}
