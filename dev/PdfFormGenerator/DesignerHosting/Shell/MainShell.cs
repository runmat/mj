using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Host;
using Loader;
using ToolWindows;

namespace Shell
{
    /// <summary>
    /// This is the Shell that has the Toolbox, PropertyGrid, hosts Designers, etc.
    /// </summary>
    public partial class MainShell : Form
    {
        private int _formCount = 0;
        private int _userControlCount = 0;
        private int _componentCount = 0;
        private int _grapherCount = 0;
        private HostSurfaceManager _hostSurfaceManager = null;
        private int _prevIndex = 0;
        private int _curIndex = 0;

        public MainShell()
        {
            InitializeComponent();
            CustomInitialize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var currentHostControl = CurrentDocumentsHostControl;
            if (CurrentActiveDocumentLoaderType == LoaderType.BasicDesignerLoader)
                ((BasicHostLoader)currentHostControl.HostSurface.Loader).PromptDispose();

            base.OnClosing(e);
        }

        /// <summary>
        /// Adds custom services to the HostManager like TGoolbox, PropertyGrid, 
        /// SolutionExplorer.
        /// OutputWindow is added as a service. It is used by the HostSurfaceManager
        /// to write out to the OutputWindow. You can add any services
        /// you want.
        /// </summary>
        private void CustomInitialize()
        {
            _hostSurfaceManager = new HostSurfaceManager();
            _hostSurfaceManager.AddService(typeof(IToolboxService), this.toolbox1);
            _hostSurfaceManager.AddService(typeof(ToolWindows.OutputWindow), this.OutputWindow);
            _hostSurfaceManager.AddService(typeof(System.Windows.Forms.PropertyGrid), this.propertyGrid1);

            codeDomDesignerLoaderMenuItem_Click(null, null);
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.noLoaderMenuItem_Click(null, null);


            OpenFile(@"test.xml");
        }

        private int CurrentDocumentsDesignIndex
        {
            get
            {
                string codeText;
                string designText;
                int index = 0;

                if (CurrentDocumentView == Strings.Design)
                    return this.tabControl1.SelectedIndex;
                else
                {
                    // This is the Code View. So let us find the Design View
                    codeText = this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Text.Trim();
                    designText = codeText.Replace(Strings.Code, Strings.Design);
                    foreach (TabPage tab in this.tabControl1.TabPages)
                    {
                        if (tab.Text == designText)
                            return index;
                        index++;
                    }
                }

                return -1;
            }
        }
        private int CurrentDocumentsCodeIndex
        {
            get
            {
                if (CurrentDocumentView == Strings.Code)
                    return this.tabControl1.SelectedIndex;

                int index = 0;

                //HostControl currentHostControl = CurrentDocumentsHostControl;
                // Find out if the Code Tab already exists
                string designText = this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Text.Trim();
                string codeText = designText.Replace(Strings.Design, Strings.Code);

                foreach (TabPage tab in this.tabControl1.TabPages)
                {
                    if (tab.Text == codeText)
                        return index;

                    index++;
                }

                TabPage tabPage = new TabPage();

                tabPage.Text = codeText;
                tabPage.Tag = CurrentActiveDocumentLoaderType;
                this.tabControl1.Controls.Add(tabPage);
                //this.tabControl1.Controls.Add(tabPage);

                // Create new RichTextBox for codeEditor
                RichTextBox codeEditor = new System.Windows.Forms.RichTextBox();

                codeEditor.BackColor = System.Drawing.SystemColors.Desktop;
                codeEditor.ForeColor = System.Drawing.Color.White;
                codeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
                codeEditor.Font = new System.Drawing.Font("Verdana", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                codeEditor.Location = new System.Drawing.Point(0, 0);
                codeEditor.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
                codeEditor.WordWrap = false;
                codeEditor.Size = new System.Drawing.Size(284, 247);
                codeEditor.TabIndex = 0;
                codeEditor.ReadOnly = true;
                codeEditor.Text = "";
                tabPage.Controls.Add(codeEditor);
                return this.tabControl1.TabPages.Count - 1;
            }
        }
        private HostControl CurrentDocumentsHostControl
        {
            get
            {
                return (HostControl)this.tabControl1.TabPages[CurrentDocumentsDesignIndex].Controls[0];
            }
        }
        private RichTextBox CurrentDocumentsCodeEditor
        {
            get
            {
                return (RichTextBox)this.tabControl1.TabPages[CurrentDocumentsCodeIndex].Controls[0];
            }
        }
        private LoaderType CurrentMenuSelectionLoaderType
        {
            get
            {
                if (this.basicDesignerLoaderMenuItem.Checked)
                    return LoaderType.BasicDesignerLoader;
                else if (this.codeDomDesignerLoaderMenuItem.Checked)
                    return LoaderType.CodeDomDesignerLoader;
                else
                    return LoaderType.NoLoader;
            }
        }
        private LoaderType CurrentActiveDocumentLoaderType
        {
            get
            {
                TabPage tabPage = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];

                return (LoaderType)tabPage.Tag;
            }
        }
        private string CurrentDocumentView
        {
            get
            {
                TabPage tabPage = this.tabControl1.TabPages[this.tabControl1.SelectedIndex];

                if (tabPage.Text.Contains(Strings.Design))
                    return Strings.Design;
                else
                    return Strings.Code;
            }
        }


