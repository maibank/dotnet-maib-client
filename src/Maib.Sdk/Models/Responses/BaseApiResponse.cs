using System.Diagnostics.CodeAnalysis;

namespace Maib.Sdk.Models.Responses
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseApiResponse
    {
        protected BaseApiResponse()
        {

        }

        protected BaseApiResponse(BaseApiResponse apiResponse)
        {
            Success = apiResponse.Success;
            Error = apiResponse.Error;
        }

        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}