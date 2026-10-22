using System.Globalization;

namespace Matvi.Front.UI.Extensions
{
        /// <summary>
    /// Representa la clase para format extensions.
    /// </summary>
public static class FormatExtensions
    {
                /// <summary>
        /// Ejecuta la operación de to phone format.
        /// </summary>
        /// <param name="input">Parámetro input.</param>
        /// <returns>Objeto con la información resultante de to phone format.</returns>
public static string ToPhoneFormat(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 10 || !input.All(char.IsDigit))
                return input; // O lanzar excepción si prefieres validación estricta

            return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6, 4)}";
        }

                /// <summary>
        /// Ejecuta la operación de to money format.
        /// </summary>
        /// <param name="input">Parámetro input.</param>
        /// <returns>Objeto con la información resultante de to money format.</returns>
public static string ToMoneyFormat(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            // Intentar convertir a decimal
            if (!decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
                return "$0.00";

            // Regresar formato moneda MX
            return value.ToString("C2", new CultureInfo("es-MX"));
        }

                /// <summary>
        /// Ejecuta la operación de to custom decimal format.
        /// </summary>
        /// <param name="input">Parámetro input.</param>
        /// <returns>Objeto con la información resultante de to custom decimal format.</returns>
public static string ToCustomDecimalFormat(this decimal input)
        {
            var culture = new CultureInfo("es-MX");

            if (input % 1 == 0)
                return input.ToString("#,0", culture);

            var str = input.ToString(CultureInfo.InvariantCulture);
            var decimals = str.Contains('.') ? str.Split('.')[1].TrimEnd('0') : "";

            if (decimals.Length == 1)
                return input.ToString("#,0.00", culture);

            return input.ToString("#,0.################", culture);
        }

                /// <summary>
        /// Ejecuta la operación de to date time format.
        /// </summary>
        /// <param name="value">Parámetro value.</param>
        /// <returns>Objeto con la información resultante de to date time format.</returns>
public static string ToDateTimeFormat(this DateTime? value)
        {
            if (!value.HasValue)
                return string.Empty;

            return value.Value.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }

                /// <summary>
        /// Ejecuta la operación de to date time format.
        /// </summary>
        /// <param name="value">Parámetro value.</param>
        /// <returns>Objeto con la información resultante de to date time format.</returns>
public static string ToDateTimeFormat(this DateTime value)
        {
            return value.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Trunca (no redondea) a 2 decimales y formatea como moneda MX.
        /// Ej: 99.999 → "$99.99"
        /// </summary>
        public static string ToTruncateMoney(this decimal value)
        {
            var truncated = decimal.Truncate(value * 100) / 100;
            return truncated.ToString("C", CultureInfo.GetCultureInfo("es-MX"));
        }

        /// <inheritdoc cref="ToTruncateMoney(decimal)"/>
        public static string ToTruncateMoney(this decimal? value)
        {
            return (value ?? 0m).ToTruncateMoney();
        }

    }
}
