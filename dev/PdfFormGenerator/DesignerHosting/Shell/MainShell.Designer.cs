namespace Shell
{
    partial class MainShell
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainShell));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.newMenuItem = new System.Windows.Forms.MenuItem();
            this.formMenuItem = new System.Windows.Forms.MenuItem();
            this.userControlMenuItem = new System.Windows.Forms.MenuItem();
            this.componentMenuItem = new System.Windows.Forms.MenuItem();
            this.grapherMenuItem = new System.Windows.Forms.MenuItem();
            this.typeMenuItem = new System.Windows.Forms.MenuItem();
            this.noLoaderMenuItem = new System.Windows.Forms.MenuItem();
            this.basicDesignerLoaderMenuItem = new System.Windows.Forms.MenuItem();
            this.codeDomDesignerLoaderMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.saveMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
            this.runMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.eMenuItem = new System.Windows.Forms.MenuItem();
            this.cutMenuItem = new System.Windows.Forms.MenuItem();
            this.cMenuItem = new System.Windows.Forms.MenuItem();
            this.pMenuItem = new System.Windows.Forms.MenuItem();
            this.deMenuItem = new System.Windows.Forms.MenuItem();
            this.selectAllMenuItem = new System.Windows.Forms.MenuItem();
            this.alignoLoaderMenuItem = new System.Windows.Forms.MenuItem();
            this.leMenuItem = new System.Windows.Forms.MenuItem();
            this.ceMenuItem = new System.Windows.Forms.MenuItem();
            this.rightsMenuItem = new System.Windows.Forms.MenuItem();
            this.tMenuItem = new System.Windows.Forms.MenuItem();
            this.miMenuItem = new System.Windows.Forms.MenuItem();
            this.bMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuItem = new System.Windows.Forms.MenuItem();
            this.copenMenuItem = new System.Windows.Forms.MenuItem();
            this.xmlMenuItem = new System.Windows.Forms.MenuItem();
            this.cMenuItem1 = new System.Windows.Forms.MenuItem();
            this.vBMenuItem = new System.Windows.Forms.MenuItem();
            this.toolsMenuItem = new System.Windows.Forms.MenuItem();
            this.openMenuItem1 = new System.Windows.Forms.MenuItem();
            this.hMenuItem = new System.Windows.Forms.MenuItem();
            this.abMenuItem = new System.Windows.Forms.MenuItem();
            this.StatusBar = new System.Windows.Forms.StatusBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.Toolbox = new ToolboxLibrary.Toolbox();
            this.propertyGrid1 = new Shell.MyPropertyGrid();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.OutputWindow = new ToolWindows.OutputWindow();
            this.toolbox1 = new ToolboxLibrary.Toolbox();
            this.toolbox2 = new ToolboxLibrary.Toolbox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.eMenuItem,
            this.viewMenuItem,
            this.toolsMenuItem,
            this.hMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newMenuItem,
            this.typeMenuItem,
            this.openMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.runMenuItem,
            this.exitMenuItem});
            this.fileMenuItem.Text = "&File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Index = 0;
            this.newMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.formMenuItem,
            this.userControlMenuItem,
            this.componentMenuItem,
            this.grapherMenuItem});
            this.newMenuItem.Text = "&New";
            // 
            // formMenuItem
            // 
            this.formMenuItem.Index = 0;
            this.formMenuItem.Text = "&Form";
            this.formMenuItem.Click += new System.EventHandler(this.formMenuItem_Click);
            // 
            // userControlMenuItem
            // 
            this.userControlMenuItem.Index = 1;
            this.userControlMenuItem.Text = "&UserControl";
            this.userControlMenuItem.Click += new System.EventHandler(this.userControlMenuItem_Click);
            // 
            // componentMenuItem
            // 
            this.componentMenuItem.Index = 2;
            this.componentMenuItem.Text = "&Component";
            this.componentMenuItem.Click += new System.EventHandler(this.componentMenuItem_Click);
            // 
            // grapherMenuItem
            // 
            this.grapherMenuItem.Index = 3;
            this.grapherMenuItem.Text = "&Grapher";
            this.grapherMenuItem.Click += new System.EventHandler(this.grapherMenuItem_Click);
            // 
            // typeMenuItem
            // 
            this.typeMenuItem.Index = 1;
            this.typeMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.noLoaderMenuItem,
            this.basicDesignerLoaderMenuItem,
            this.codeDomDesignerLoaderMenuItem});
            this.typeMenuItem.Text = "&Type";
            // 
            // noLoaderMenuItem
            // 
            this.noLoaderMenuItem.Checked = true;
            this.noLoaderMenuItem.Index = 0;
            this.noLoaderMenuItem.Text = "&No Loader";
            this.noLoaderMenuItem.Click += new System.EventHandler(this.noLoaderMenuItem_Click);
            // 
            // basicDesignerLoaderMenuItem
            // 
            this.basicDesignerLoaderMenuItem.Index = 1;
            this.basicDesignerLoaderMenuItem.Text = "&BasicDesignerLoader";
            this.basicDesignerLoaderMenuItem.Click += new System.EventHandler(this.basicDesignerLoaderMenuItem_Click);
            // 
            // codeDomDesignerLoaderMenuItem
            // 
            this.codeDomDesignerLoaderMenuItem.Index = 2;
            this.codeDomDesignerLoaderMenuItem.Text = "&CodeDomDesignerLoader";
            this.codeDomDesignerLoaderMenuItem.Click += new System.EventHandler(this.codeDomDesignerLoaderMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 2;
            this.openMenuItem.Text = "&Open";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Index = 3;
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Index = 4;
            this.saveAsMenuItem.Text = "S&ave As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // runMenuItem
            // 
            this.runMenuItem.Index = 5;
            this.runMenuItem.Text = "&Run";
            this.runMenuItem.Click += new System.EventHandler(this.runMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 6;
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // eMenuItem
            // 
            this.eMenuItem.Index = 1;
            this.eMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cutMenuItem,
            this.cMenuItem,
            this.pMenuItem,
            this.deMenuItem,
            this.selectAllMenuItem,
            this.alignoLoaderMenuItem});
            this.eMenuItem.Text = "&Edit";
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Index = 0;
            this.cutMenuItem.Text = "&Cut";
            this.cutMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // cMenuItem
            // 
            this.cMenuItem.Index = 1;
            this.cMenuItem.Text = "C&opy";
            this.cMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // pMenuItem
            // 
            this.pMenuItem.Index = 2;
            this.pMenuItem.Text = "&Paste";
            this.pMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // deMenuItem
            // 
            this.deMenuItem.Index = 3;
            this.deMenuItem.Text = "&Delete";
            this.deMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Index = 4;
            this.selectAllMenuItem.Text = "&Select All";
            this.selectAllMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // alignoLoaderMenuItem
            // 
            this.alignoLoaderMenuItem.Index = 5;
            this.alignoLoaderMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.leMenuItem,
            this.ceMenuItem,
            this.rightsMenuItem,
            this.tMenuItem,
            this.miMenuItem,
            this.bMenuItem});
            this.alignoLoaderMenuItem.Text = "&Align";
            // 
            // leMenuItem
            // 
            this.leMenuItem.Index = 0;
            this.leMenuItem.Text = "&Lefts";
            this.leMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // ceMenuItem
            // 
            this.ceMenuItem.Index = 1;
            this.ceMenuItem.Text = "&Centers";
            this.ceMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // rightsMenuItem
            // 
            this.rightsMenuItem.Index = 2;
            this.rightsMenuItem.Text = "&Rights";
            this.rightsMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // tMenuItem
            // 
            this.tMenuItem.Index = 3;
            this.tMenuItem.Text = "&Tops";
            this.tMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // miMenuItem
            // 
            this.miMenuItem.Index = 4;
            this.miMenuItem.Text = "&Middles";
            this.miMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // bMenuItem
            // 
            this.bMenuItem.Index = 5;
            this.bMenuItem.Text = "&Bottoms";
            this.bMenuItem.Click += new System.EventHandler(this.ActionClick);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.Index = 2;
            this.viewMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.copenMenuItem});
            this.viewMenuItem.Text = "&View";
            // 
            // copenMenuItem
            // 
            this.copenMenuItem.Index = 0;
            this.copenMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.xmlMenuItem,
            this.cMenuItem1,
            this.vBMenuItem});
            this.copenMenuItem.Text = "&Code";
            // 
            // xmlMenuItem
            // 
            this.xmlMenuItem.Index = 0;
            this.xmlMenuItem.Text = "&Xml";
            this.xmlMenuItem.Click += new System.EventHandler(this.xmlMenuItem_Click);
            // 
            // cMenuItem1
            // 
            this.cMenuItem1.Index = 1;
            this.cMenuItem1.Text = "&C#";
            this.cMenuItem1.Click += new System.EventHandler(this.cMenuItem1_Click);
            // 
            // vBMenuItem
            // 
            this.vBMenuItem.Index = 2;
            this.vBMenuItem.Text = "&VB";
            this.vBMenuItem.Click += new System.EventHandler(this.vBMenuItem_Click);
            // 
            // toolsMenuItem
            // 
            this.toolsMenuItem.Index = 3;
            this.toolsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openMenuItem1});
            this.toolsMenuItem.Text = "&Tools";
            // 
            // openMenuItem1
            // 
            this.openMenuItem1.Index = 0;
            this.openMenuItem1.Text = "&Options...";
            this.openMenuItem1.Click += new System.EventHandler(this.openMenuItem1_Click);
            // 
            // hMenuItem
            // 
            this.hMenuItem.Index = 4;
            this.hMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.abMenuItem});
            this.hMenuItem.Text = "&Help";
            // 
            // abMenuItem
            // 
            this.abMenuItem.Index = 0;
            this.abMenuItem.Text = "&About";
            this.abMenuItem.Click += new System.EventHandler(this.abMenuItem_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 385);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(1050, 22);
            this.StatusBar.TabIndex = 1;
            this.StatusBar.Text = "Ready";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1Collapsed = true;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1050, 385);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(1050, 385);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer3.Size = new System.Drawing.Size(235, 385);
            this.splitContainer3.SplitterDistance = 52;
            this.splitContainer3.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(235, 53);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbSave
            // 
            this.tbSave.AutoSize = false;
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(50, 50);
            this.tbSave.Text = "Speichern";
            this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.Toolbox);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer5.Size = new System.Drawing.Size(235, 333);
            this.splitContainer5.SplitterDistance = 205;
            this.splitContainer5.TabIndex = 1;
            // 
            // Toolbox
            // 
            this.Toolbox.DesignerHost = null;
            this.Toolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Toolbox.FilePath = null;
            this.Toolbox.Location = new System.Drawing.Point(0, 0);
            this.Toolbox.Name = "Toolbox";
            this.Toolbox.SelectedCategory = null;
            this.Toolbox.Size = new System.Drawing.Size(235, 205);
            this.Toolbox.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.Size = new System.Drawing.Size(235, 124);
            this.propertyGrid1.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.OutputWindow);
            this.splitContainer4.Panel2Collapsed = true;
            this.splitContainer4.Size = new System.Drawing.Size(811, 385);
            this.splitContainer4.SplitterDistance = 25;
            this.splitContainer4.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 385);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // OutputWindow
            // 
            this.OutputWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputWindow.Location = new System.Drawing.Point(0, 0);
            this.OutputWindow.Name = "OutputWindow";
            this.OutputWindow.Size = new System.Drawing.Size(150, 46);
            this.OutputWindow.TabIndex = 0;
            // 
            // toolbox1
            // 
            this.toolbox1.DesignerHost = null;
            this.toolbox1.FilePath = null;
            this.toolbox1.Location = new System.Drawing.Point(0, 0);
            this.toolbox1.Name = "toolbox1";
            this.toolbox1.SelectedCategory = null;
            this.toolbox1.Size = new System.Drawing.Size(127, 283);
            this.toolbox1.TabIndex = 0;
            // 
            // toolbox2
            // 
            this.toolbox2.DesignerHost = null;
            this.toolbox2.FilePath = null;
            this.toolbox2.Location = new System.Drawing.Point(0, 0);
            this.toolbox2.Name = "toolbox2";
            this.toolbox2.SelectedCategory = null;
            this.toolbox2.Size = new System.Drawing.Size(127, 283);
            this.toolbox2.TabIndex = 0;
            // 
            // MainShell
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1050, 407);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MainShell";
            this.Text = "CKG PDF Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem fileMenuItem;
        private System.Windows.Forms.MenuItem newMenuItem;
        private System.Windows.Forms.MenuItem formMenuItem;
        private System.Windows.Forms.MenuItem userControlMenuItem;
        private System.Windows.Forms.MenuItem componentMenuItem;
        private System.Windows.Forms.MenuItem grapherMenuItem;
        private System.Windows.Forms.MenuItem typeMenuItem;
        private System.Windows.Forms.MenuItem noLoaderMenuItem;
        private System.Windows.Forms.MenuItem basicDesignerLoaderMenuItem;
        private System.Windows.Forms.MenuItem codeDomDesignerLoaderMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem saveMenuItem;
        private System.Windows.Forms.MenuItem saveAsMenuItem;
        private System.Windows.Forms.MenuItem runMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.MenuItem eMenuItem;
        private System.Windows.Forms.MenuItem cutMenuItem;
        private System.Windows.Forms.MenuItem cMenuItem;
        private System.Windows.Forms.MenuItem pMenuItem;
        private System.Windows.Forms.MenuItem deMenuItem;
        private System.Windows.Forms.MenuItem selectAllMenuItem;
        private System.Windows.Forms.MenuItem alignoLoaderMenuItem;
        private System.Windows.Forms.MenuItem leMenuItem;
        private System.Windows.Forms.MenuItem ceMenuItem;
        private System.Windows.Forms.MenuItem rightsMenuItem;
        private System.Windows.Forms.MenuItem tMenuItem;
        private System.Windows.Forms.MenuItem miMenuItem;
        private System.Windows.Forms.MenuItem bMenuItem;
        private System.Windows.Forms.MenuItem viewMenuItem;
        private System.Windows.Forms.MenuItem copenMenuItem;
        private System.Windows.Forms.MenuItem xmlMenuItem;
        private System.Windows.Forms.MenuItem cMenuItem1;
        private System.Windows.Forms.MenuItem vBMenuItem;
        private System.Windows.Forms.MenuItem toolsMenuItem;
        private System.Windows.Forms.MenuItem openMenuItem1;
        private System.Windows.Forms.MenuItem hMenuItem;
        private System.Windows.Forms.MenuItem abMenuItem;
        public System.Windows.Forms.StatusBar StatusBar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private ToolboxLibrary.Toolbox toolbox1;
        private ToolboxLibrary.Toolbox toolbox2;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private ToolWindows.OutputWindow OutputWindow;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private ToolboxLibrary.Toolbox Toolbox;
        private MyPropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbSave;
    }
}