        private void basicDesignerLoaderMenuItem_Click(object sender, System.EventArgs e)
        {
            this.noLoaderMenuItem.Checked = false;
            this.codeDomDesignerLoaderMenuItem.Checked = false;
            this.basicDesignerLoaderMenuItem.Checked = true;

            // Disable all item types except Form
            this.formMenuItem.Enabled = true;
            this.userControlMenuItem.Enabled = false;
            this.componentMenuItem.Enabled = false;
            this.grapherMenuItem.Enabled = false;
        }

        private void noLoaderMenuItem_Click(object sender, System.EventArgs e)
        {
            this.noLoaderMenuItem.Checked = true;
            this.codeDomDesignerLoaderMenuItem.Checked = false;
            this.basicDesignerLoaderMenuItem.Checked = false;

            // Enable all item types
            this.formMenuItem.Enabled = true;
            this.userControlMenuItem.Enabled = true;
            this.componentMenuItem.Enabled = true;
            this.grapherMenuItem.Enabled = true;
        }

        private void codeDomDesignerLoaderMenuItem_Click(object sender, System.EventArgs e)
        {
            this.noLoaderMenuItem.Checked = false;
            this.codeDomDesignerLoaderMenuItem.Checked = true;
            this.basicDesignerLoaderMenuItem.Checked = false;

            // Disable all item types except Form
            this.formMenuItem.Enabled = true;
            this.userControlMenuItem.Enabled = false;
            this.componentMenuItem.Enabled = false;
            this.grapherMenuItem.Enabled = false;
        }

        private void xmlMenuItem_Click(object sender, System.EventArgs e)
        {
            SwitchToCode(Strings.Xml);
        }
        private void cMenuItem1_Click(object sender, System.EventArgs e)
        {
            SwitchToCode(Strings.CS);
        }

        private void vBMenuItem_Click(object sender, System.EventArgs e)
        {
            SwitchToCode(Strings.VB);
        }

        private void jMenuItem1_Click(object sender, System.EventArgs e)
        {
            SwitchToCode(Strings.JS);
        }

        private void SwitchToCode(string context)
        {
            if (CurrentActiveDocumentLoaderType == LoaderType.NoLoader)
            {
                MessageBox.Show("Code view is not supported for document loaded without Loaders");
                return;
            }

            if (context == Strings.Xml && CurrentActiveDocumentLoaderType != LoaderType.BasicDesignerLoader)
            {
                MessageBox.Show("Xml code view is supported only for BasicDesignerLoader");
                return;
            }

            if ((context == Strings.CS || context == Strings.VB || context == Strings.JS) && CurrentActiveDocumentLoaderType != LoaderType.CodeDomDesignerLoader)
            {
                MessageBox.Show(context + " code view is supported only for CodeDomDesignerLoader");
                return;
            }

            HostControl currentHostControl = CurrentDocumentsHostControl;
            RichTextBox codeEditor = CurrentDocumentsCodeEditor;
            if (CurrentActiveDocumentLoaderType == LoaderType.BasicDesignerLoader)
                codeEditor.Text = ((BasicHostLoader)currentHostControl.HostSurface.Loader).GetCode();
            else if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
                codeEditor.Text = ((CodeDomHostLoader)currentHostControl.HostSurface.Loader).GetCode(context);

            int index = CurrentDocumentsCodeIndex;

            if (this.tabControl1.SelectedIndex != index)
            {
                _prevIndex = this.tabControl1.SelectedIndex;
                this.tabControl1.SelectedIndex = index;
                _curIndex = index;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentDocumentView == Strings.Design)
            {
                if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
                    this.eMenuItem.Enabled = true;
                else
                    this.eMenuItem.Enabled = false;
            }
            else
            {
                if (CurrentActiveDocumentLoaderType == LoaderType.BasicDesignerLoader)
                    SwitchToCode(Strings.Xml);
                else if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
                    SwitchToCode(Strings.CS);
            }
        }

