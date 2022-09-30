using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Maib.Sdk.Models.Responses;

namespace Maib.Sdk.Abstractions
{
    /// <summary>
    ///     An <see cref="IMaibClient"/> representing the maib interaction api.
    /// </summary>
    public interface IMaibClient
    {
        /// <summary>
        ///     Start SMS transaction. This is simplest form that charges amount to customer instantly.
        /// </summary>
        /// <param name="amount">
        ///     A <see cref="decimal"/> representing the transaction amount.
        /// </param>
        /// <param name="currency">
        ///     A <see cref="string"/> representing the currency. ex: MDL, EUR, USD
        /// </param>
        /// <param name="clientIpAddress">
        ///     A <see cref="IPAddress"/> representing the ip address from the user who is doing the transaction.
        /// </param>
        /// <param name="language">
        ///     A <see cref="string"/> representing the language. ex: en
        /// </param>
        /// <param name="description">
        ///     A <see cref="string"/> representing the transaction description, will be showed on ui interface and transaction history.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<RegisterSmsTransactionResponse> RegisterSmsTransactionAsync(decimal amount, string currency, IPAddress clientIpAddress,
            string language, string description = "", CancellationToken cancellationToken = default);

        /// <summary>
        ///     DMS is different from SMS, dms_start_authorization blocks amount, and than we use dms_make_transaction to charge customer.
        /// </summary>
        /// <param name="amount">
        ///     A <see cref="decimal"/> representing the transaction amount.
        /// </param>
        /// <param name="currency">
        ///     A <see cref="string"/> representing the currency. ex: MDL, EUR, USD
        /// </param>
        /// <param name="clientIpAddress">
        ///     A <see cref="IPAddress"/> representing the ip address from the user who is doing the transaction.
        /// </param>
        /// <param name="language">
        ///     A <see cref="string"/> representing the language. ex: en
        /// </param>
        /// <param name="description">
        ///     A <see cref="string"/> representing the transaction description, will be showed on ui interface and transaction history.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<RegisterDmsAuthorizationResponse> RegisterDmsAuthorizationAsync(decimal amount, string currency, IPAddress clientIpAddress,
            string language, string description = "", CancellationToken cancellationToken = default);

        /// <summary>
        ///     Executing a DMS transaction.
        /// </summary>
        /// <param name="authId">
        ///     A <see cref="string"/> representing the transaction id.
        /// </param>
        /// <param name="amount">
        ///     A <see cref="decimal"/> representing the transaction amount.
        /// </param>
        /// <param name="currency">
        ///     A <see cref="string"/> representing the currency. ex: MDL, EUR, USD
        /// </param>
        /// <param name="clientIpAddress">
        ///     A <see cref="IPAddress"/> representing the ip address from the user who is doing the transaction.
        /// </param>
        /// <param name="language">
        ///     A <see cref="string"/> representing the language. ex: en
        /// </param>
        /// <param name="description">
        ///     A <see cref="string"/> representing the transaction description, will be showed on ui interface and transaction history.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<MakeDmsTransactionResponse> MakeDmsTransactionAsync(string authId, decimal amount, string currency, IPAddress clientIpAddress,
            string language, string description = "", CancellationToken cancellationToken = default);

        /// <summary>
        ///     Transaction result.
        /// </summary>
        /// <param name="transactionId">
        ///     A <see cref="string"/> representing the transaction id.
        /// </param>
        /// <param name="clientIpAddress">
        ///     A <see cref="IPAddress"/> representing the ip address from the user who is doing the transaction.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<GetTransactionResultResponse> GetTransactionResultAsync(string transactionId, IPAddress clientIpAddress, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Transaction reversal.
        /// </summary>
        /// <param name="transactionId">
        ///     A <see cref="string"/> representing the transaction id.
        /// </param>
        /// <param name="amount">
        ///     Reversal amount in fractional units (up to 12 characters).
        ///     For DMS authorizations only full amount can be reversed, i.e.,
        ///     the reversal and authorization amounts have to match.
        ///     In other cases partial reversal is also available.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<RevertTransactionResponse> RevertTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Transaction refund.
        /// </summary>
        /// <param name="transactionId">
        ///     A <see cref="string"/> representing the transaction id.
        /// </param>
        /// <param name="amount">
        ///     Full original transaction amount is always refunded.
        /// </param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<RefundTransactionResponse> RefundTransactionAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Needs to be run once every 24 hours.
        ///     this tells bank to process all transactions of that day SMS or DMS that were success
        ///     in case of DMS only confirmed and successful transactions will be processed
        /// </summary>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> representing the cancellation token.
        /// </param>
        Task<CloseDayResponse> CloseDayAsync(CancellationToken cancellationToken = default);
    }
}