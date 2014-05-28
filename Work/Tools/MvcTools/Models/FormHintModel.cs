using GeneralTools.Models;

namespace MvcTools.Models
{
    public class FormHintModel : GeneralEntity
    {
        public enum Mode
        {
            Error,
            Info,
            Success,
        };

        public Mode HintMode { get; set; }

        public string CssClass { get { return string.Format("alert-{0}", InnerCssClass); } }

        private string InnerCssClass
        {
            get
            {
                switch (HintMode)
                {
                    case Mode.Error:
                        return "error";

                    case Mode.Success:
                        return "success";
                }

                return "info";
            }
        }
    }
}
