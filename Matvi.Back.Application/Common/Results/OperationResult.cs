namespace Matvi.Back.Application.Common.Results
{
    public record OperationResult<T>(bool IsSuccessful, T? Data, string? Message, MessageType MessageType)
    {
        public static OperationResult<T> Success(T data, string message)
            => new(true, data, message, MessageType.Success);

        public static OperationResult<T> Info(T data, string message)
            => new(true, data, message, MessageType.Info);

        public static OperationResult<T> Warning(T data, string message)
            => new(true, data, message, MessageType.Warning);

        public static OperationResult<T> Error(string message)
            => new(false, default, message, MessageType.Error);
    }
}
