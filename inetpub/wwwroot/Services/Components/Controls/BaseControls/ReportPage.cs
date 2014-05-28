namespace CKG.Components.Controls
{
    using System;
    using System.Web.UI;

    using CKG.Base.Kernel.Common;
    using CKG.Base.Kernel.Security;

    public abstract class ReportPage : Page
    {
        protected User CKGUser { get; private set; }
        protected App App { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CKGUser = Common.GetUser(this);
            Common.FormAuth(this, this.CKGUser);
            this.App = new App(this.CKGUser);

            Common.GetAppIDFromQueryString(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Common.SetEndASPXAccess(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            Common.SetEndASPXAccess(this);
        }
    }
}