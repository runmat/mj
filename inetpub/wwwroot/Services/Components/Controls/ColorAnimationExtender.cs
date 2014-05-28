using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using CKG.Components.Controls;

[assembly: WebResource(Constants.JQueryColorPluginResource, "text/javascript")] 
namespace CKG.Components.Controls
{
    [DefaultProperty("TargetElements")]
    [ToolboxData("<{0}:ColorAnimationExtender runat=server></{0}:ColorAnimationExtender>")]
    [ParseChildren(ChildrenAsProperties = true)]
    public class ColorAnimationExtender : Control
    {
        private string baseColor = "#FFFFFF";
        private string targetColor = "#99FF99";
        private List<ColorAnimationTarget> targetElements = new List<ColorAnimationTarget>();
        private const string script = @"$(document).ready(function(){{
	                                        $('{0}').stop().animate({{ {3}: '{1}' }}, {4}, function() {{
		                                        $(this).animate({{ {3}: '{2}' }}, {5});
	                                        }});
                                        }});";
        private bool startAnimation = false;
        private string jQueryScriptIncludeUrl = Constants.JQueryUrl;
        private string targetStyle = "backgroundColor";
        private uint fadeInSpeed = 1000;
        private uint fadeOutSpeed = 2000;

        public string TargetStyle
        {
            get { return targetStyle; }
            set { targetStyle = value; }
        }

        public uint FadeInSpeed
        {
            get { return fadeInSpeed; }
            set { fadeInSpeed = value; }
        }

        public uint FadeOutSpeed
        {
            get { return fadeOutSpeed; }
            set { fadeOutSpeed = value; }
        } 

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ColorAnimationTarget> TargetElements
        {
            get { return this.targetElements; }
            set { this.targetElements = value; }
        }

        public string JQueryScriptIncludeUrl
        {
            get { return jQueryScriptIncludeUrl; }
            set { jQueryScriptIncludeUrl = value; }
        }

        public string BaseColor
        {
            get { return this.baseColor; }
            set { this.baseColor = value; }
        }

        public string TargetColor
        {
            get { return this.targetColor; }
            set { this.targetColor = value; }
        }

        protected override void CreateChildControls()
        {
            if (startAnimation)
            {
                // Register client scripts
                if (!string.IsNullOrEmpty(jQueryScriptIncludeUrl))
                {
                    Page.ClientScript.RegisterClientScriptInclude(GetType(), Constants.JQueryScriptKey, jQueryScriptIncludeUrl.Replace("http", System.Web.HttpContext.Current.Request.Url.Scheme));
                }

                ScriptManager.RegisterClientScriptResource(this, this.GetType(), Constants.JQueryColorPluginResource);

                var selector = string.Join(", ", GetClientIdSelectors(this.NamingContainer).ToArray());
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ColorAnimationScript", string.Format(script, selector, targetColor, baseColor, targetStyle, fadeInSpeed, fadeOutSpeed), true);
                startAnimation = false;
            }
            base.CreateChildControls();
        }

        public static Control FindChildControl(Control parent, string id)
        {
            if (parent == null) return null;

            Control ctrl = parent.FindControl(id);
            if (ctrl == null)
            {
                foreach (Control child in parent.Controls)
                {
                    ctrl = FindChildControl(child, id);
                    if (ctrl != null) break;
                }
            }

            return ctrl;
        }

        private IEnumerable<string> GetClientIdSelectors(Control parent)
        {
            foreach (var element in targetElements)
            {
                var control = FindChildControl(parent, element.ControlID);
                if (control != null)
                {
                    yield return "#" + control.ClientID;
                }
            }
        }

        public void StartAnimation()
        {
            startAnimation = true;
        }


        protected override void OnPreRender(EventArgs e)
        {
            
            base.OnPreRender(e);
        }
    }
}
