using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung
{
    public interface IWizardStep
    {
        event EventHandler<EventArgs> Completed;
        event EventHandler<EventArgs> NavigateBack;
        void Validate();
        void ResetNavigation();
    }
}