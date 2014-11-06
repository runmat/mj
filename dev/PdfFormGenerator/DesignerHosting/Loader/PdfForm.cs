using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Loader
{
    public class PdfForm : Form
    {
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
        public override Cursor Cursor { get; set; }

        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle { get { return base.FormBorderStyle; } set { base.FormBorderStyle = value; } }

        [Browsable(false)]
        public override RightToLeft RightToLeft { get; set; }

        [Browsable(false)]
        public new bool CausesValidation { get; set; }

        [Browsable(false)]
        public override AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; } }

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
        public new bool Enabled { get; set; }

        [Browsable(false)]
        public new int TabIndex { get; set; }

        [Browsable(false)]
        public new bool UseWaitCursor { get; set; }

        [Browsable(false)]
        public new ControlBindingsCollection DataBindings { get { return base.DataBindings; } }

        [Browsable(false)]
        public new IButtonControl AcceptButton { get; set; }

        [Browsable(false)]
        public new bool AutoScroll { get { return base.AutoScroll; } set { base.AutoScroll = value; } }

        [Browsable(false)]
        public new Padding AutoScrollMargin { get; set; }

        [Browsable(false)]
        public new Size AutoScrollMinSize { get; set; }

        [Browsable(false)]
        public new  AutoSizeMode AutoSizeMode { get; set; }

        [Browsable(false)]
        public new  AutoValidate AutoValidate { get; set; }

        [Browsable(false)]
        public override sealed ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

        [Browsable(false)]
        public new IButtonControl CancelButton { get; set; }

        [Browsable(false)]
        public new IButtonControl HelpButton { get; set; }

        [Browsable(false)]
        public new bool ControlBox { get; set; }

        [Browsable(false)]
        public new Font Font { get { return base.Font; } set { base.Font = value; } }

        [Browsable(false)]
        public new Color ForeColor { get; set; }

        [Browsable(false)]
        public new Icon Icon { get; set; }

        [Browsable(false)]
        public new ImeMode ImeMode { get; set; }

        [Browsable(false)]
        public new bool IsMdiContainer { get; set; }

        [Browsable(false)]
        public new bool KeyPreview { get; set; }

        [Browsable(false)]
        public new bool MaximizeBox { get; set; }

        [Browsable(false)]
        public new bool MinimizeBox { get; set; }

        [Browsable(false)]
        public new MenuStrip MainMenuStrip { get; set; }

        [Browsable(false)]
        public new double Opacity { get { return base.Opacity; } set { base.Opacity = value; } }

        [Browsable(false)]
        public override bool RightToLeftLayout { get { return base.RightToLeftLayout; } set { base.RightToLeftLayout = value; } }

        [Browsable(false)]
        public new bool ShowIcon { get { return base.ShowIcon; } set { base.ShowIcon = value; } }

        [Browsable(false)]
        public new bool ShowInTaskbar { get { return base.ShowInTaskbar; } set { base.ShowInTaskbar = value; } }

        [Browsable(false)]
        public new SizeGripStyle SizeGripStyle { get { return base.SizeGripStyle; } set { base.SizeGripStyle = value; } }

        [Browsable(false)]
        public new FormStartPosition StartPosition { get { return base.StartPosition; } set { base.StartPosition = value; } }

        [Browsable(false)]
        public new FormWindowState WindowState { get { return base.WindowState; } set { base.WindowState = value; } }

        [Browsable(false)]
        public new string Text { get { return base.Text; } set { base.Text = value; } }

        [Browsable(false)]
        public new object Tag { get { return base.Tag; } set { base.Tag = value; } }

        [Browsable(false)]
        public new bool TopMost { get { return base.TopMost; } set { base.TopMost = value; } }

        [Browsable(false)]
        public new Color TransparencyKey { get { return base.TransparencyKey; } set { base.TransparencyKey = value; } }

        [Browsable(false)]
        public new Point Location { get { return base.Location; } set { base.Location = value; } }

        [Browsable(false)]
        public new Size Size { get { return base.Size; } set { base.Size = value; } }

        [Browsable(false)]
        protected override bool DoubleBuffered { get { return base.DoubleBuffered; } set { base.DoubleBuffered = value; } }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new AutoScaleMode AutoScaleMode { get { return base.AutoScaleMode; } set { base.AutoScaleMode = value; } }

        #endregion



        public PdfForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackgroundImageLayout = ImageLayout.None;
        }
    }
}
