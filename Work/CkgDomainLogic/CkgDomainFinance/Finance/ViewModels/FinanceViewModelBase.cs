using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;

namespace CkgDomainLogic.Finance.ViewModels
{
    public abstract class FinanceViewModelBase : CkgBaseViewModel
    {
        public List<string> GetVertragsarten()
        {
            var vertragsArten = new List<string>();

            var myLogonContext = (LogonContext as LogonContextDataServiceDadServices);
            if (myLogonContext != null)
            {
                if ((myLogonContext.Organization != null) && (!String.IsNullOrEmpty(myLogonContext.Organization.OrganizationName)))
                {
                    var vArten = myLogonContext.Organization.OrganizationName.Split('+');
                    Array.Sort(vArten);
                    foreach (var vArt in vArten)
                    {
                        vertragsArten.Add(vArt.Trim());
                    }
                }
            }

            return vertragsArten;
        }
    }
}
