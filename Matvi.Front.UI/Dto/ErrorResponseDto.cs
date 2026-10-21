namespace Matvi.Front.UI.Dto
{
    public class ErrorResponseDto
    {
        public string ErrorCode { get; set; } = string.Empty;

        public string ExceptionMessage { get; set; } = string.Empty;

        public string ErrorCodeDescription { get; set; } = string.Empty;

        public string StackTrace { get; set; } = string.Empty;
    }
}
