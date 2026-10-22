using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Transversal.Exceptions
{
    /// <summary>
    /// Excepción informativa para notificar al usuario sobre reglas de negocio o validaciones no críticas.
    /// </summary>
    public class InformativeException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="InformativeException"/> con un mensaje informativo.
        /// </summary>
        /// <param name="message">Mensaje informativo descriptivo.</param>
        public InformativeException(string message) : base(message) 
        {
        }

    }
}
