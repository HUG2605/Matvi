using Matvi.Back.Application.Common.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Matvi.Front.UI
{
    public class BaseModel : PageModel
    {
        public string ToString(string cfrValue)
        {
            return HttpUtility.UrlDecode(cfrValue.FromObfuscated());
        }

        public byte ToByte(string cfrValue)
        {
            return Convert.ToByte(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public short ToShort(string cfrValue)
        {
            return Convert.ToInt16(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public int ToInt(string cfrValue)
        {
            return Convert.ToInt32(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public long ToLong(string cfrValue)
        {
            return Convert.ToInt64(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public decimal ToDecimal(string cfrValue)
        {
            return Convert.ToDecimal(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public static DateTime ToDateTime(string cfrValue)
        {
            return Convert.ToDateTime(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

        public bool ToBoolean(string cfrValue)
        {
            return Convert.ToBoolean(HttpUtility.UrlDecode(cfrValue.FromObfuscated()));
        }

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
