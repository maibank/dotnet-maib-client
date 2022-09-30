// ReSharper disable IdentifierTypo
using Maib.Sdk.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    /// <summary>
    ///     The RESULT_CODE and 3DSECURE fields are informative only and can be not shown.
    ///     The fields RRN and APPROVAL_CODE appear for successful transactions only, for informative purposes,
    ///     and they facilitate tracking the transactions in Card Suite Processing RTPS system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetTransactionResultResponse : BaseApiResponse
    {
        public GetTransactionResultResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     A <see cref="GetTransactionResultCode"/> representing the result. RESULT.
        /// </summary>
        public GetTransactionResultCode Result { get; set; } = GetTransactionResultCode.Unknown;

        /// <summary>
        ///     A <see cref="GetTransactionResultPsCode"/> representing the result ps. RESULT_PS transaction result, Payment Server interpretation (shown only
        ///     if configured to return ECOMM2 specific details)
        /// </summary>
        public GetTransactionResultPsCode ResultPs { get; set; } = GetTransactionResultPsCode.Unknown;

        /// <summary>
        ///     A <see cref="string"/> representing the result code. RESULT. transaction result code returned from Card Suite Processing RTPS (3 digits)
        /// </summary>
        public string ResultCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="ThreeDSecureCode"/> representing the 3DSECURE. 3DSECURE.
        /// </summary>
        public ThreeDSecureCode ThreeDSecure { get; set; } = ThreeDSecureCode.Unknown;

        /// <summary>
        ///     A <see cref="string"/> representing the retrieval reference number returned from Card Suite Processing RTPS
        /// </summary>
        public string Rrn { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the approval code returned from Card Suite Processing RTPS (max 6 characters)
        /// </summary>
        public string ApprovalCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the masked card number.
        /// </summary>
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the results of the verification of hash value in AAV merchant name (only if failed)
        /// </summary>
        public string Aav { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the RECC_PMNT_ID
        /// </summary>
        public string ReccPmntId { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the RECC_PMNT_EXPIRY
        /// </summary>
        public string ReccPmntExpiry { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the MRCH_TRANSACTION_ID
        /// </summary>
        public string MrchTrabsactionId { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the warning.
        /// </summary>
        public string Warning { get; set; } = string.Empty;
    }
}