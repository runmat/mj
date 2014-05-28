using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    [ToolboxData("<{0}:EitherOrValidator runat=\"server\"></{0}:EitherOrValidator>")]
    public class EitherOrValidator : BaseValidator
    {
        [DefaultValue("")]
        [TypeConverter(typeof(ValidatedControlConverter))]
        [Themeable(false)]
        [Category("Behavior")]
        [Description("Other control which must be filled if ControlToValidate is empty.")]
        public string OrControlToValidate
        {
            get
            {
                var ret = (string)this.ViewState["OrControlToValidate"];
                return ret ?? String.Empty;
            }

            set
            {
                this.ViewState["OrControlToValidate"] = value;
            }
        }

        protected override bool ControlPropertiesValid()
        {
            this.CheckControlValidationProperty(this.OrControlToValidate, "OrControlToValidate");

            if (StringComparer.OrdinalIgnoreCase.Equals(base.ControlToValidate, this.OrControlToValidate))
            {
                throw new HttpException(String.Format("Validator_bad_compare_control", this.ID, this.OrControlToValidate));
            }

            return base.ControlPropertiesValid();
        }

        protected override bool EvaluateIsValid()
        {
            var controlValidationValue = this.GetControlValidationValue(this.ControlToValidate);
            var controlValidationValueGood = controlValidationValue == null || !String.IsNullOrEmpty(controlValidationValue.Trim());

            var orControlValidationValue = this.GetControlValidationValue(this.OrControlToValidate);
            var orControlValidationValueGood = orControlValidationValue == null || !String.IsNullOrEmpty(orControlValidationValue.Trim());

            return controlValidationValueGood || orControlValidationValueGood;
        }
    }
}
