using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Requests
{
    [ExcludeFromCodeCoverage]
    internal class BaseMaibRequest
    {
        public string Command { get; set; } = string.Empty;
    }
}