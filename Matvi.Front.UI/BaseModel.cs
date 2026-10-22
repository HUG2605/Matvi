using Matvi.Back.Application.Common.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Matvi.Front.UI
{
        /// <summary>
    /// Representa la clase para base.
    /// </summary>
public class BaseModel : PageModel
    {
                /// <summary>
        /// Ejecuta la operación de to string.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to string.</returns>
public string ToString(string cfrValue)
        {
            return HttpUtility.UrlDecode(cfrValue.FromObfuscated());
        }

                /// <summary>
        /// Ejecuta la operación de to byte.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to byte.</returns>
public byte ToByte(string cfrValue)
        {
            return Convert.ToByte(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to short.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to short.</returns>
public short ToShort(string cfrValue)
        {
            return Convert.ToInt16(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to int.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to int.</returns>
public int ToInt(string cfrValue)
        {
            return Convert.ToInt32(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to long.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to long.</returns>
public long ToLong(string cfrValue)
        {
            return Convert.ToInt64(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to decimal.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to decimal.</returns>
public decimal ToDecimal(string cfrValue)
        {
            return Convert.ToDecimal(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to date time.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns>Objeto con la información resultante de to date time.</returns>
public static DateTime ToDateTime(string cfrValue)
        {
            return Convert.ToDateTime(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Ejecuta la operación de to boolean.
        /// </summary>
        /// <param name="cfrValue">Parámetro cfr value.</param>
        /// <returns><c>true</c> si la operación se completó exitosamente; en caso contrario, <c>false</c>.</returns>
public bool ToBoolean(string cfrValue)
        {
            return Convert.ToBoolean(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

                /// <summary>
        /// Recupera la información correspondiente a get profile letters.
        /// </summary>
        /// <param name="value">Parámetro value.</param>
        /// <returns>Objeto con la información resultante de get profile letters.</returns>
public string GetProfileLetters(string value)
        {
            var palabras = value.Split(" ");

            string? inicialesPerfil;

            if (palabras.Length > 1)
            {
                inicialesPerfil = $"{palabras.First().Substring(0, 1).ToUpper()}{palabras.Last().Substring(0, 1).ToUpper()}";
            }
            else
            {
                inicialesPerfil = $"{palabras.First().Substring(0, 1).ToUpper()}";
            }

            return inicialesPerfil;
        }

                /// <summary>
        /// Ejecuta la operación de i form file to byte array.
        /// </summary>
        /// <param name="formFile">Parámetro form file.</param>
        /// <returns>Objeto con la información resultante de i form file to byte array.</returns>
public byte[]? IFormFileToByteArray(IFormFile? formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                fileBytes = ms.ToArray();
                // string s = Convert.ToBase64String(fileBytes);
            }

            return fileBytes;
        }

    }
}
