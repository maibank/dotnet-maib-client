using Maib.Sdk.Abstractions;
using Maib.Sdk.Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maib.Sdk.Demo.Controllers
{
    [Route("api/maib-payment")]
    [ApiController]
    public class MaibPaymentsController : ControllerBase
    {
        private readonly IMaibClient _maibClient;

        public MaibPaymentsController(IMaibClient maibClient)
        {
            _maibClient = maibClient;
        }

        [HttpPost("register-sms-transaction")]
        public async Task<IActionResult> RegisterSmsTransaction
            (
            [FromForm] RegisterSmsTransactionViewModel model,
            CancellationToken cancellationToken = default
            )
        {
            var response = await _maibClient.RegisterSmsTransactionAsync(model.Amount, model.Currency, HttpContext.Connection.RemoteIpAddress, model.Language, model.Description, cancellationToken);
            return Ok(response);
        }

        [HttpGet("transaction-result")]
        public async Task<IActionResult> GetTransactionResult
            (
            [FromQuery] string transactionId,
            CancellationToken cancellationToken = default
            )
        {
            var response = await _maibClient.GetTransactionResultAsync(transactionId, HttpContext.Connection.RemoteIpAddress, cancellationToken);
            return Ok(response);
        }
    }
}