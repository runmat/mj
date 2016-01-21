using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Models
{
    public enum FormMultiColumnMode { None, Left, Right, Left3, Middle3, Right3 }

    public class FormControlModel
    {
        public string ModelTypeName { get; set; }

        public string PropertyName { get; set; }

        public IKeyValueStore<string> KeyStringStore { get { return DependencyResolver.Current.GetService<IKeyValueStore<string>>(); } }

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

        /// <summary>
        /// Is this a checkbox?
        /// </summary>
        public bool IsCheckBox { get; set; }

        /// <summary>
        /// optionally hide left positioned label 
        /// </summary>
        public bool LabelHidden { get; set; }

        /// <summary>
        /// optionally collapse whole control with all surrounding html templates
        /// </summary>
        public bool IsCollapsed { get; set; }

        public bool IsGrayed { get; set; }

        private FormMultiColumnMode _columnMode = FormMultiColumnMode.None;
        public FormMultiColumnMode ColumnMode
        {
            get
            {
                if (_columnMode != FormMultiColumnMode.None)
                    return _columnMode;

                if (ControlHtmlAttributes == null || !ControlHtmlAttributes.ContainsKey("col"))
                    return FormMultiColumnMode.None;

                var colValue = ControlHtmlAttributes["col"].ToString();
                var autoMode = AutoMultiColumnModeTryGetAndIncrementValue(colValue);

                _columnMode = (autoMode != FormMultiColumnMode.None) ? autoMode : GetMultiColumnMode(colValue);

                return _columnMode;
            }
        }

        public IHtmlString PreControlHtml { get; set; }

        public IHtmlString PostControlHtml { get; set; }

        /// <summary>
        /// indicates if this property is persistable
        /// </summary>
        public MvcHtmlString PerstistenceIndicatorHtml { get; set; }

        public void AutoMultiColumnModeStart(string autoKey)
        {
            KeyStringStore.SetValue(autoKey, "");
        }

        public bool AutoMultiColumnModeEndReached(string autoKey)
        {
            return AutoMultiColumnModeTryGetAndIncrementValue(autoKey).ToString("F").ToLower().StartsWith("left");
        }

        FormMultiColumnMode AutoMultiColumnModeTryGetAndIncrementValue(string autoKey)
        {
            if (!autoKey.ToLower().StartsWith("auto"))
                return FormMultiColumnMode.None;

            var storedValue = KeyStringStore.GetValue(autoKey);

            var nextValue = IncrementMultiColumnMode(autoKey, ref storedValue);

            KeyStringStore.SetValue(autoKey, nextValue.ToString("F").ToLower());

            return GetMultiColumnMode(storedValue);
        }

        private static FormMultiColumnMode GetMultiColumnMode(string value)
        {
            switch (value)
            {
                case "left":
                    return FormMultiColumnMode.Left;

                case "right":
                    return FormMultiColumnMode.Right;

                case "left3":
                    return FormMultiColumnMode.Left3;

                case "middle3":
                    return FormMultiColumnMode.Middle3;

                case "right3":
                    return FormMultiColumnMode.Right3;

                default:
                    return FormMultiColumnMode.None;
            }
        }

        private FormMultiColumnMode IncrementMultiColumnMode(string autoKey, ref string storedValue)
        {
            FormMultiColumnMode nextValue;

            if (autoKey.EndsWith("3"))
            {
                // 3 Column mode

                if (storedValue.IsNullOrEmpty())
                    storedValue = "left3";

                if (IsCollapsed)
                    return GetMultiColumnMode(storedValue);

                switch (storedValue)
                {
                    case "left3":
                        nextValue = FormMultiColumnMode.Middle3;
                        break;

                    case "middle3":
                        nextValue = FormMultiColumnMode.Right3;
                        break;

                    case "right3":
                        nextValue = FormMultiColumnMode.Left3;
                        break;

                    default:
                        nextValue = FormMultiColumnMode.None;
                        break;
                }
            }
            else
            {
                // 2 Column mode

                if (storedValue.IsNullOrEmpty())
                    storedValue = "left";

                if (IsCollapsed)
                    return GetMultiColumnMode(storedValue);

                switch (storedValue)
                {
                    case "left":
                        nextValue = FormMultiColumnMode.Right;
                        break;

                    case "right":
                        nextValue = FormMultiColumnMode.Left;
                        break;

                    default:
                        nextValue = FormMultiColumnMode.None;
                        break;
                }
            }

            return nextValue;
        }
    }
}
