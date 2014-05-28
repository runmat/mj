using System.Web.Mvc.Ajax;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public class MvcAjaxOptions : AjaxOptions
    {
        public static string AjaxFormContainerName { get { return "ajaxFormContainer"; } }

        public new string OnComplete
        {
            get { return base.OnComplete; }
            set { base.OnComplete = string.Format("{0}{1}", "try { LoadingHide(); } catch (e) {} ", value.NotNullOrEmpty()); }
        }

        public MvcAjaxOptions(int formID = -1)
        {
            UpdateTargetId = string.Format("{0}{1}", AjaxFormContainerName, formID == -1 ? "" : formID.ToString());
            //OnSuccess = "$.validator.unobtrusive.parse('form');";
            InsertionMode = InsertionMode.Replace;
        }

        public MvcAjaxOptions(string targetId)
        {
            UpdateTargetId = targetId;
            InsertionMode = InsertionMode.Replace;
        }
    }
}
