using System.Text;

namespace Matvi.Back.Application.Common.Extensions
{
    public static class StringExtensions
    {
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

        public static string ToObfuscated(this string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(textoPlano);
            return Convert.ToBase64String(bytes);
        }

        public static string FromObfuscated(this string textoOfuscado)
        {
            if (string.IsNullOrEmpty(textoOfuscado)) return string.Empty;
            var bytes = Convert.FromBase64String(textoOfuscado);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
