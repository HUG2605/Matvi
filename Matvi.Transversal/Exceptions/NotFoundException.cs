using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Transversal.Exceptions
{
    public class NotFoundException : Exception
    {
        public string NombreEntidad { get; }
        public object Clave { get; }

        public NotFoundException(string nombreEntidad, object clave)
            : base($"No se encontró la entidad '{nombreEntidad}' con clave '{clave}'.")
        {
            NombreEntidad = nombreEntidad;
            Clave = clave;
        }
    }

}
