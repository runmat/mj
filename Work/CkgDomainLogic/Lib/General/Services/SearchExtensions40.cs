using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public static class SearchExtensions40
    {
        //
        // CAUTION: Please take care of corresponding (similar) code in .NET 3.5 lib  "GeneralTools", class "SearchExtensions35"
        //

        public static IQueryable<object> SearchPropertiesWithOrCondition(this IQueryable<object> source, string searchTerm, string stringProperties, Type alternativeType)
        {
            searchTerm = searchTerm.ToLower();

            if (String.IsNullOrEmpty(searchTerm))
                return source;

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm)
            //                || x.[property2].Contains(searchTerm)
            //                || x.[property3].Contains(searchTerm)...)

            var searchTermExpression = Expression.Constant(searchTerm);

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;

            var type = alternativeType;
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

            var delegateType = Expression.GetDelegateType(type, typeof(bool));  // Create Delegate "Func<type, bool>"
            var completeExpression = Expression.Lambda(delegateType, orExpression, parameter);   // Generic Pendant:  "Expression.Lambda<Func<type, bool>(orExpression, parameter);"
            var compiledExpression = completeExpression.Compile();

            var query = source.Where(s => (bool)compiledExpression.DynamicInvoke(new[] { s }));

            return query;
        }

        public static List<object> SearchPropertiesWithOrCondition(this List<object> source, string searchTerm, string stringProperties, Type alternativeType)
        {
            return source.AsQueryable().SearchPropertiesWithOrCondition(searchTerm, stringProperties, alternativeType).ToList();
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
