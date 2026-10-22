using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Transversal.Exceptions
{
    /// <summary>
    /// Excepción que se produce cuando se intenta registrar una entidad con una clave única o duplicada que ya existe en el sistema.
    /// </summary>
    public class DuplicateKeyException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DuplicateKeyException"/> con un mensaje descriptivo del error.
        /// </summary>
        /// <param name="message">Mensaje que detalla la causa de la duplicidad de clave.</param>
        public DuplicateKeyException(string message) : base(message)
        {
        }

    }

}
