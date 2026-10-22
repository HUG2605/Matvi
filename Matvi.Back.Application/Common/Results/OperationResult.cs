namespace Matvi.Back.Application.Common.Results
{
        /// <summary>
    /// Representa la registro (record) para operation result.
    /// </summary>
public record OperationResult<T>(bool IsSuccessful, T? Data, string? Message, MessageType MessageType)
    {
                /// <summary>
        /// Ejecuta la operación de success.
        /// </summary>
        /// <param name="data">Parámetro data.</param>
        /// <param name="message">Parámetro message.</param>
        /// <returns>Resultado de la operación que incluye estado, mensaje y datos generados.</returns>
public static OperationResult<T> Success(T data, string message)
            => new(true, data, message, MessageType.Success);

                /// <summary>
        /// Ejecuta la operación de info.
        /// </summary>
        /// <param name="data">Parámetro data.</param>
        /// <param name="message">Parámetro message.</param>
        /// <returns>Resultado de la operación que incluye estado, mensaje y datos generados.</returns>
public static OperationResult<T> Info(T data, string message)
            => new(true, data, message, MessageType.Info);

                /// <summary>
        /// Ejecuta la operación de warning.
        /// </summary>
        /// <param name="data">Parámetro data.</param>
        /// <param name="message">Parámetro message.</param>
        /// <returns>Resultado de la operación que incluye estado, mensaje y datos generados.</returns>
public static OperationResult<T> Warning(T data, string message)
            => new(true, data, message, MessageType.Warning);

                /// <summary>
        /// Ejecuta la operación de error.
        /// </summary>
        /// <param name="message">Parámetro message.</param>
        /// <returns>Resultado de la operación que incluye estado, mensaje y datos generados.</returns>
public static OperationResult<T> Error(string message)
            => new(false, default, message, MessageType.Error);
    }
}
