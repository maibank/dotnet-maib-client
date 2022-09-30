using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Maib.Sdk.Enums;

namespace Maib.Sdk.Models.Requests
{
    [ExcludeFromCodeCoverage]
    internal class RegisterTransactionRequest : BaseMaibRequest
    {
        public string Amount { get; set; } = "";

        /// <summary>
        ///     The currency of the transaction - is the 3 digits code of currency from ISO 4217.
        /// </summary>
        public string Currency { get; set; } = "";

        [JsonPropertyName("client_ip_addr")]
        public string ClientIpAddress { get; set; } = "";

        public string Description { get; set; } = "";

        public string Language { get; set; } = "";

        [JsonPropertyName("msg_type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaibMessageType MessageType { get; set; }
    }
}