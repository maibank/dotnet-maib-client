using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Maib.Sdk.Models.Requests
{
    [ExcludeFromCodeCoverage]
    internal sealed class MakeDmsTransactionResultRequest : RegisterTransactionRequest
    {

        [JsonPropertyName("trans_id")]
        public string TransactionId { get; set; } = "";
    }
}