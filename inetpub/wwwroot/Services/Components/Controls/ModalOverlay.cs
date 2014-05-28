using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Globalization;
using CKG.Components.Controls;

[assembly: WebResource(Constants.JQueryCenterPluginResource, "text/javascript")] 
namespace CKG.Components.Controls
{
    public enum ModalOverlayType 
    {
        PostBack,
        Click
    }

    [DefaultProperty("ParentContainer")]
    [ToolboxData("<{0}:ModalOverlay runat=server></{0}:ModalOverlay>")]
    [ParseChildren(ChildrenAsProperties = true)]
    public class ModalOverlay : Control, INamingContainer
    {
        private ModalOverlayType type = ModalOverlayType.PostBack;
        private List<ModalOverlayTrigger> triggers = new List<ModalOverlayTrigger>();
        private string parentContainer;
        private string parentContainerClientId;
        private string backgroundColor = "#000";
        private uint zIndex = 100;
        private uint fadeInSpeed = 500;
        private ITemplate contentTemplate;
        private double opacity = 0.35d;
        private string jQueryScriptIncludeUrl = Constants.JQueryUrl;
        private string jQueryUiScriptIncludeUrl = Constants.JQueryUIUrl;

        private const string clickScript = @"function {0}_close() {{
                                                var overlayId = '{0}';
                                                $(""#"" + overlayId + ""_backgroundlayer"").fadeOut({5}); 
                                                $(""#"" + overlayId).fadeOut({5}); 
                                            }};

                                            $(document).ready(function(){{   
                                                 {0}_init();
                                                 Sys.WebForms.PageRequestManager.getInstance().add_endRequest({0}_EndRequestHandler);
                                            }});

                                            $(document).keydown(function(e) {{
                                                var element = e.target.nodeName.toLowerCase();
                                                if ((e.keyCode == 27 || (!$(e.target).is('textarea') && !(e.target.type == ""text"" || e.target.type == ""password"") && e.keyCode == 8)) && $(""#{0}"").is(':visible')) {{
                                                    {0}_close();
                                                    return false;
                                                }}
                                            }});

                                            function {0}_EndRequestHandler(sender, args) {{
                                                {0}_init();
                                            }}

