using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Extensions
{
    /// <summary>
    /// Métodos de extensión para manipulación y ordenamiento dinámico sobre consultas <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Aplica ordenamiento ascendente o descendente dinámico sobre un <see cref="IQueryable{T}"/> a partir del nombre de la propiedad.
        /// </summary>
        /// <typeparam name="T">Tipo del elemento consultado.</typeparam>
        /// <param name="source">Origen de datos consultable.</param>
        /// <param name="propertyName">Nombre de la propiedad por la cual ordenar.</param>
        /// <param name="descending">Indica si el orden debe ser descendente (<c>true</c>) o ascendente (<c>false</c>).</param>
        /// <returns>La consulta con el ordenamiento aplicado, o la consulta original si el nombre de propiedad está vacío.</returns>
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> source, string propertyName, bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }
    }
}
