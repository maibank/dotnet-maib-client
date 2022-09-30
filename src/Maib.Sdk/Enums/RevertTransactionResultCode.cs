// ReSharper disable InconsistentNaming
namespace Maib.Sdk.Enums
{
    public enum RevertTransactionResultCode
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
        ///     Transaction has already been reversed.
        /// </summary>
        REVERSED,

        /// <summary>
        ///     Failed.
        /// </summary>
        FAILED
    }
}