using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk
{
    [ExcludeFromCodeCoverage]
    internal static class MaibCommands
    {
        public const string RegisterSmsTransaction = "v";
        public const string RegisterDmsAuthorization = "a";
        public const string MakeDmsTransaction = "t";
        public const string GetTransactionResult = "c";
        public const string RevertTransaction = "r";
        public const string CloseDay = "b";
        public const string Refund = "k";
    }
}