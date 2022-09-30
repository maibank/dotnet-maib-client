using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    internal class InternalMaibApiResponse : BaseApiResponse
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}