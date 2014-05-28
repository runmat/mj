namespace PortalMvcTools.Models
{
    public class FormInnerLayerModel
    {
        public int ID { get; set; }

        public string Header { get; set; }

        private string _headerCssClass;
        private string _formOpenerCssClass = "formopener";

        public string HeaderCssClass
        {
            get { return string.IsNullOrEmpty(_headerCssClass) ? "" : string.Format("class=\"{0}\"", _headerCssClass); }
            set { _headerCssClass = value; }
        }

        public string FormID { get { return string.Format("form{0}", ID); } }

        public string FormLayerTopID { get { return string.Format("formlayer_top{0}", ID); } }

        public string FormOpenerID { get { return string.Format("formopener{0}", ID); } }

        public string FormValidationErrorDivID { get { return string.Format("form_validation_error{0}", ID); } }

        public string FormOpenerCssClass
        {
            get { return _formOpenerCssClass; }
            set { _formOpenerCssClass = value; }
        }
    }
}