        /// <summary>
        /// Persist the code if the host is loaded using a BasicDesignerLoader
        /// </summary>
        private void saveMenuItem_Click(object sender, System.EventArgs e)
        {
            if (CurrentActiveDocumentLoaderType == LoaderType.NoLoader)
            {
                MessageBox.Show("Cannot persist document created without loaders");
                return;
            }

            if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
            {
                MessageBox.Show("Cannot persist document created using CodeDomDesignerLoader");
                return;
            }

            HostControl currentHostControl = CurrentDocumentsHostControl;

            if (CurrentActiveDocumentLoaderType == LoaderType.BasicDesignerLoader)
                ((BasicHostLoader)currentHostControl.HostSurface.Loader).Save();

            this.OutputWindow.RichTextBox.Text += "Saved host.\n";
        }

        /// <summary>
        /// Open an xml file that was saved earlier
        /// </summary>
        private void openMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string fileName = null;

                // Open File Dialog
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = "xml";
                dlg.Filter = "Xml Files|*.xml";
                if (dlg.ShowDialog() == DialogResult.OK)
                    fileName = dlg.FileName;

                if (fileName == null)
                    return;

                OpenFile(fileName);
            }
            catch
            {
                MessageBox.Show("Error in creating new host", "Shell Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void OpenFile(string fileName)
        {
            // Create Form
            _formCount++;
            HostControl hc = _hostSurfaceManager.GetNewHost(fileName);
            this.Toolbox.DesignerHost = hc.DesignerHost;
            TabPage tabpage = new TabPage("Form" + _formCount.ToString() + " - " + Strings.Design);

            if (fileName.EndsWith("xml"))
                tabpage.Tag = LoaderType.BasicDesignerLoader;
            else if (fileName.EndsWith("cs") || fileName.EndsWith("vb"))
                tabpage.Tag = LoaderType.CodeDomDesignerLoader;

            hc.Parent = tabpage;
            hc.Dock = DockStyle.Fill;
            this.tabControl1.TabPages.Add(tabpage);
            this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
            this.OutputWindow.RichTextBox.Text += "Opened new host.\n";
        }

        private void saveAsMenuItem_Click(object sender, System.EventArgs e)
        {
            if (CurrentActiveDocumentLoaderType == LoaderType.NoLoader)
            {
                MessageBox.Show("Cannot persist document created without loaders");
                return;
            }

            if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
            {
                MessageBox.Show("Cannot persist document created using CodeDomDesignerLoader");
                return;
            }

            HostControl currentHostControl = CurrentDocumentsHostControl;
            ((BasicHostLoader)currentHostControl.HostSurface.Loader).Save(true);
        }

        /// <summary>
        /// If the host was loaded using a CodeDomDesignerLoader then we can run it
        /// </summary>
        private void runMenuItem_Click(object sender, System.EventArgs e)
        {
            if (CurrentActiveDocumentLoaderType == LoaderType.NoLoader || CurrentActiveDocumentLoaderType == LoaderType.BasicDesignerLoader)
            {
                MessageBox.Show("Cannot Run document created without loaders or created using BasicDesignerLoader. To run a document use CodeDomDesignerLoader");
                return;
            }

            HostControl currentHostControl = CurrentDocumentsHostControl;
            ((CodeDomHostLoader)currentHostControl.HostSurface.Loader).Run();
        }

        private void exitMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void AddTabForNewHost(string tabText, HostControl hc)
        {
            this.Toolbox.DesignerHost = hc.DesignerHost;
            TabPage tabpage = new TabPage(tabText);
            tabpage.Tag = CurrentMenuSelectionLoaderType;
            hc.Parent = tabpage;
            hc.Dock = DockStyle.Fill;
            this.tabControl1.TabPages.Add(tabpage);
            this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
            _hostSurfaceManager.ActiveDesignSurface = hc.HostSurface;
            if (CurrentActiveDocumentLoaderType == LoaderType.CodeDomDesignerLoader)
                this.eMenuItem.Enabled = true;
            else
                this.eMenuItem.Enabled = false;
            
        }

        private void formMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                _formCount++;
                HostControl hc = _hostSurfaceManager.GetNewHost(typeof(Form), CurrentMenuSelectionLoaderType);
                AddTabForNewHost("Form" + _formCount.ToString() + " - " + Strings.Design, hc);
            }
            catch
            {
                MessageBox.Show("Error in creating new host", "Shell Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void userControlMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _userControlCount++;
                HostControl hc = _hostSurfaceManager.GetNewHost(typeof(UserControl), CurrentMenuSelectionLoaderType);
                AddTabForNewHost("UserControl" + _userControlCount.ToString() + " - " + Strings.Design, hc);
            }
            catch
            {
                MessageBox.Show("Error in creating new host", "Shell Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void componentMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _componentCount++;
                HostControl hc = _hostSurfaceManager.GetNewHost(typeof(Component), CurrentMenuSelectionLoaderType);
                AddTabForNewHost("Component" + _componentCount.ToString() + " - " + Strings.Design, hc);
            }
            catch
            {
                MessageBox.Show("Error in creating new host", "Shell Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grapherMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _grapherCount++;
                HostControl hc = _hostSurfaceManager.GetNewHost(typeof(MyTopLevelComponent), CurrentMenuSelectionLoaderType);
                AddTabForNewHost("CustomDesigner" + _grapherCount.ToString() + " - " + Strings.Design, hc);
            }
            catch
            {
                MessageBox.Show("Error in creating new host", "Shell Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Perform all the Edit menu options using the MenuCommandService
        /// </summary>
        private void PerformAction(string text)
        {
            if (this.CurrentDocumentView == Strings.Code)
            {
                MessageBox.Show("This is not in supported code view");
                return;
            }

            if (this.CurrentDocumentsHostControl == null)
                return;

            IMenuCommandService ims = this.CurrentDocumentsHostControl.HostSurface.GetService(typeof(IMenuCommandService)) as IMenuCommandService;

            try
            {
                switch (text)
                {
                    case "&Cut":
                        ims.GlobalInvoke(StandardCommands.Cut);
                        break;
                    case "C&opy":
                        ims.GlobalInvoke(StandardCommands.Copy);
                        break;
                    case "&Paste":
                        ims.GlobalInvoke(StandardCommands.Paste);
                        break;
                    case "&Undo":
                        ims.GlobalInvoke(StandardCommands.Undo);
                        break;
                    case "&Redo":
                        ims.GlobalInvoke(StandardCommands.Redo);
                        break;
                    case "&Delete":
                        ims.GlobalInvoke(StandardCommands.Delete);
                        break;
                    case "&Select All":
                        ims.GlobalInvoke(StandardCommands.SelectAll);
                        break;
                    case "&Lefts":
                        ims.GlobalInvoke(StandardCommands.AlignLeft);
                        break;
                    case "&Centers":
                        ims.GlobalInvoke(StandardCommands.AlignHorizontalCenters);
                        break;
                    case "&Rights":
                        ims.GlobalInvoke(StandardCommands.AlignRight);
                        break;
                    case "&Tops":
                        ims.GlobalInvoke(StandardCommands.AlignTop);
                        break;
                    case "&Middles":
                        ims.GlobalInvoke(StandardCommands.AlignVerticalCenters);
                        break;
                    case "&Bottoms":
                        ims.GlobalInvoke(StandardCommands.AlignBottom);
                        break;
                    default:
                        break;
                }
            }
            catch 
            {
                this.OutputWindow.RichTextBox.Text += "Error in performing the action: " + text.Replace("&", "");
            }
        }

        private void ActionClick(object sender, EventArgs e)
        {
            PerformAction((sender as MenuItem).Text);
        }

        private void openMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Options can be implemented here");
        }

        private void abMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CKG PDF Designer, (c) 2014-15 Christoph Kroschke GmbH");
        }

        private class Strings
        {
            public const string Design = "Design";
            public const string Code = "Code";
            public const string Xml = "Xml";
            public const string CS = "C#";
            public const string JS = "J#";
            public const string VB = "VB";
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            saveMenuItem_Click(null, null);
        }
    }
}