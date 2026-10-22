namespace Matvi.Front.UI.Dto
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para error response.
    /// </summary>
public class ErrorResponseDto
    {
                /// <summary>
        /// Propiedad que representa error code.
        /// </summary>
public string ErrorCode { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa exception message.
        /// </summary>
public string ExceptionMessage { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa error code description.
        /// </summary>
public string ErrorCodeDescription { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa stack trace.
        /// </summary>
public string StackTrace { get; set; } = string.Empty;
    }
}
