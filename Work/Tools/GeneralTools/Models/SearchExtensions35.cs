using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GeneralTools.Models
{
    public static class SearchExtensions35
    {
        //
        // CAUTION: Please take care of corresponding (similar) code in .NET 4.0 lib  "CkgDomainLogic", class "SearchExtensions40"
        //

        public static IQueryable<T> SearchPropertiesWithOrCondition<T>(this IQueryable<T> source, string searchTerm, string stringProperties)
        {
            searchTerm = searchTerm.ToLower();

            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm)
            //                || x.[property2].Contains(searchTerm)
            //                || x.[property3].Contains(searchTerm)...)

            var searchTermExpression = Expression.Constant(searchTerm);

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            //Build a contains expression for each property
            var stringPropertyList = stringProperties.Split(',', ';', '~').Select(s => s.Trim());
            foreach (var propertyName in stringPropertyList)
            {
                var propertyInfo = type.GetProperty(propertyName.ToLower(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo == null)
                    continue;

                var propertyReference = Expression.Property(parameter, propertyName);
                var lambdaExpression = Expression.Lambda(propertyReference, new[] { parameter });
                var propertyType = propertyInfo.PropertyType;
                //Build expression to represent x.[propertyX].Contains(searchTerm)
                var containsExpression = BuildContainsExpression(propertyType, lambdaExpression, searchTermExpression);

                orExpression = BuildOrExpression(orExpression, containsExpression);
            }

            if (orExpression == null)
                return source;

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
            return source.Where(completeExpression);
        }

        public static List<T> SearchPropertiesWithOrCondition<T>(this List<T> source, string searchTerm, string stringProperties)
        {
            return source.AsQueryable().SearchPropertiesWithOrCondition(searchTerm, stringProperties).ToList();
        }


        private static Expression BuildOrExpression(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
                return expressionToAdd;

            //Build 'OR' expression for each property
            return Expression.OrElse(existingExpression, expressionToAdd);
        }

        private static MethodCallExpression BuildContainsExpression(Type type, LambdaExpression stringProperty, ConstantExpression searchTermExpression)
        {
            MethodCallExpression toLowerLeftExpression;
            if (type == typeof(string))
                toLowerLeftExpression = Expression.Call(null, typeof(StringExtensions).GetMethod("NotNullOrEmpty", new[] { typeof(string) }), stringProperty.Body);
            else
                toLowerLeftExpression = Expression.Call(stringProperty.Body, type.GetMethod("ToString", new Type[] { }));

            toLowerLeftExpression = Expression.Call(toLowerLeftExpression, typeof(string).GetMethod("ToLower", new Type[] { }));
            var toLowerRightExpression = searchTermExpression; 

            return Expression.Call(toLowerLeftExpression, typeof(string).GetMethod("Contains"), toLowerRightExpression);
        }
    }
}
