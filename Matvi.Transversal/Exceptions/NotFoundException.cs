using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Transversal.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando una entidad solicitada no existe en la base de datos o almacenamiento.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Nombre descriptivo del tipo o tabla de la entidad no localizada.
        /// </summary>
        public string NombreEntidad { get; }

        /// <summary>
        /// Clave primaria o identificador de búsqueda que no fue encontrado.
        /// </summary>
        public object Clave { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/> indicando la entidad y clave faltante.
        /// </summary>
        /// <param name="nombreEntidad">Nombre de la entidad no encontrada.</param>
        /// <param name="clave">Identificador o clave que no se pudo encontrar.</param>
        public NotFoundException(string nombreEntidad, object clave)
            : base($"No se encontró la entidad '{nombreEntidad}' con clave '{clave}'.")
        {
            NombreEntidad = nombreEntidad;
            Clave = clave;
        }
    }

}
