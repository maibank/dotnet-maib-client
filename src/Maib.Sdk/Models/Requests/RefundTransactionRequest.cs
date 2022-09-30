using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Maib.Sdk.Models.Requests
{
    [ExcludeFromCodeCoverage]
    internal sealed class RefundTransactionRequest : BaseMaibRequest
    {
        public string Amount { get; set; } = "";

        [JsonPropertyName("trans_id")]
        public string TransactionId { get; set; } = "";
    }
}