using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcTools.Models
{
    public enum Form2ColumnMode { None, Left, Right }

    public class FormControlModel
    {
        /// <summary>
        /// label text, grid header, etc
        /// </summary>
        public MvcHtmlString DisplayNameHtml { get; set; }

        /// <summary>
        /// will be automatically filled if the corresponding property has a "Required" attribute
        /// </summary>
        public MvcHtmlString RequiredIndicatorHtml { get; set; }

        /// <summary>
        /// the raw html control (textbox, checkbox, select, etc)
        /// </summary>
        public MvcHtmlString ControlHtml { get; set; }

        /// <summary>
        /// validation error message for this property
        /// </summary>
        public MvcHtmlString ValidationMessageHtml { get; set; }

        /// <summary>
        /// optional: Icon behind the control
        /// </summary>
        public string IconCssClass { get; set; }

        /// <summary>
        /// used for inline labels (i. e. directly behind checkboxes)
        /// </summary>
        public MvcHtmlString InlineDisplayNameHtml { get; set; }

        /// <summary>
        /// used for special template control purpose
        /// </summary>
        public IDictionary<string, object> ControlHtmlAttributes { get; set; }

        public Form2ColumnMode ColumnMode
        {
            get
            {
                if (ControlHtmlAttributes == null || !ControlHtmlAttributes.ContainsKey("col"))
                    return Form2ColumnMode.None;

                return ControlHtmlAttributes["col"].ToString() == "left" ? Form2ColumnMode.Left : Form2ColumnMode.Right;
            }
        }

        public IHtmlString PreControlHtml { get; set; }

        public IHtmlString PostControlHtml { get; set; }
    }
}
