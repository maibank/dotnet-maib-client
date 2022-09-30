// ReSharper disable InconsistentNaming
using Maib.Sdk.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public class CloseDayResponse : BaseApiResponse
    {
        public CloseDayResponse(BaseApiResponse response) : base(response)
        {

        }

        /// <summary>
        ///     A <see cref="CloseDayResultCode"/> representing the result. RESULT.
        /// </summary>
        public CloseDayResultCode Result { get; set; } = CloseDayResultCode.Unknown;

        /// <summary>
        ///     A <see cref="string"/> representing the result code. RESULT. transaction result code returned from Card Suite Processing RTPS (3 digits)
        /// </summary>
        public string ResultCode { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the number of credit reversals (up to 10 digits), shown only if result_code begins with 5
        /// </summary>
        public string FLD_075 { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the number of debit transactions (up to 10 digits), shown only if result_code begins with 5
        /// </summary>
        public string FLD_076 { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the total amount of credit reversals (up to 16 digits), shown only if result_code begins with 5
        /// </summary>
        public string FLD_087 { get; set; } = string.Empty;

        /// <summary>
        ///     A <see cref="string"/> representing the total amount of debit transactions (up to 16 digits), shown only if result_code begins with 5.
        /// </summary>
        public string FLD_088 { get; set; } = string.Empty;
    }
}