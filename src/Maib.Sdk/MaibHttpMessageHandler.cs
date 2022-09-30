using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Maib.Sdk.Models;
using Microsoft.Extensions.Options;

namespace Maib.Sdk
{
    internal class MaibHttpMessageHandler : HttpClientHandler
    {
        public MaibHttpMessageHandler(IOptions<MaibClientConfiguration> configuration)
        {
            var sslCertificate = new X509Certificate2(configuration.Value.CertificatePath,
                configuration.Value.CertificatePassword);
            ClientCertificates.Add(sslCertificate);
        }
    }
}