using System.Web;
using System.Linq;

namespace MvcTools.Models
{
    public class FormWizardModel
    {
        public string Header { get; set; }

        public string HeaderIconCssClass { get; set; }

        public IHtmlString[] StepTitles { get; set; }

        public string[] StepKeys { get; set; }

        public IHtmlString StepKeysAsJavascript { get { return new HtmlString(StepKeys == null ? "" :  string.Join(",", StepKeys.Select(s => string.Format("'{0}'", s)))); } }


        public int StepCount { get { return StepTitles.Length; } }

        public bool StepTitlesInNewLine { get; set; }



        public string GetTabHref(int i)
        {
            return string.Format("#{0}", GetTabID(i));
        }

        public string GetTabID(int i)
        {
            return string.Format("formWizardPrivateTab{0}", GetStepNo(i));
        }

        public string GetStepNo(int i)
        {
            return string.Format("{0}", (i + 1));
        }

        public IHtmlString GetStepTitle(int i)
        {
            return StepTitles[i];
        }

        public string GetStepKey(int i)
        {
            return StepKeys == null || StepKeys.Length <= i ? "" : StepKeys[i];
        }

        public string GetStepTitleCssClass(int i)
        {
            return "step" + (i == 0 ? " active" : "");
        }

        public string GetStepSpanCssClass()
        {
            return "formWizardStepSpan" + StepCount + " formWizardStepSpanGeneral";
        }

        public string GetStepTabCssClass(int i)
        {
            return "tab-pane" + (i == 0 ? " active" : "");
        }
    }
}
