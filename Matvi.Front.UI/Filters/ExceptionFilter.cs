using Matvi.Front.UI.Dto;
using Matvi.Transversal.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;

namespace Matvi.Front.UI.Filters
{
        /// <summary>
    /// Representa la clase para exception filter.
    /// </summary>
public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

                /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ExceptionFilter"/>.
        /// </summary>
        /// <param name="logger">Parámetro logger.</param>
public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

                /// <summary>
        /// Ejecuta la operación de on exception.
        /// </summary>
        /// <param name="context">Parámetro context.</param>
public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var exceptionMessage = ObtenerMensajeCompleto(ex);
            var idError = $"{DateTime.Now:ddMMyyyy-HHmmssfff}";

            LogError(context, idError, exceptionMessage, ex.StackTrace);

            ErrorResponseDto errorDto;

            switch (ex)
            {
                case InformativeException infoEx:
                    errorDto = new ErrorResponseDto
                    {
                        ErrorCodeDescription = infoEx.Message.Trim(),
                    };
                    break;

                case DuplicateKeyException dupEx:
                    errorDto = new ErrorResponseDto
                    {
                        ErrorCode = idError,
                        ErrorCodeDescription = dupEx.Message.Trim(),
                    };
                    break;

                default:
                    errorDto = new ErrorResponseDto
                    {
                        ErrorCode = idError,
                        ErrorCodeDescription = "Ocurrió un error en el proceso. Por favor reporte el código de error al área de soporte.",
                    };
                    break;
            }

            // Bifurcación AJAX/no-AJAX
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult(errorDto);
            }
            else
            {
                context.Result = new RedirectToPageResult("/Error", new { errorCode = errorDto.ErrorCode });
            }
        }

        private string ObtenerMensajeCompleto(Exception ex)
        {
            var mensaje = new StringBuilder();
            while (ex != null)
            {
                mensaje.AppendLine(ex.Message);
                ex = ex.InnerException!;
            }
            return mensaje.ToString();
        }

        private void LogError(ExceptionContext context, string idError, string mensaje, string? stackTrace)
        {
            var log = new StringBuilder();
            log.AppendLine();
            log.AppendLine("###################################################################################################################################################################################################");
            log.AppendLine($"Id: {idError}");
            log.AppendLine($"Method: {context.HttpContext.Request.Method}");
            log.AppendLine($"Host: {context.HttpContext.Request.Host}");
            log.AppendLine($"Path: {context.HttpContext.Request.Path}");
            log.AppendLine($"Message: {mensaje}");
            log.AppendLine($"StackTrace: {stackTrace}");
            log.AppendLine("###################################################################################################################################################################################################");
            log.AppendLine();

            _logger.LogError(log.ToString());
        }
    }
}