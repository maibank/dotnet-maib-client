namespace Maib.Sdk.Demo.Models;

public class RegisterSmsTransactionViewModel
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "";
    public string? Description { get; set; }
    public string Language { get; set; } = "";
}