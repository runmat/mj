using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GeneralTools.Models;
using MvcTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc.UI.Fluent;

namespace Telerik.Web.Mvc.UI
{
    public static class GridExtensions
    {
        public static GridBoundColumnBuilder<TModel> XBoundAllExcept<TModel>(this GridColumnFactory<TModel> builder, Type alternativeType, params Expression<Func<TModel, object>>[] propertiesToExclude)
            where TModel : class
        {
            return builder.XBoundAllPrivate(alternativeType, propertiesToExclude);
        }

        public static GridBoundColumnBuilder<TModel> XBoundAllExcept<TModel>(this GridColumnFactory<TModel> builder, params Expression<Func<TModel, object>>[] propertiesToExclude)
            where TModel : class
        {
            return builder.XBoundAllPrivate(null, propertiesToExclude);
        }

        public static GridBoundColumnBuilder<TModel> XBoundAll<TModel>(this GridColumnFactory<TModel> builder, Type alternativeType=null)
            where TModel : class
        {
            return builder.XBoundAllPrivate(alternativeType);
        }

        private static GridBoundColumnBuilder<TModel> XBoundAllPrivate<TModel>(this GridColumnFactory<TModel> builder, Type alternativeType = null, params Expression<Func<TModel, object>>[] propertiesToExclude)
            where TModel : class
        {
            var modelType = typeof(TModel);
            if (alternativeType != null)
                modelType = alternativeType;

            GridBoundColumnBuilder<TModel> column = null;

            var modelColumnList = modelType.GetScaffoldPropertyNames().ToListOrEmptyList(); 
            var userSlaveColumnsToHide = new List<string>();
            var propertyNameList =  modelColumnList.Where(slave => modelType.GetScaffoldPropertyLowerNames().Contains(slave.ToLower())).ToList();
            propertyNameList.ForEach(propertyName =>
                        {
                            if (propertiesToExclude == null || propertiesToExclude.None() || propertiesToExclude.None(pExclude => pExclude.GetPropertyName(true).ToLower() == propertyName.ToLower()))
                                column = builder.XBound(propertyName, !userSlaveColumnsToHide.Contains(propertyName));
                        });

            return column;
        }
    }
}
