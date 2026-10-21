using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Common.Dtos
{
    public interface IIdDto<TId>
    {
        TId Id { get; set; }
        string SafeId { get; }
    }
}
