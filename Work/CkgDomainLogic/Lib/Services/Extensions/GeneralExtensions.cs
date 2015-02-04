// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CkgDomainLogic.General.Services;

namespace SapORM.Models
{
    public static class GeneralExtensions
    {
        public static string FormatSapSaveException(this Exception e)
        {
            return string.Format("{0}: SAP-Ausnahmefehler '{1}', Details: '{2}'", 
                        Localize.SaveFailed, e.Message, (e.InnerException != null ? e.InnerException.Message : "Keine weitere Beschreibung aus SAP verfügbar!"));
        }

        public static string FormatSapSaveResultMessage(this string resultMessage)
        {
            return string.Format("{0}: {1}", Localize.SaveFailed, resultMessage);
        }
    }
}
