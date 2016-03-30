using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ADServerDAL.Helpers
{
    /// <summary>
    /// Klasa pomocniczna do zapytań LINQ
    /// </summary>
    public static class LinqExtentions
    {
        /// <summary>
        /// Metoda pozwalająca sortować query wg podanej nazwy pola
        /// </summary>
        /// <typeparam name="T">Typ obiektów query</typeparam>
        /// <param name="source">Kolekcja obiektów</param>
        /// <param name="property">Nazwa pola do sortowania</param>
        /// <param name="acceding">Czy sortować rosnąco</param>        
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, bool acceding)
        {
            return ApplyOrder<T>(source, property, acceding ? "OrderBy" : "OrderByDescending");
        }

        /// <summary>
        /// Właściwa metoda sortująca
        /// </summary>
        /// <typeparam name="T">Typ obiektów query</typeparam>
        /// <param name="source">Kolekcja obiektów</param>
        /// <param name="property">Nazwa pola do sortowania</param>
        /// <param name="methodName">Nazwa metody sortującej</param>        
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            System.Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                ///Pobierz właściwość odpowiadającą sortowanemu polu
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            
            ///Zbuduj wyrażenie Lambda
            System.Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            ///Wywołaj metodę sortującą na wyrażeniu
            object result = typeof(Queryable).GetMethods().Single(method => method.Name == methodName
                                                                         && method.IsGenericMethodDefinition
                                                                         && method.GetGenericArguments().Length == 2
                                                                         && method.GetParameters().Length == 2)
                                                         .MakeGenericMethod(typeof(T), type)
                                                         .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}