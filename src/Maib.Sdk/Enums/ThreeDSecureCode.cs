// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace Maib.Sdk.Enums
{
    public enum ThreeDSecureCode
    {
        /// <summary>
        ///     Invalid result.
        /// </summary>
        Unknown,

        /// <summary>
        ///     successful 3D Secure authorization
        /// </summary>
        AUTHENTICATED,

        /// <summary>
        ///     failed 3D Secure authorization
        /// </summary>
        DECLINED,

        /// <summary>
        ///     cardholder is not a member of 3D Secure scheme
        /// </summary>
        NOTPARTICIPATED,

        /// <summary>
        ///     card is not in 3D secure card range defined by issuer
        /// </summary>
        NO_RANGE,

        /// <summary>
        ///     cardholder 3D secure authorization using attempts ACS server
        /// </summary>
        ATTEMPTED,

        /// <summary>
        ///     cardholder 3D secure authorization is unavailable
        /// </summary>
        UNAVAILABLE,

        /// <summary>
        /// error message received from ACS server
        /// </summary>
        ERROR,

        /// <summary>
        /// 3D secure authorization ended with system error
        /// </summary>
        SYSERROR,

        /// <summary>
        ///     3D secure authorization was attempted by wrong card scheme (Dinners club, American Express)
        /// </summary>
        UNKNOWNSCHEME
    }
}