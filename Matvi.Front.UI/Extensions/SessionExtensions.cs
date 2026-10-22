using System.Text.Json;

namespace Matvi.Front.UI.Extensions
{
        /// <summary>
    /// Representa la clase para session extensions.
    /// </summary>
public static class SessionExtensions
    {
                /// <summary>
        /// Ejecuta la operación de set object.
        /// </summary>
        /// <param name="session">Parámetro session.</param>
        /// <param name="key">Parámetro key.</param>
        /// <param name="value">Parámetro value.</param>
public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

                /// <summary>
        /// Recupera la información correspondiente a get object.
        /// </summary>
        /// <param name="session">Parámetro session.</param>
        /// <param name="key">Parámetro key.</param>
        /// <returns>Objeto con la información resultante de get object.</returns>
public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
