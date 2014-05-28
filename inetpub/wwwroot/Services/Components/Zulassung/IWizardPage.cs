using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Components.Zulassung.DAL;
using CKG.Base.Kernel.Security;

namespace CKG.Components.Zulassung
{
    public interface IWizardPage
    {
        ZulassungDal DAL { get; }
        User User { get; }
        App App { get; }
    }
}