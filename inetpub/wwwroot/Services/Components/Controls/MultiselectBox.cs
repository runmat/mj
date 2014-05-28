using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;

namespace CKG.Components.Controls
{
    [ToolboxData("<{0}:MultiselectBox runat=server></{0}:MultiselectBox>")]
    [ParseChildren(ChildrenAsProperties = true)]
    public class MultiselectBox : DataBoundControl, INamingContainer
    {
        private List<ListItem> items = new List<ListItem>();
        private string hiddenFieldValue;
        private string cssClass = "multiselect";
        private IEnumerable<string> selectedValues;
        private string jQueryScriptIncludeUrl = Constants.JQueryUrl;
        private string jQueryUiScriptIncludeUrl = Constants.JQueryUIUrl;

        private LiteralControl listItemContainer = new LiteralControl();
        private LiteralControl selectedListItemContainer = new LiteralControl();

        private const string script = @"var {0}_multi_items = new Array({4});
                                        $(document).ready(function(){{
                                            $('#{1}').sortable({{ connectWith: '#{2}', cursor: 'move', receive: function(event, ui) {{ 
                                                {0}_Unselect(ui.item.attr('id'));
                                            }}}}).disableSelection();
    
                                            $('#{2}').sortable({{ connectWith: '#{1}', cursor: 'move', receive: function(event, ui) {{ 
                                                {0}_Select(ui.item.attr('id'));
                                            }}}}).disableSelection();
                                        }});

                                        function {0}_Select(index) {{
                                            {0}_multi_items.push(index);
                                            $('#{3}').val({0}_multi_items);
                                        }}

                                        function {0}_Unselect(index) {{
                                            {0}_multi_items = jQuery.grep({0}_multi_items, function(value) {{
                                                return value != index;
                                            }});
                                            $('#{3}').val({0}_multi_items);
                                        }}";

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ListItem> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }

        public IEnumerable<ListItem> SourceItems
        {
            get { return items.Where(itm => !itm.Selected); }
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

        public string DataValueField { get; set; }

        [PersistenceMode(PersistenceMode.Attribute)]
        public FlowDirection FlowDirection { get; set; }

        public string DataTextField { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FooterTemplate { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemsHeaderTemplate { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemsFooterTemplate { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate SelectedItemsHeaderTemplate { get; set; }

        [TemplateContainer(typeof(EmptyContainer))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate SelectedItemsFooterTemplate { get; set; }

        public IEnumerable<string> SelectedValues
        {
            get { return SelectedItems.Select(itm => itm.Value); }
            set
            {
                this.selectedValues = value;
                this.hiddenFieldValue = null;
                this.RestoreSelection();
            }
        }

        public IEnumerable<ListItem> SelectedItems
        {
            get { return items.Where(itm => itm.Selected); }
        }

        private string HiddenFieldValue
        {
            get
            {
                if (String.IsNullOrEmpty(hiddenFieldValue))
                {
                    var selected = SelectedItems.Select(itm => itm.Value).ToArray();
                    var selectedItems = String.Join("','", ((selectedValues != null) ? selectedValues.ToArray() : selected));
                    hiddenFieldValue = String.IsNullOrEmpty(selectedItems) ? string.Empty : string.Format("'{0}'", selectedItems);
                }
                return hiddenFieldValue;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            // Let the page know this control needs the control state.
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);

            RestoreClientState();
        }

        protected override object SaveControlState()
        {
            return HiddenFieldValue;
        }

        protected override void LoadControlState(object savedState)
        {
            var value = savedState as string;
            if (value != null)
            {
                hiddenFieldValue = value;
                RestoreSelection();
            }
            base.LoadControlState(savedState);
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

            InstantiateTemplate(HeaderTemplate);

            if (FlowDirection == FlowDirection.LeftToRight)
            {
                AddSourceList();
                AddSelectionList();
            }
            else
            {
                AddSelectionList();
                AddSourceList();
            }

            InstantiateTemplate(FooterTemplate);

            base.CreateChildControls();
        }

        private void AddSourceList()
        {
            InstantiateTemplate(ItemsHeaderTemplate);
            Controls.Add(listItemContainer);
            InstantiateTemplate(ItemsFooterTemplate);
        }

        private void AddSelectionList()
        {
            InstantiateTemplate(SelectedItemsHeaderTemplate);
            Controls.Add(selectedListItemContainer);
            InstantiateTemplate(SelectedItemsFooterTemplate);
        }

        private void RestoreSelection()
        {
            if (string.IsNullOrEmpty(HiddenFieldValue))
            {
                foreach (var item in Items)
                {
                    item.Selected = false;
                }
            }
            else
            {
                var values = HiddenFieldValue.Split(',').Select(val => val.Trim('\''));

                foreach (var item in Items)
                {
                    if(values.Contains(item.Value)) 
                    {
                        item.Selected = true;
                    }
                    else 
                    {
                        item.Selected = false;
                    }
                }
            }
        }


        private void RestoreClientState()
        {
            // Restore client state
            var value = Page.Request.Form[HiddenFieldClientId];

            if (value != null)
            {
                var items = value.Split(',');

                for (int i = 0; i < items.Length; i++)
                {
                    var itm = items[i];
                    if (!itm.StartsWith("'"))
                    {
                        items[i] = string.Format("'{0}'", itm);
                    }
                }

                this.hiddenFieldValue = string.Join(",", items);
                RestoreSelection();
            }
        }

        protected override void PerformDataBinding(IEnumerable retrievedData)
        {
            retrievedData = retrievedData ?? new object[] { };

            if (retrievedData != null)
            {
                Items = new List<ListItem>();
                foreach (object dataItem in retrievedData)
                {
                    var itm = new ListItem();
                    if (DataTextField.Length > 0)
                    {
                        itm.Text = DataBinder.GetPropertyValue(dataItem, DataTextField, null);
                    }
                    if (DataValueField.Length > 0)
                    {
                        itm.Value = DataBinder.GetPropertyValue(dataItem, DataValueField, null);
                    }
                    Items.Add(itm);
                }
                RestoreSelection();
            }
        }

        private string GetClientId(string id)
        {
            return string.Format("{0}{1}{2}", this.ClientID, this.ClientIDSeparator, id);
        }

        private string SourceItemsClientId
        {
            get
            {
                return GetClientId("SourceItems");
            }
        }

        private string SelectedItemsClientId
        {
            get
            {
                return GetClientId("SelectedItems");
            }
        }

        private string HiddenFieldClientId
        {
            get
            {
                return GetClientId("SelectedValues");
            }
        }

        private void InstantiateTemplate(ITemplate template)
        {
            if (template != null)
            {
                var container = new EmptyContainer();
                template.InstantiateIn(container);
                Controls.Add(container);
                container.DataBind();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            listItemContainer.Text = GetListItemsSource();
            selectedListItemContainer.Text = GetSelectedListItemsSource();
            base.OnPreRender(e);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "MultiliselectBoxScript", string.Format(script, ClientID, SourceItemsClientId, SelectedItemsClientId, HiddenFieldClientId, HiddenFieldValue), true);
            ScriptManager.RegisterHiddenField(this, HiddenFieldClientId, HiddenFieldValue);
        }

        private string GetListItemsSource()
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, SourceItemsClientId);
                if (!string.IsNullOrEmpty(cssClass))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var item in SourceItems)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, item.Value);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.WriteEncodedText(item.Text);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }


        private string GetSelectedListItemsSource()
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, SelectedItemsClientId);
                if (!string.IsNullOrEmpty(cssClass))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var item in SelectedItems)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, item.Value);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.WriteEncodedText(item.Text);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }

    }
}
