using System.Configuration;
using CKG.Base.Kernel.Security;
using MvcAppZulassungsdienst.Models;
using MvcAppZulassungsdienst.SAP;

namespace MvcAppZulassungsdienst.ViewModels
{
    public class BeauftragungViewModel
    {
        private Beauftragung _beauftragungsdaten;
        public Beauftragung Beauftragungsdaten
        {
            get
            {
                if (_beauftragungsdaten == null)
                {
                    var user = new User(UserID, ConfigurationManager.AppSettings["Connectionstring"]);
                    var app = new App(user);
                    var form = new System.Web.UI.Page();
                    form.Session["AppId"] = "AppZulassungsdienst.Beauftragung";

                    _beauftragungsdaten = new Beauftragung(user, app, form.Session["AppId"].ToString(), form.Session.SessionID, "");
                }

                return _beauftragungsdaten;
            }
            set { _beauftragungsdaten = value; } 
        }

        public int UserID { get; set; }
    }
}