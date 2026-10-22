using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Common.Dtos
{
    /// <summary>
    /// Contrato para DTOs que exponen un identificador único y su representación ofuscada segura para URLs.
    /// </summary>
    /// <typeparam name="TId">Tipo del identificador primario.</typeparam>
    public interface IIdDto<TId>
    {
        /// <summary>
        /// Identificador único de la entidad.
        /// </summary>
        TId Id { get; set; }

        /// <summary>
        /// Identificador ofuscado seguro para transporte y consumo en interfaz de usuario.
        /// </summary>
        string SafeId { get; }
    }
}
