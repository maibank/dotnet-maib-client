// ReSharper disable InconsistentNaming
using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk
{
    [ExcludeFromCodeCoverage]
    public static class MaibConstants
    {
        /// <summary>
        ///     The http client name.
        /// </summary>
        internal const string HttpClientName = "maib_client";

        ///
        /// The Payment Gateway URL to use in development/testing mode.
        ///
        public const string MAIB_TEST_REDIRECT_URL = "https://maib.ecommerce.md:21443";

        ///
        /// The Bank server URL to use in development/testing mode.
        ///
        public const string MAIB_TEST_BASE_URI = "https://maib.ecommerce.md:21440";

        ///
        /// The Certificate PASSWORD to use in development/testing mode.
        ///
        public const string MAIB_TEST_CERT_PASS = "Za86DuC$";
    }
}