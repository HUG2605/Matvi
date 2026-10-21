namespace Matvi.Back.Application.Common.Dtos
{
    public class BaseDto<TId>
    {
        public TId Id { get; set; } = default!;

        public string Nombre { get; set; } = string.Empty;
    }
}
