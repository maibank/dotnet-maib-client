using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models
{
    [ExcludeFromCodeCoverage]
    public class MaibClientConfiguration
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string RedirectBaseUrl { get; set; } = string.Empty;
        public string CertificatePath { get; set; } = string.Empty;
        public string CertificatePassword { get; set; } = string.Empty;
    }
}