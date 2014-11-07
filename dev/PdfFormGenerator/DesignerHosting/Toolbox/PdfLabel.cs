using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ToolboxLibrary
{
    public class PdfLabel : Label 
    {
        private string _name;

        #region Designer Hidden Properties

        [Browsable(false)]
        public new string AccessibleDescription { get; set; }

        [Browsable(false)]
        public new string AccessibleName { get; set; }

        [Browsable(false)]
        public new string AccessibleRole { get; set; }

        [Browsable(false)]
        public override Color BackColor { get; set; }

        [Browsable(false)]
        public new FlatStyle FlatStyle { get; set; }

        [Browsable(false)]
        public override Cursor Cursor { get; set; }

        [Browsable(false)]
        public override BorderStyle BorderStyle { get; set; }

        [Browsable(false)]
        public new Image Image { get; set; }

        [Browsable(false)]
        public new ContentAlignment ImageAlign
        {
            get { return base.ImageAlign; }
            set { base.ImageAlign = value; }
        }

        [Browsable(false)]
        public new int ImageIndex { get; set; }

        [Browsable(false)]
        public new string ImageKey { get; set; }

        [Browsable(false)]
        public new ImageList ImageList { get; set; }

        [Browsable(false)]
        public override RightToLeft RightToLeft { get; set; }

        [Browsable(false)]
        public new bool CausesValidation { get; set; }

        [Browsable(false)]
        public override AnchorStyles Anchor { get; set; }

        [Browsable(false)]
        public override bool AutoSize { get; set; }

        [Browsable(false)]
        public new Padding Margin { get; set; }

        [Browsable(false)]
        public new Padding Padding { get; set; }

        [Browsable(false)]
        public override DockStyle Dock { get; set; }

        [Browsable(false)]
        public override Size MinimumSize { get; set; }

        [Browsable(false)]
        public override Size MaximumSize { get; set; }

        [Browsable(false)]
        public override bool AllowDrop { get; set; }

        [Browsable(false)]
        public override ContextMenu ContextMenu { get; set; }

        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip { get; set; }

        [Browsable(false)]
        public new bool AutoEllipsis { get; set; }

        [Browsable(false)]
        public new bool Enabled { get; set; }

        [Browsable(false)]
        public new bool Locked { get; set; }

        [Browsable(false)]
        public new int TabIndex { get; set; }

        [Browsable(false)]
        public new bool UseCompatibleTextRendering { get; set; }

        [Browsable(false)]
        public new bool UseMnemonic { get; set; }

        [Browsable(false)]
        public new bool UseWaitCursor { get; set; }

        [Browsable(false)]
        public override ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        [Browsable(false)]
        public new ControlBindingsCollection DataBindings
        {
            get { return base.DataBindings; }
        }

        [Browsable(false)]
        public new string Tag { get; set; }

        #endregion



        public PdfLabel()
        {
            ForeColor = Color.Red;
            BackColor = Color.Transparent;
            Height = 16;
            Width = 389;
            TextAlign = ContentAlignment.TopLeft;
        }
    }
}
