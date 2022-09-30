using Maib.Sdk.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class RefundTransactionResponse : BaseApiResponse
    {
        public RefundTransactionResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     A <see cref="RefundTransactionResultCode"/> representing the result. RESULT.
        /// </summary>
        public RefundTransactionResultCode Result { get; set; } = RefundTransactionResultCode.Unknown;

        /// <summary>
        ///     A <see cref="string"/> representing the result code. RESULT. transaction result code returned from Card Suite Processing RTPS (3 digits)
        /// </summary>
        public string ResultCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the refund transaction id.
        /// </summary>
        public string RefundTransactionId { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the warning.
        /// </summary>
        public string Warning { get; set; } = string.Empty;
    }
}