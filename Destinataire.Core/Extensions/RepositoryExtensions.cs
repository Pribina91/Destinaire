using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Destinataire.Core.Helpers;

namespace Destinataire.Core.Extensions
{
    public static class RepositoryExtensions
    {
        public static IQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> source, string sortSpecification)
        {
            if (string.IsNullOrWhiteSpace(sortSpecification)) return source;

            var query = source;

            var orderParams = sortSpecification.Trim().Split(',');
            //var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var index = 0;

            var orderLambdas = GenerateLambdas<TSource>(orderParams).ToList();
            var first = orderLambdas.FirstOrDefault();
            if (first != null)
            {
                var local = orderParams[0].EndsWith(" desc")
                    ? query.OrderBy(first)
                    : query.OrderByDescending(first);

                for (int i = 1; i < orderLambdas.Count(); i++)
                {
                    local = orderParams[i].EndsWith(" desc")
                        ? local.ThenBy(orderLambdas[i])
                        : local.ThenByDescending(orderLambdas[i]);
                }

                return local;
            }

            return query;
        }

        private static IEnumerable<Expression<Func<TSource, dynamic>>> GenerateLambdas<TSource>(string[] orderParams)
        {
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var lambda = CreateExpression<TSource>(param);
                yield return lambda;
            }
        }

        private static Expression<Func<TSource, dynamic>> CreateExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            var body = Expression.Convert(Expression.Property(param, propertyName), typeof(object));
            return Expression.Lambda<Func<TSource, dynamic>>(body, param);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}