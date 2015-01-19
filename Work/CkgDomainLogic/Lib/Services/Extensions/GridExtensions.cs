using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc.UI.Fluent;

namespace Telerik.Web.Mvc.UI
{
    public static class GridExtensions
    {
        private static string GridGroup { get { return SessionHelper.GetSessionString("GridGroup"); } }

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

            var userMasterColumns = GetUserGridColumnNames(modelType, GridColumnMode.Master, GridGroup);
            var userSlaveColumns = GetUserGridColumnNames(modelType, GridColumnMode.Slave, GridGroup);
            var userSlaveColumnsToHide = userMasterColumns.Except(userSlaveColumns);

            userMasterColumns = SortMasterColumnsLikeSlaveColumns(userMasterColumns, userSlaveColumns);

            GridBoundColumnBuilder<TModel> column = null;

            var modelColumnList = userMasterColumns;
            var propertyNameList =  modelColumnList.Where(slave => modelType.GetScaffoldPropertyLowerNames().Contains(slave.ToLower())).ToList();
            propertyNameList.ForEach(propertyName =>
                        {
                            if (propertiesToExclude == null || propertiesToExclude.None() || propertiesToExclude.None(pExclude => pExclude.GetPropertyName().ToLower() == propertyName.ToLower()))
                                column = builder.XBound(propertyName, !userSlaveColumnsToHide.Contains(propertyName));
                        });

            return column;
        }

        static List<string> SortMasterColumnsLikeSlaveColumns(List<string> masterColumns, List<string> slaveColumns)
        {
            return slaveColumns.Clone().Concat(masterColumns.Except(slaveColumns)).ToList();
        }

        static List<string> GetUserGridColumnNames(Type modelType, GridColumnMode gridColumnMode, string gridGroup)
        {
            var logonContext = (SessionStore.GetCurrentLogonContext() as ILogonContextDataService);
            if (logonContext == null)
                return new List<string>();

            return logonContext.GetUserGridColumnNames(modelType, gridColumnMode, gridGroup).Split('~', ' ', ',', ';').ToList();
        }
    }
}
