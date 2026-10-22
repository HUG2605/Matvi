using System.Text;

namespace Matvi.Back.Application.Common.Extensions
{
        /// <summary>
    /// Representa la clase para string extensions.
    /// </summary>
public static class StringExtensions
    {
                /// <summary>
        /// Ejecuta la operación de to capitalize.
        /// </summary>
        /// <param name="texto">Parámetro texto.</param>
        /// <returns>Objeto con la información resultante de to capitalize.</returns>
public static string ToCapitalize(this string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var oraciones = texto.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var resultado = new List<string>();

            foreach (var oracion in oraciones)
            {
                var trimmed = oracion.TrimStart();
                if (string.IsNullOrWhiteSpace(trimmed))
                    continue;

                var capitalizada = char.ToUpper(trimmed[0]) + trimmed.Substring(1).ToLower();
                resultado.Add(capitalizada);
            }

            return string.Join(". ", resultado) + (texto.EndsWith(".") ? "." : "");
        }

                /// <summary>
        /// Ejecuta la operación de to obfuscated.
        /// </summary>
        /// <param name="textoPlano">Parámetro texto plano.</param>
        /// <returns>Objeto con la información resultante de to obfuscated.</returns>
public static string ToObfuscated(this string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(textoPlano);
            return Convert.ToBase64String(bytes);
        }

                /// <summary>
        /// Ejecuta la operación de from obfuscated.
        /// </summary>
        /// <param name="textoOfuscado">Parámetro texto ofuscado.</param>
        /// <returns>Objeto con la información resultante de from obfuscated.</returns>
public static string FromObfuscated(this string textoOfuscado)
        {
            if (string.IsNullOrEmpty(textoOfuscado)) return string.Empty;
            var bytes = Convert.FromBase64String(textoOfuscado);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