                                            function {0}_show() {{
                                                var overlayId = '{0}';
                                                $(""#"" + overlayId).center({1});
                                                $(""#"" + overlayId + ""_backgroundlayer"").fadeTo({5}, {4:0.00}); 
                                                $(""#"" + overlayId).fadeIn({5}); 
                                            }}

                                            function {0}_showInstant() {{
                                                var overlayId = '{0}';
                                                $(""#"" + overlayId).center({1});
                                                $(""#"" + overlayId + ""_backgroundlayer"").fadeTo(0, {4:0.00}); 
                                                $(""#"" + overlayId).fadeIn(0); 
                                            }}

                                            function {0}_init() {{
                                                 var triggers = new Array('{2}');
                                                 for(var i=0; i<triggers.length; i++)
                                                 {{
                                                    var id = triggers[i];
                                                    if(id.length > 0) {{
                                                        $('#' + id).click(function() {{  
                                                            {0}_show(); 
                                                            return false;
                                                        }});
                                                    }}
                                                 }}
                                                 $(""#{0}"").draggable({{ handle:'.header'}});
                                            }}";

        private const string postbackScript = @"function {0}_BeginRequestHandler(sender, args) {{
                                            var elem = args.get_postBackElement();
                                            var triggers = new Array('{2}');
                                            if($.inArray(elem.id, triggers) > -1 || (triggers[0] == '' && elem.id.indexOf('{3}') == 0)) {{
                                                {0}_show(); 
                                            }}
                                        }}

                                        function {0}_show() {{
                                            var overlayId = '{0}';
                                            $(""#"" + overlayId).center({1});
                                            $(""#"" + overlayId + ""_backgroundlayer"").fadeTo({5}, {4:0.00}); 
                                            $(""#"" + overlayId).fadeIn({5}); 
                                        }}

                                        function {0}_showInstant() {{
                                            var overlayId = '{0}';
                                            $(""#"" + overlayId).center({1});
                                            $(""#"" + overlayId + ""_backgroundlayer"").fadeTo(0, {4:0.00}); 
                                            $(""#"" + overlayId).fadeIn(0); 
                                        }}

                                        function {0}_close() {{
                                            var overlayId = '{0}';
                                            $(""#"" + overlayId + ""_backgroundlayer"").fadeOut({5}); 
                                            $(""#"" + overlayId).fadeOut({5}); 
                                        }};

                                        function {0}_EndRequestHandler(sender, args) {{
                                            {0}_close();
                                        }}

                                        $(document).ready(function(){{    
                                            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest({0}_BeginRequestHandler);
                                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest({0}_EndRequestHandler);
                                        }});";

        private bool _pagePreLoadFired;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ModalOverlayTrigger> Triggers
        {
            get { return this.triggers; }
            set { this.triggers = value; }
        }

        [TemplateContainer(typeof(OverlayContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        public string ShowOverlayClientScript
        {
            get
            {
                return string.Concat(this.ClientID, "_show()");
            }
        }

        public string ShowOverlayInstantClientScript
        {
            get
            {
                return string.Concat(this.ClientID, "_showInstant()");
            }
        }

        public string HideOverlayClientScript
        {
            get
            {
                return string.Concat(this.ClientID, "_close()");
            }
        }

        public ModalOverlayType Type
        {
            get { return type; }
            set { type = value; }
        }

        public OverlayContainer ContentTemplateContainer
        {
            get
            {
                this.EnsureChildControls();
                return (OverlayContainer)this.Controls[0];
            }
        }

        public double Opacity
        {
            get { return opacity; }
            set 
            {
                if (value > 1 || value < 0.01)
                {
                    throw new ArgumentOutOfRangeException("value", "only values between 0.01 and 1.00 are allowed.");
                }
                opacity = value; 
            }
        }

        public uint ZIndex
        {
            get { return zIndex; }
            set 
            {
                zIndex = value; 
            }
        }

        public string BackgroundColor
        {
            get { return backgroundColor; }
            set 
            {
                if (string.IsNullOrEmpty(backgroundColor))
                {
                    throw new ArgumentNullException("value", "value cannot be empty");
                }
                backgroundColor = value; 
            }
        }

        public string ParentContainer
        {
            get 
            { 
                return parentContainer; 
            }
            set { parentContainer = value; }
        }

        public string ParentContainerClientId
        {
            get 
            {
                if (string.IsNullOrEmpty(parentContainer))
                {
                    return string.Empty;
                }

                if(string.IsNullOrEmpty(parentContainerClientId)) 
                { 
                    return GetParentContainerClientId(); 
                }
                return parentContainerClientId; 
            }
            set { parentContainerClientId = value; }
        }

        private string GetParentContainerClientId()
        {
            if (!string.IsNullOrEmpty(parentContainer))
            {
                Control parent;

                if (NamingContainer.ID.Equals(parentContainer))
                {
                    parent = NamingContainer;
                }
                else
                {
                    parent = FindChildControl(NamingContainer, parentContainer);
                }

                if (parent == null)
                {
                    throw new ArgumentException(string.Format("Cannot find a parent with ID '{0}'.", parentContainer));
                }

                return parent.ClientID;
            }
            else
            {
                return Parent.ClientID;
            }
        }

        private string GetParentNamingContainerClientId()
        {
            if (!string.IsNullOrEmpty(parentContainer))
            {
                Control parent;

                if (NamingContainer.ID.Equals(parentContainer))
                {
                    parent = NamingContainer;
                }
                else
                {
                    parent = FindChildControl(NamingContainer, parentContainer);
                }

                if (parent == null)
                {
                    throw new ArgumentException(string.Format("Cannot find a parent with ID '{0}'.", parentContainer));
                }

                return parent.NamingContainer.ClientID;
            }
            else
            {
                return Parent.NamingContainer.ClientID;
            }
        }

        public string JQueryScriptIncludeUrl
        {
            get { return jQueryScriptIncludeUrl; }
            set { jQueryScriptIncludeUrl = value; }
        }

        public string JQueryUiScriptIncludeUrl
        {
            get { return jQueryUiScriptIncludeUrl; }
            set { jQueryUiScriptIncludeUrl = value; }
        }

        public IEnumerable<string> GetTriggerClientIDs()
        {
            foreach (var trigger in Triggers)
            {
                var control = FindChildControl(Parent, trigger.ControlID);
                if (control == null)
                {
                    throw new ArgumentException(string.Format("Cannot find a trigger with ID '{0}'.", trigger.ControlID));
                }
                yield return control.ClientID;
            }
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

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        protected override void CreateChildControls()
        {
            // Register client scripts
            if (!string.IsNullOrEmpty(jQueryScriptIncludeUrl))
            {
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), Constants.JQueryScriptKey, jQueryScriptIncludeUrl.Replace("http", System.Web.HttpContext.Current.Request.Url.Scheme));
            }
            if (!string.IsNullOrEmpty(jQueryUiScriptIncludeUrl))
            {
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), Constants.JQueryUIScriptKey, jQueryUiScriptIncludeUrl.Replace("http", System.Web.HttpContext.Current.Request.Url.Scheme));
            }

            Page.ClientScript.RegisterClientScriptResource(this.GetType(), Constants.JQueryCenterPluginResource);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), ClientID + "PopupScript", string.Format(CultureInfo.InvariantCulture, (Type == ModalOverlayType.PostBack) ? postbackScript : clickScript, this.ClientID, ((string.IsNullOrEmpty(ParentContainerClientId)) ? "{ against: 'window' }" : string.Format("{{ against: '#{0}' }}", ParentContainerClientId)), (triggers.Count > 0) ? string.Join("','", GetTriggerClientIDs().ToArray()) : string.Empty, GetParentNamingContainerClientId(), this.opacity, this.fadeInSpeed), true);

            if (ContentTemplate != null)
            {
                var container = new OverlayContainer();
                container.CloseScript = this.ClientID + "_close()";
                ContentTemplate.InstantiateIn(container);
                Controls.Add(container);

                if (this.RequiresDataBinding)
                {
                    container.DataBind();
                    this.ViewState["_!DataBound"] = true;
                }
            }

            base.CreateChildControls();
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_backgroundlayer");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "fixed");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Top, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Left, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, zIndex + " !important");

            if (string.IsNullOrEmpty(backgroundColor))
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#fff");
            }
            else
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, backgroundColor);
            }
            
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, (zIndex + 1) + " !important");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderChildren(writer);
            writer.RenderEndTag();
        }

        public bool RequiresDataBinding { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Page != null)
            {
                this.Page.PreLoad +=this.OnPagePreLoad;
                if (!base.IsViewStateEnabled && this.Page.IsPostBack)
                {
                    this.RequiresDataBinding = true;
                }
            }

        }

        protected virtual void OnPagePreLoad(object sender, EventArgs e)
        {
            if (this.Page != null)
            {
                this.Page.PreLoad -= this.OnPagePreLoad;

                if (!this.Page.IsPostBack)
                {
                    this.RequiresDataBinding = true;
                }

                if (this.Page.IsPostBack && base.IsViewStateEnabled && this.ViewState["_!DataBound"] == null)
                {
                    this.RequiresDataBinding = true;
                }

                this._pagePreLoadFired = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Page != null && !this._pagePreLoadFired && this.ViewState["_!DataBound"] == null)
            {
                if (!this.Page.IsPostBack)
                {
                    this.RequiresDataBinding = true;
                }
                else
                {
                    if (base.IsViewStateEnabled)
                    {
                        this.RequiresDataBinding = true;
                    }
                }
            }
            base.OnLoad(e);
        }
    }
}
