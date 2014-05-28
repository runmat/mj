namespace CKG.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;

    [ParseChildren(ChildrenAsProperties = true)]
    public class DocumentList : DataBoundControl, INamingContainer
    {
        private static readonly object EventDocumentListCommand = new object();
        private static readonly object EventDocumentListItemCommand = new object();
        private static readonly object EventLayoutCreated = new object();

        private int _groupsOriginalIndexOfGroupPlaceholderInContainer = -1;
        private Control _groupsGroupPlaceholderContainer;
        private int _groupsItemCreatedCount;
        private int _autoIDIndex;
        private System.Collections.ArrayList _groupCounts = new System.Collections.ArrayList();

        public string SelectCountry
        {
            get { return (string)this.ViewState["SelectCountry"]; }
            set { this.ViewState["SelectCountry"] = value; }
        }

        [Description("ItemPlaceholderID"), DefaultValue("itemPlaceholder"), Category("Behavior")]
        public virtual string ItemPlaceholderID
        {
            get
            {
                object obj = this.ViewState["ItemPlaceholderID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "itemPlaceholder";
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.ViewState["ItemPlaceholderID"] = value;
            }
        }

        [Description("GroupPlaceholderID"), DefaultValue("groupPlaceholder"), Category("Behavior")]
        public virtual string GroupPlaceholderID
        {
            get
            {
                object obj = this.ViewState["GroupPlaceholderID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "groupPlaceholder";
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException();
                }

                this.ViewState["GroupPlaceholderID"] = value;
            }
        }

        [TemplateContainer(typeof(DocumentListLayout))]
        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate LayoutTemplate { get; set; }

        [TemplateContainer(typeof(DocumentListGroup))]
        [TemplateInstance(TemplateInstance.Multiple)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GroupTemplate { get; set; }

        [TemplateContainer(typeof(DocumentListDataItem))]
        [TemplateInstance(TemplateInstance.Multiple)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemTemplate { get; set; }

        [TemplateContainer(typeof(DocumentListLayout))]
        [TemplateInstance(TemplateInstance.Multiple)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemSeparatorTemplate { get; set; }

        [TemplateContainer(typeof(DocumentListLayout))]
        [TemplateInstance(TemplateInstance.Multiple)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate EmptyDataTemplate { get; set; }

        public IList<DocumentListDataItem> Items { get; private set; }

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
            var obj = this.ViewState["_!ItemCount"];
            if (obj == null && this.RequiresDataBinding)
            {
                this.EnsureDataBound();
            }

            if (obj != null && (int)obj != -1)
            {
                var dataSource = new object[(int)obj];
                this.CreateChildControls(dataSource, false);
                base.ClearChildViewState();
            }
        }

        protected override void LoadControlState(object savedState)
        {
            if (savedState is object[])
            {
                var savedState2 = (object[])savedState;
                base.LoadControlState(savedState2[0]);
                this._groupCounts = (System.Collections.ArrayList)savedState2[1];
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }

        protected override object SaveControlState()
        {
            if (this._groupCounts.Count > 0)
            {
                return new object[] {
                    base.SaveControlState(),
                    this._groupCounts};
            }
            else
            {
                return base.SaveControlState();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Page != null)
            {
                this.Page.RegisterRequiresControlState(this);
            }
        }

        private int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int totalRowCount = 0;
            this.EnsureLayout(dataBinding);

            if (dataSource != null)
            {
                this.Items = this.CreateItemsInGroups(dataSource.Cast<IDocumentGroup>(), dataBinding);
                totalRowCount = this._groupsItemCreatedCount;

                if (this.Items.Count == 0)
                {
                    this.Controls.Clear();
                    this.InstantiateEmptyDataTemplate(dataBinding);
                }
            }
            else
            {
                this.Controls.Clear();
                this.InstantiateEmptyDataTemplate(dataBinding);
            }
            return totalRowCount;
        }

        protected virtual void AddControlToContainer(Control control, Control container, int addLocation)
        {
            if (container is System.Web.UI.HtmlControls.HtmlTable)
            {
                var listViewTableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                container.Controls.AddAt(addLocation, listViewTableRow);
                listViewTableRow.Controls.Add(control);
                return;
            }
            if (container is System.Web.UI.HtmlControls.HtmlTableRow)
            {
                var listViewTableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                container.Controls.AddAt(addLocation, listViewTableCell);
                listViewTableCell.Controls.Add(control);
                return;
            }
            container.Controls.AddAt(addLocation, control);
        }

        private Control GetPreparedContainerInfo(Control outerContainer, bool isItem, out int placeholderIndex)
        {
            string containerID = isItem ? this.ItemPlaceholderID : this.GroupPlaceholderID;
            Control control = outerContainer.FindControl(containerID);

            if (control != null)
            {
                Control parent = control.Parent;
                placeholderIndex = parent.Controls.IndexOf(control);
                parent.Controls.Remove(control);
                return parent;
            }

            throw new InvalidOperationException();
        }

        private void AutoIDControl(Control control)
        {
            control.ID = "ctrl" + this._autoIDIndex++.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private IList<DocumentListDataItem> CreateItemsInGroups(IEnumerable<IDocumentGroup> dataSource, bool dataBinding)
        {
            if (this._groupsOriginalIndexOfGroupPlaceholderInContainer == -1)
            {
                this._groupsGroupPlaceholderContainer = this.GetPreparedContainerInfo(this.Controls[0], false, out this._groupsOriginalIndexOfGroupPlaceholderInContainer);
            }

            int num = this._groupsOriginalIndexOfGroupPlaceholderInContainer;
            this._groupsItemCreatedCount = 0;
            int num2 = 0;
            Control container = null;

            var list = new List<DocumentListDataItem>();
            int num4 = 0;

            foreach (IDocumentGroup currentGroup in dataSource)
            {
                var listViewContainer3 = new DocumentListGroup(num - this._groupsOriginalIndexOfGroupPlaceholderInContainer);

                if (dataBinding)
                {
                    listViewContainer3.SelectedCountry = this.SelectCountry;
                    listViewContainer3.GroupName = currentGroup.GroupName;
                    listViewContainer3.GroupCount = currentGroup.Documents.Count;
                }

                this.AutoIDControl(listViewContainer3);
                this.InstantiateGroupTemplate(listViewContainer3);
                this.AddControlToContainer(listViewContainer3, this._groupsGroupPlaceholderContainer, num);

                if (dataBinding)
                {
                    listViewContainer3.DataBind();
                }

                container = this.GetPreparedContainerInfo(listViewContainer3, true, out num2);

                int count = 0;

                if (dataBinding)
                {
                    count = currentGroup.Documents.Count;
                    this._groupCounts.Insert(num - this._groupsOriginalIndexOfGroupPlaceholderInContainer, count);
                }
                else
                {
                    count = (int)this._groupCounts[num - this._groupsOriginalIndexOfGroupPlaceholderInContainer];
                }

                num++;

                for (var i = 0; i < count; i++)
                {
                    var dataItem = new DocumentListDataItem(num4);
                    this.InstantiateItemTemplate(dataItem);

                    if (dataBinding)
                    {
                        dataItem.Document = currentGroup.Documents[i];
                    }

                    if (i > 0 && this.ItemSeparatorTemplate != null)
                    {
                        var listViewContainer4 = new DocumentListLayout();
                        this.InstantiateItemSeparatorTemplate(listViewContainer4);
                        this.AddControlToContainer(listViewContainer4, container, num2);
                        num2++;
                    }

                    this.AddControlToContainer(dataItem, container, num2);
                    num2++;
                    list.Add(dataItem);

                    if (dataBinding)
                    {
                        dataItem.DataBind();
                        dataItem.Document = null;
                    }

                    num4++;
                }
            }

            this._groupsItemCreatedCount = num - this._groupsOriginalIndexOfGroupPlaceholderInContainer;
            return list;
        }

        private void EnsureLayout(bool databinding)
        {
            this.Controls.Clear();
            this.InstantiateLayout(databinding);
        }

        private void InstantiateLayout(bool databinding)
        {
            this._groupsOriginalIndexOfGroupPlaceholderInContainer = -1;
            this._groupsItemCreatedCount = 0;
            this._groupsGroupPlaceholderContainer = null;
            this._autoIDIndex = 0;

            if (this.LayoutTemplate != null)
            {
                var container = new DocumentListLayout() { SelectedCountry = this.SelectCountry };
                this.LayoutTemplate.InstantiateIn(container);
                this.Controls.Add(container);

                if (databinding)
                {
                    container.DataBind();
                }

                this.RaiseLayoutCreated(new DocumentListLayoutEventArgs() { Layout = container });
            }
        }

        private void InstantiateGroupTemplate(Control container)
        {
            if (this.GroupTemplate != null)
            {
                this.GroupTemplate.InstantiateIn(container);
            }
        }

        private void InstantiateItemTemplate(Control container)
        {
            if (this.ItemTemplate != null)
            {
                this.ItemTemplate.InstantiateIn(container);
            }
        }

        private void InstantiateItemSeparatorTemplate(Control container)
        {
            if (this.ItemSeparatorTemplate != null)
            {
                this.ItemSeparatorTemplate.InstantiateIn(container);
            }
        }

        private void InstantiateEmptyDataTemplate(bool databinding)
        {
            if (this.EmptyDataTemplate != null)
            {
                var listViewItem = new DocumentListLayout() { SelectedCountry = this.SelectCountry };
                this.AutoIDControl(listViewItem);
                this.EmptyDataTemplate.InstantiateIn(listViewItem);
                this.AddControlToContainer(listViewItem, this, 0);

                if (databinding)
                {
                    listViewItem.DataBind();
                }
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is DocumentListItemCommandEventArgs)
            {
                this.RaiseDocumentListItemCommand((DocumentListItemCommandEventArgs)args);
            }
            else if (args is CommandEventArgs)
            {
                this.RaiseDocumentListCommand((CommandEventArgs)args);
                return true;
            }
            return base.OnBubbleEvent(source, args);
        }

        public event CommandEventHandler DocumentListCommand
        {
            add { this.Events.AddHandler(DocumentList.EventDocumentListCommand, value); }
            remove { this.Events.RemoveHandler(DocumentList.EventDocumentListCommand, value); }
        }

        private void RaiseDocumentListCommand(CommandEventArgs e)
        {
            var eh = (CommandEventHandler)this.Events[DocumentList.EventDocumentListCommand];

            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler<DocumentListItemCommandEventArgs> DocumentListItemCommand
        {
            add { this.Events.AddHandler(DocumentList.EventDocumentListItemCommand, value); }
            remove { this.Events.RemoveHandler(DocumentList.EventDocumentListItemCommand, value); }
        }

        private void RaiseDocumentListItemCommand(DocumentListItemCommandEventArgs e)
        {
            var eh = (EventHandler<DocumentListItemCommandEventArgs>)this.Events[DocumentList.EventDocumentListItemCommand];

            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler<DocumentListLayoutEventArgs> LayoutCreated
        {
            add { this.Events.AddHandler(DocumentList.EventLayoutCreated, value); }
            remove { this.Events.RemoveHandler(DocumentList.EventLayoutCreated, value); }
        }

        private void RaiseLayoutCreated(DocumentListLayoutEventArgs e)
        {
            var eh = (EventHandler<DocumentListLayoutEventArgs>)this.Events[DocumentList.EventLayoutCreated];

            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected override void PerformDataBinding(System.Collections.IEnumerable data)
        {
            base.PerformDataBinding(data);
            this.TrackViewState();

            this._groupCounts.Clear();
            var num = this.CreateChildControls(data, true);
            this.ChildControlsCreated = true;

            this.ViewState["_!ItemCount"] = num;
        }
    }
}
