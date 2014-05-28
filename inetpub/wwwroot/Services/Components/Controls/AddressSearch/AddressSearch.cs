using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CKG.Components.Controls
{
    public class AddressSearch : CompositeControl
    {
        private static readonly object EventAddressSelected = new object();

        private static readonly DataControlFieldCollection DefaultResultColumns = new DataControlFieldCollection()
        {
            new TemplateField() { HeaderTemplate = new DefaultHeaderTemplate("lbl_Name"), ItemTemplate = new DefaultItemTemplate("DataItem.NAME1") },
            new TemplateField() { HeaderTemplate = new DefaultHeaderTemplate("lbl_Name2"), ItemTemplate = new DefaultItemTemplate("DataItem.NAME2") },
            new TemplateField() { HeaderTemplate = new DefaultHeaderTemplate("lbl_PLZ"), ItemTemplate = new DefaultItemTemplate("DataItem.PSTLZ") },
            new TemplateField() { HeaderTemplate = new DefaultHeaderTemplate("lbl_Strasse"), ItemTemplate = new DefaultItemTemplate("DataItem.STRAS") },
            new TemplateField() { HeaderTemplate = new DefaultHeaderTemplate("lbl_Ort"), ItemTemplate = new DefaultItemTemplate("DataItem.ORT01") },
        };

        static AddressSearch()
        {
            foreach (TemplateField tf in DefaultResultColumns)
            {
                tf.ItemStyle.Width = new Unit(170, UnitType.Pixel);
            }

            DefaultResultColumns[2].ItemStyle.Width = new Unit(40, UnitType.Pixel);
        }

        private class DefaultHeaderTemplate : ITemplate
        {
            private readonly string id;

            public DefaultHeaderTemplate(string id)
            {
                this.id = id;
            }

            public void InstantiateIn(Control container)
            {
                container.Controls.Add(new Label()
                            {
                                ID = this.id,
                                Text = this.id,
                            });
            }
        }

        private class DefaultItemTemplate : ITemplate
        {
            private readonly string fieldName;

            public DefaultItemTemplate(string fieldName)
            {
                this.fieldName = fieldName;
            }

            public void InstantiateIn(Control container)
            {
                var label = new Label();
                label.DataBinding += this.OnDataBinding;
                container.Controls.Add(label);
            }

            private void OnDataBinding(object sender, EventArgs e)
            {
                var label = (Label)sender;
                var container = (IDataItemContainer)label.BindingContainer;
                label.Text = Convert.ToString(DataBinder.Eval(container, this.fieldName), System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        private class ContentTemplate : ITemplate
        {
            private readonly AddressSearch owner;

            public ContentTemplate(AddressSearch owner)
            {
                this.owner = owner;
            }

            public void InstantiateIn(Control container)
            {
                var panel = new Panel()
                            {
                                ID = "pCriteria",
                                DefaultButton = "lbSearch",
                            };

                var table = new System.Web.UI.HtmlControls.HtmlTable()
                            {
                                CellPadding = 0,
                                CellSpacing = 0,
                            };

                var row = new System.Web.UI.HtmlControls.HtmlTableRow();
                var cell = new System.Web.UI.HtmlControls.HtmlTableCell();
                cell.Style.Add(HtmlTextWriterStyle.Width, "70px");

                var label = new Label()
                            {
                                ID = "lbl_Name",
                                AssociatedControlID = "tbName",
                                Text = "lbl_Name",
                            };
                cell.Controls.Add(label);
                row.Controls.Add(cell);

                cell = new System.Web.UI.HtmlControls.HtmlTableCell();
                this.owner.Name = new TextBox()
                                  {
                                      ID = "tbName",
                                      CssClass = "long",
                                  };
                cell.Controls.Add(this.owner.Name);
                row.Controls.Add(cell);
                table.Controls.Add(row);

                row = new System.Web.UI.HtmlControls.HtmlTableRow();
                cell = new System.Web.UI.HtmlControls.HtmlTableCell();
                label = new Label()
                        {
                            ID = "lbl_Plz",
                            AssociatedControlID = "tbPlz",
                            Text = "lbl_Plz",
                        };
                cell.Controls.Add(label);
                cell.Controls.Add(new LiteralControl(", "));
                label = new Label()
                        {
                            ID = "lbl_Ort",
                            AssociatedControlID = "tbOrt",
                            Text = "lbl_Ort",
                        };
                cell.Controls.Add(label);
                row.Controls.Add(cell);

                cell = new System.Web.UI.HtmlControls.HtmlTableCell();
                this.owner.Postcode = new TextBox()
                                      {
                                          ID = "tbPlz",
                                          CssClass = "short",
                                      };
                cell.Controls.Add(this.owner.Postcode);
                cell.Controls.Add(new LiteralControl("\n "));

                this.owner.Town = new TextBox()
                                  {
                                      ID = "tbOrt",
                                      CssClass = "middle",
                                  };
                cell.Controls.Add(this.owner.Town);
                cell.Controls.Add(new LiteralControl("\n "));

                this.owner.SearchButton = new LinkButton()
                                          {
                                              ID = "lbSearch",
                                              Text = "Suchen",
                                              CssClass = "blueButton search",
                                          };
                cell.Controls.Add(this.owner.SearchButton);
                row.Controls.Add(cell);
                table.Controls.Add(row);
                panel.Controls.Add(table);
                container.Controls.Add(panel);

                this.owner.ResultGrid = new GridView()
                                        {
                                            ID = "gvResults",
                                            AutoGenerateColumns = false,
                                            EnableModelValidation = true,
                                            ShowHeader = true,
                                            Width = new Unit(100, UnitType.Percentage),
                                            CssClass = "SearchResultTable",
                                            BorderWidth = new Unit(0),
                                        };

                this.owner.ResultGrid.Columns.Clear();

                foreach (DataControlField field in this.owner.ResultColumns)
                {
                    this.owner.ResultGrid.Columns.Add(field);
                }

                var selectButton = new ButtonField()
                                   {
                                       CommandName = "Select",
                                       ImageUrl = "~/Images/Zulassung/icon_select.gif",
                                       ButtonType = ButtonType.Image,
                                       Text = "Auswählen",
                                   };
                this.owner.ResultGrid.Columns.Add(selectButton);
                container.Controls.Add(this.owner.ResultGrid);
            }
        }

        private class DefaultOverlayTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                var div = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                div.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#fff");
                div.Style.Add(HtmlTextWriterStyle.Width, "300px");
                div.Style.Add(HtmlTextWriterStyle.Padding, "15px");
                div.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                div.Style.Add(HtmlTextWriterStyle.BorderWidth, "3px");
                div.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                div.Style.Add(HtmlTextWriterStyle.BorderColor, "#335393");

                var img = new System.Web.UI.HtmlControls.HtmlImage()
                              {
                                  Src = "~/Images/Zulassung/loading.gif",
                                  Alt = "&hellip;Suche&hellip;"
                              };
                img.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                img.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                div.Controls.Add(img);

                var literal = new LiteralControl()
                              {
                                  Text = @"<br /><label style=""font-size: 14px; font-weight: bold;"">Bitte warten...</label>
                                           <br /><label style=""font-size: 10px; font-weight: bold;"">Ihr Suchanfrage wird bearbeitet.</label>"
                              };
                div.Controls.Add(literal);
                container.Controls.Add(div);
            }
        }

        private LinkButton searchButton;
        private GridView resultGrid;

        private System.Collections.Generic.Dictionary<int, bool> visibility;

        protected override HtmlTextWriterTag TagKey { get { return HtmlTextWriterTag.Div; } }
        protected UpdatePanel Container { get; private set; }
        protected ModalOverlay SearchOverlay { get; private set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate OverlayTemplate { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataControlFieldCollection ResultColumns { get; set; }

        public event EventHandler AddressSelected
        {
            add
            {
                this.Events.AddHandler(AddressSearch.EventAddressSelected, value);
            }
            remove
            {
                this.Events.RemoveHandler(AddressSearch.EventAddressSelected, value);
            }
        }

        private TextBox Name { get; set; }
        private TextBox Postcode { get; set; }
        private TextBox Town { get; set; }
        private LinkButton SearchButton
        {
            get
            {
                return this.searchButton;
            }

            set
            {
                if (this.searchButton != null)
                {
                    this.searchButton.Click -= this.OnSearchClick;
                }

                this.searchButton = value;

                if (this.searchButton != null)
                {
                    this.searchButton.Click += this.OnSearchClick;
                }
            }
        }
        private GridView ResultGrid
        {
            get
            {
                return this.resultGrid;
            }

            set
            {
                if (this.resultGrid != null)
                {
                    this.resultGrid.SelectedIndexChanging -= this.OnResultChanging;
                    this.resultGrid.RowDataBound -= this.OnResultRowBound;
                    this.resultGrid.DataBound -= this.OnResultBound;
                }

                this.resultGrid = value;

                if (this.resultGrid != null)
                {
                    this.resultGrid.SelectedIndexChanging += this.OnResultChanging;
                    this.resultGrid.RowDataBound += this.OnResultRowBound;
                    this.resultGrid.DataBound += this.OnResultBound;
                }
            }
        }

        private string AddressType { get; set; }

        public bool Search(string addressType, string name, string postcode, string town, out int count, int maxCount)
        {
            count = 0;

            const string searchWildcard = "*";

            if (!name.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                name += searchWildcard;
            }

            if (!postcode.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                postcode += searchWildcard;
            }

            if (!town.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                town += searchWildcard;
            }

            this.AddressType = addressType;
            this.Name.Text = name;
            this.Postcode.Text = postcode;
            this.Town.Text = town;
            this.Container.Update();

            var addresses = this.DoSearch(addressType, name, postcode, town);

            if (addresses != null)
            {
                count = addresses.Rows.Count;

                if (count == 1)
                {
                    var addressRow = addresses.Rows[0];
                    this.ExtractData(addressRow);
                    return true;
                }
                else if (count <= maxCount)
                {
                    BindGrid(addresses);
                }
            }
            return false;
        }

        public bool Search(string addressType, string name, string postcode, string town)
        {
         
            const string searchWildcard = "*";

            if (!name.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                name += searchWildcard;
            }

            if (!postcode.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                postcode += searchWildcard;
            }

            if (!town.EndsWith(searchWildcard, StringComparison.Ordinal))
            {
                town += searchWildcard;
            }

            this.AddressType = addressType;
            this.Name.Text = name;
            this.Postcode.Text = postcode;
            this.Town.Text = town;
            this.Container.Update();

            var addresses = this.DoSearch(addressType, name, postcode, town);
            
            if (addresses != null)
            {
 
                if (addresses.Rows.Count == 1)
                {
                    var addressRow = addresses.Rows[0];
                    this.ExtractData(addressRow);
                    return true;
                }
                else
                {
                    BindGrid(addresses);
                }
                    
            }
            return false;
        }

        protected virtual DataTable DoSearch(string addressType, string name, string postcode, string town)
        {
            return null;
        }

        protected virtual void ExtractData(DataRow addressRow)
        {
        }

        protected virtual void OnAddressSelected(object sender, EventArgs e)
        {
            var eh = (EventHandler)this.Events[AddressSearch.EventAddressSelected];

            if (eh != null)
            {
                eh(sender, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }

        protected override void LoadControlState(object savedState)
        {
            var pair = (Pair)savedState;
            base.LoadControlState(pair.First);
            this.AddressType = (string)pair.Second;
        }

        protected override object SaveControlState()
        {
            return new Pair(base.SaveControlState(), this.AddressType);
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            if (this.ResultColumns == null)
            {
                this.ResultColumns = AddressSearch.DefaultResultColumns;
            }

            this.Container = new UpdatePanel()
                                 {
                                     ID = "Container",
                                     UpdateMode = UpdatePanelUpdateMode.Conditional,
                                     ChildrenAsTriggers = true,
                                     ContentTemplate = new ContentTemplate(this),
                                 };

            this.Controls.Add(this.Container);

            this.SearchOverlay = new ModalOverlay()
                                     {
                                         ID = "SearchOverlay",
                                         ParentContainer = this.ID,
                                         Triggers = new System.Collections.Generic.List<ModalOverlayTrigger> { new ModalOverlayTrigger { ControlID = "lbSearch" } },
                                         ContentTemplate = this.OverlayTemplate ?? new DefaultOverlayTemplate(),
                                     };

            this.Controls.Add(this.SearchOverlay);
        }

        private void BindGrid(DataTable addresses)
        {
            //if (addresses != null)
            //{
            //    var gridColumnMapping2 = new System.Collections.Generic.Dictionary<string, int>();
            //    gridColumnMapping2.Add("NAME1", 0);
            //    gridColumnMapping2.Add("NAME2", 1);
            //    gridColumnMapping2.Add("PSTLZ", 2);
            //    gridColumnMapping2.Add("STRAS", 3);
            //    gridColumnMapping2.Add("ORT01", 4);
            //    gridColumnMapping2.Add("POS_Text", 5);

            //    // Hide empty columns.
            //    addresses.Rows.Cast<DataRow>()
            //            .Aggregate(gridColumnMapping2, (m, row) =>
            //            {
            //                foreach (var k in m.Keys)
            //                {
            //                    this.ResultGrid.Columns[m[k]].Visible |= !String.IsNullOrEmpty(row.Field<string>(k));
            //                }

            //                return m;
            //            });
            //}

            this.ResultGrid.DataSource = addresses;
            this.ResultGrid.DataBind();
            this.Container.Update();
        }

        private void OnSearchClick(object sender, EventArgs e)
        {
            var addresses = this.DoSearch(this.AddressType, this.Name.Text, this.Postcode.Text, this.Town.Text);
            BindGrid(addresses);
        }

        private void OnResultRowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.visibility == null)
                {
                    this.visibility = new System.Collections.Generic.Dictionary<int, bool>(e.Row.Cells.Count);
                }

                for (var i = 0; i < e.Row.Cells.Count; i++)
                {
                    var cell = e.Row.Cells[i];

                    if (cell.Controls.Count == 1)
                    {
                        var control = cell.Controls[0];

                        if (control is Label)
                        {
                            var label = (Label)control;

                            if (this.visibility.ContainsKey(i))
                            {
                                this.visibility[i] |= !String.IsNullOrEmpty(label.Text);
                            }
                            else
                            {
                                this.visibility.Add(i, !String.IsNullOrEmpty(label.Text));
                            }
                        }
                        else if (control is ImageButton)
                        {
                            var button = (ImageButton)control;
                            button.OnClientClick = "event.preventDefault?event.preventDefault():event.returnValue=false;";
                        }
                    }
                }
            }
        }

        private void OnResultBound(object sender, EventArgs e)
        {
            if (this.visibility != null)
            {
                foreach (var kvp in this.visibility)
                {
                    this.ResultGrid.Columns[kvp.Key].Visible = kvp.Value;
                }
            }
        }

        private void OnResultChanging(object sender, GridViewSelectEventArgs e)
        {
            var addresses = this.DoSearch(this.AddressType, this.Name.Text, this.Postcode.Text, this.Town.Text);

            if (addresses != null && addresses.Rows.Count > e.NewSelectedIndex)
            {
                var addressRow = addresses.Rows[e.NewSelectedIndex];
                this.ExtractData(addressRow);
                this.OnAddressSelected(this, EventArgs.Empty);
            }
        }
    }
}
