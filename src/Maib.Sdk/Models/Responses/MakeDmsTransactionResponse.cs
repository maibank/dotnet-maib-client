using Maib.Sdk.Enums;
using System.Diagnostics.CodeAnalysis;
// ReSharper disable InconsistentNaming

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class MakeDmsTransactionResponse : BaseApiResponse
    {
        public MakeDmsTransactionResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     A <see cref="MakeDmsTransactionResultCode"/> representing the result. RESULT.
        /// </summary>
        public MakeDmsTransactionResultCode Result { get; set; }

        /// <summary>
        ///     A <see cref="string"/> representing the result code. RESULT. transaction result code returned from Card Suite Processing RTPS (3 digits)
        /// </summary>
        public string ResultCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the retrieval reference number returned from Card Suite Processing RTPS (12 characters). BRN.
        /// </summary>
        public string Brn { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the approval code returned from Card Suite Processing RTPS (max 6 characters). APPROVAL_CODE.
        /// </summary>
        public string Approval_Code { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the masked card number. CARD_NUMBER
        /// </summary>
        public string CardNumber { get; set; } = string.Empty;
    }
}