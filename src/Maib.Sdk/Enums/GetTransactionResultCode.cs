// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace Maib.Sdk.Enums
{
    public enum GetTransactionResultCode
    {
        /// <summary>
        ///     Invalid result.
        /// </summary>
        Unknown,

        /// <summary>
        ///     Successful.
        /// </summary>
        OK,

        /// <summary>
        ///     Transaction has failed
        /// </summary>
        FAILED,

        /// <summary>
        ///     Transaction just registered in the system
        /// </summary>
        CREATED,

        /// <summary>
        ///     Transaction is not accomplished yet
        /// </summary>
        PENDING,

        /// <summary>
        ///     Transaction declined by ECOMM
        /// </summary>
        DECLINED,

        /// <summary>
        ///     Transaction is reversed
        /// </summary>
        REVERSED,

        /// <summary>
        ///     Transaction is reversed by autoreversal
        /// </summary>
        AUTOREVERSED,

        /// <summary>
        ///     Transaction was timed out
        /// </summary>
        TIMEOUT
    }
}