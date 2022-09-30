using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class RegisterDmsAuthorizationResponse : BaseApiResponse
    {
        public RegisterDmsAuthorizationResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     TRANSACTION_ID - transaction identifier (28 characters in base64 encoding)
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the redirect url.
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;
    }
}