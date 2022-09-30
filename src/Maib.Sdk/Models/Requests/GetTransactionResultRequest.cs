using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Maib.Sdk.Models.Requests
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetTransactionResultRequest : BaseMaibRequest
    {
        [JsonPropertyName("client_ip_addr")]
        public string ClientIpAddress { get; set; } = "";

        [JsonPropertyName("trans_id")]
        public string TransactionId { get; set; } = "";
    }
}