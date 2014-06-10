using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Kantine
{
	public partial class GridNavigation : System.Web.UI.UserControl
	{
		protected GridView mGridView;
		protected DataGrid mDataGrid;

        public delegate void PageSize_ChangedEventHandler();
        public delegate void Pager_ChangedEventHandler(object sender, int pageindex);

        public event PageSize_ChangedEventHandler PageSizeChanged;
        public event Pager_ChangedEventHandler PagerChanged;

		private int PageCount;
		private int PagerStart;


        protected GridNavigation()
        {
             
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (mDataGrid != null)
            {
                mDataGrid.ItemDataBound += new DataGridItemEventHandler(DataGrid_DataBinding);               
            }
            else if (mGridView != null)
            {
                mGridView.DataBound += new EventHandler(GridView_DataBinding);
            }
			if (!IsPostBack)
			{
			    fillPageSizeDropDown();
                ProofPager();
			}
		}

        protected void GridView_DataBinding(object sender,  EventArgs e)
		{
			ProofPager(); 
		}

        protected void DataGrid_DataBinding(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemIndex == -1)
            {
                ProofPager();
            }
		}

		public void setGridElement(ref GridView GridElement)
		{
			mGridView = GridElement;			
		}

		public void setGridElement(ref DataGrid GridElement)
		{
			mDataGrid = GridElement;			
		}

		public void setGridTitle(String Title)
		{
			lbltitle.Text = Title;
			if (Title.Trim().Length > 0)
			{
				lbltitle.Visible = true;
			}
		}

		private void fillPageSizeDropDown()
		{
			ddlPageSize.Items.Add("10");
			ddlPageSize.Items.Add("20");
			ddlPageSize.Items.Add("50");
			ddlPageSize.Items.Add("100");
			ddlPageSize.Items.Add("200");
			ddlPageSize.Items.Add("500");
			ddlPageSize.SelectedIndex = 1;
		}

		public void ProofPager()
		{
			if (mGridView != null)
			{
				if (mGridView.PageCount <= 1)
				{
					lbtnNext.Visible = false;
					lbtnPrevious.Visible = false;
					lbtnPrevious10.Visible = false;
					lbtnNext10.Visible = false;
					Repeater1.Visible = false;
				}
				else
				{
					if (mGridView.PageIndex < mGridView.PageCount - 1)
					{
						lbtnNext.Visible = true;

						if (mGridView.PageCount > 9)
						{
							PageCount = 9;

							if (mGridView.PageIndex < mGridView.PageCount - 10)	//-1 für 0 basierten Index, -9 für Next10
							{
								string strPageIndex = mGridView.PageIndex.ToString();

								string sPage = strPageIndex.Substring(0, 1);
								sPage += "0";
								PagerStart = Convert.ToInt16(sPage);

								lbtnNext10.Visible = true;
							}
						}
						else
						{
							PageCount = mGridView.PageCount - 1;

							lbtnNext10.Visible = false;
							lbtnPrevious10.Visible = false;
						}
					}
					else
					{
						lbtnNext.Visible = false;
					}

					if (mGridView.PageIndex > 0)
					{
						lbtnPrevious.Visible = true;

						if (mGridView.PageCount > 9)
						{
							PageCount = 9;

							if (mGridView.PageIndex > 9)
							{
								string strPageIndex = mGridView.PageIndex.ToString();

								string sPage = strPageIndex.Substring(0, 1);
								sPage += "0";
								PagerStart = Convert.ToInt16(sPage);

								lbtnPrevious10.Visible = true;
							}
							else
							{
								PagerStart = 0;
							}
						}
						else
						{
							PageCount = mGridView.PageCount - 1;

							lbtnNext10.Visible = false;
							lbtnPrevious10.Visible = false;
						}
					}
					else
					{
						lbtnPrevious.Visible = false;
					}										
				}			
			}

			if (mDataGrid != null)
			{
				if (mDataGrid.PageCount <= 1)
				{
					lbtnNext.Visible = false;
					lbtnPrevious.Visible = false;
					lbtnPrevious10.Visible = false;
					lbtnNext10.Visible = false;
					Repeater1.Visible = false;					
				}
				else
				{
					if (mDataGrid.CurrentPageIndex < mDataGrid.PageCount - 1)
					{
						lbtnNext.Visible = true;

						if (mDataGrid.PageCount > 9)
						{
							PageCount = 9;

							if (mDataGrid.CurrentPageIndex < mDataGrid.PageCount - 10)	//-1 für 0 basierten Index, -9 für Next10
							{
								string strPageIndex = mDataGrid.CurrentPageIndex.ToString();

								string sPage = strPageIndex.Substring(0, 1);
								sPage += "0";
								PagerStart = Convert.ToInt16(sPage);

								lbtnNext10.Visible = true;
							}
						}
						else
						{
							PageCount = mDataGrid.PageCount - 1;

							lbtnNext10.Visible = false;
							lbtnPrevious10.Visible = false;
						}
					}
					else 
					{ 
						lbtnNext.Visible = false; 
					}

					if (mDataGrid.CurrentPageIndex > 0)
					{
						lbtnPrevious.Visible = true;

						if (mDataGrid.PageCount > 9)
						{
							PageCount = 9;

							if (mDataGrid.CurrentPageIndex > 9)
							{
								string strPageIndex = mDataGrid.CurrentPageIndex.ToString();
								
								string sPage = strPageIndex.Substring(0, 1);
								sPage += "0";
								PagerStart = Convert.ToInt16(sPage);

								lbtnPrevious10.Visible = true;
							}
							else 
							{ 
								PagerStart = 0;
							}
						}
						else
						{
							PageCount = mDataGrid.PageCount - 1;

							lbtnNext10.Visible = false;
							lbtnPrevious10.Visible = false;
						}		
					}
					else
					{
						lbtnPrevious.Visible = false;
					}										
				}
				
				// Repeater füllen
				int i;
				DataTable LinkTable = new DataTable();

				LinkTable.Columns.Add("Index", Type.GetType("System.String"));
				LinkTable.Columns.Add("Page", Type.GetType("System.String"));

				for (i = 0; i >= PagerStart && i <= PageCount; i++) //Rows mit Index und Page hinzufügen
				{
					LinkTable.Rows.Add(new int[]{i,i+1});
				}

				Repeater1.DataSource = LinkTable;
				Repeater1.DataBind();
				Repeater1.Visible = true;
			}
		}

		protected void lbtnPrevious_Click(object sender, EventArgs e)
		{
			if (mDataGrid != null)
			{
                mDataGrid.CurrentPageIndex --;
                PagerChanged(this, mDataGrid.CurrentPageIndex);
			}
			else if (mGridView != null)
			{
                mGridView.PageIndex --;
                PagerChanged(this, mGridView.PageIndex);
			}
			else
			{
				throw new Exception("GridNavigation: noch nicht initialisiert");
			}
           
		}

		protected void lbtnPrevious10_Click(object sender, EventArgs e)
		{
            int tmpIntPageintex;
            //DataGridPageChangedEventArgs EventArg;

            if (mDataGrid != null)
            {
                tmpIntPageintex = mDataGrid.CurrentPageIndex - 9;
                if ((tmpIntPageintex).ToString().Length > 1)
                {
                    string sPage = (tmpIntPageintex).ToString().Substring(0, 1);
                    sPage += "0";
                    PagerStart = Convert.ToInt16(sPage);
                }
                tmpIntPageintex = PagerStart;
                //EventArg = new DataGridPageChangedEventArgs(mDataGrid, tmpIntPageintex);
                
            }
            else if (mGridView != null)
            {
                tmpIntPageintex = mGridView.PageIndex - 9;
                if ((tmpIntPageintex).ToString().Length > 1)
                {
                    string sPage = (tmpIntPageintex).ToString().Substring(0, 1);
                    sPage += "0";
                    PagerStart = Convert.ToInt16(sPage);
                }
                tmpIntPageintex = PagerStart;
                //EventArg = new DataGridPageChangedEventArgs(mGridView, tmpIntPageintex);
                
            }
            else
            {
                throw new Exception("GridNavigation: noch nicht initialisiert");
            }
            PagerChanged(this, tmpIntPageintex);
		}

		protected void lbtnNext10_Click(object sender, EventArgs e)
		{
            int tmpIntPageindex;
            //DataGridPageChangedEventArgs EventArg;

            if (mDataGrid != null)
            {
                tmpIntPageindex = mDataGrid.CurrentPageIndex + 9;
                if ((tmpIntPageindex + 1).ToString().Length > 1)
                {
                    String sPage = (tmpIntPageindex + 1).ToString().Substring(0, 1);
                    sPage += "0";
                    PagerStart = Convert.ToInt16(sPage);
                }
                tmpIntPageindex = PagerStart;
                //EventArg = new DataGridPageChangedEventArgs(mDataGrid, tmpIntPageintex);
            }
            else if (mGridView != null)
            {
                tmpIntPageindex = mGridView.PageIndex + 9;
                if ((tmpIntPageindex + 1).ToString().Length > 1)
                {
                    string sPage = (tmpIntPageindex + 1).ToString().Substring(0, 1);
                    sPage += "0";
                    PagerStart = Convert.ToInt16(sPage);
                }
                tmpIntPageindex = PagerStart;
                //EventArg = new DataGridPageChangedEventArgs(mGridView, tmpIntPageintex);                
            }
            else
            {
                throw new Exception("GridNavigation: noch nicht initialisiert");
            }
            PagerChanged(this, tmpIntPageindex);
		}

		protected void lbtnNext_Click(object sender, EventArgs e)
		{
            if (mDataGrid != null)
            {
                mDataGrid.CurrentPageIndex ++;
                PagerChanged(this, mDataGrid.CurrentPageIndex);
            }
            else if (mGridView != null)
            {
                mGridView.PageIndex ++;
                PagerChanged(this, mGridView.PageIndex);
            }
            else
            {
                throw new Exception("GridNavigation: noch nicht initialisiert");
            }
		}
        
		protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (mDataGrid != null)
			{
				mDataGrid.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);
			}
			else if (mGridView != null)
			{
				mGridView.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);
			}
			else
			{
				throw new Exception("GridNavigation: noch nicht initialisiert");
			}
			PageSizeChanged();
		}

        //public void lbtnPage_PageIndexChanging(object sender, EventArgs e)
        //{
        //    LinkButton lbtn = (LinkButton)sender;
        //    if (mDataGrid != null)
        //    {
        //        PagerChanged(this,Convert.ToInt32(lbtn.CommandArgument) );//new DataGridPageChangedEventArgs(mDataGrid, Convert.ToInt32(lbtn.CommandArgument))
        //    }
        //    else if (mGridView != null)
        //    {
        //        PagerChanged(this, Convert.ToInt32(lbtn.CommandArgument)); //new DataGridPageChangedEventArgs(mGridView, Convert.ToInt32(lbtn.CommandArgument))
        //    }
        //}

		private void Page_PreRender(object sender, EventArgs e)
		{
			//ProofPager();

			lblAnzahl.Visible = true;

			if (mDataGrid != null)
			{
				if (mDataGrid.DataSource is DataView)
				{
					lblAnzahl.Text = "Gesamtanzahl: " + ((DataView)mDataGrid.DataSource).Count;
				}
				else if (mDataGrid.DataSource is DataTable)
				{
					lblAnzahl.Text = "Gesamtanzahl: " + ((DataTable)mDataGrid.DataSource).Rows.Count;
				}
				else
				{
					lblAnzahl.Visible = false;
				}
			}
			else if (mGridView != null)
			{
				if (mGridView.DataSource is DataView)
				{
					lblAnzahl.Text = "Gesamtanzahl: " + ((DataView)mGridView.DataSource).Count;
				}
				else if (mGridView.DataSource is DataTable)
				{
					lblAnzahl.Text = "Gesamtanzahl: " + ((DataTable)mGridView.DataSource).Rows.Count;
				}
				else
				{
					lblAnzahl.Visible = false;
				}
			}
			else
			{
				throw new Exception("GridNavigation: noch nicht initialisiert");
			}
		}
	}
}
