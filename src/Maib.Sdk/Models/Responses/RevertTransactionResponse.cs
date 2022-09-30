using Maib.Sdk.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class RevertTransactionResponse : BaseApiResponse
    {
        public RevertTransactionResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     A <see cref="RevertTransactionResultCode"/> representing the result. RESULT.
        /// </summary>
        public RevertTransactionResultCode Result { get; set; } = RevertTransactionResultCode.Unknown;

        /// <summary>
        ///     A <see cref="string"/> representing the result code. RESULT. transaction result code returned from Card Suite Processing RTPS (3 digits)
        /// </summary>
        public string ResultCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the warning.
        /// </summary>
        public string Warning { get; set; } = string.Empty;
    }
}