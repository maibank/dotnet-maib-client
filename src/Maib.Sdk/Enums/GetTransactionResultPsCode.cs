// ReSharper disable InconsistentNaming
namespace Maib.Sdk.Enums
{
    public enum GetTransactionResultPsCode
    {
        /// <summary>
        ///     Invalid result.
        /// </summary>
        Unknown,

        /// <summary>
        ///     Successfully completed payment
        /// </summary>
        FINISHED,

        /// <summary>
        ///     Cancelled payment
        /// </summary>
        CANCELLED,

        /// <summary>
        ///     Returned payment
        /// </summary>
        RETURNED,

        /// <summary>
        ///     Registered and not yet completed payment.
        /// </summary>
        ACTIVE
    }
}