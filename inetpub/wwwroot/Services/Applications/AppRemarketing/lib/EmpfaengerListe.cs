using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CKG.Base.Kernel.Security;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Data;
using System.Web.UI.WebControls;

namespace AppRemarketing.lib
{
    public class EmpfaengerListe
    {
        const string BapiName = "Z_DPM_REM_READ_EMPFAENGER";

        public void FillVermieter(User user, App app, Page page, ListControl control)
        {
            var items = CallBapi(user, app, page, "VERMIETER").ToList();
            FillListControl(control, items, "- alle -", true);
        }

        public void FillHereinnahmecenter(User user, App app, Page page, ListControl control)
        {
            var items = CallBapi(user, app, page, "HC").ToList();
            FillListControl(control, items, "- alle -", true);
        }


        internal void fillKlasse(User user, App app, Page page, ListControl crtl1, ListControl crtl2, string art)
        {
            var items = CallBapiDekramerkmale(user, app, page, art).ToList();


            FillListControl(crtl1, items, string.Empty, true);

            if (crtl2!= null)
            {
                FillListControl(crtl2, items, string.Empty, true);
            }
        }



        private void FillListControl(ListControl control, List<EmpfaengerListe.Eintrag> list, string firstItemText ,bool AddFirstItem )
        {
            control.Items.Clear();
            if (AddFirstItem)
            {
                //control.Items.Add(new ListItem("- alle -", "00"));
                control.Items.Add(new ListItem(firstItemText, "00"));
            }

            foreach (var item in list)
            {
                control.Items.Add(new ListItem(item.Kuerzel + " - " + item.Name, item.Kuerzel));
            }

        }

       

        private IEnumerable<Eintrag> CallBapi(User user, App app, Page page, string kennung)
        {
            DataTable result;
            try
            {
                var localUser = user;
                var localApp = app;

                var myProxy = DynSapProxy.getProxy(BapiName, ref localApp, ref localUser, ref page);

                myProxy.setImportParameter("I_KUNNR", localUser.KUNNR.PadLeft(10, '0'));//KUNNR
                myProxy.setImportParameter("I_KENNUNG", kennung);

                myProxy.callBapi();

                result = myProxy.getExportTable("GT_OUT");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Beim Erstellen des Reportes ist ein Fehler aufgetreten.\n(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                //WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }

            foreach (DataRow row in result.Rows)
            {
                var kurz = (string)row["POS_KURZTEXT"];
                var name = (string)row["NAME1"];
                yield return new Eintrag(kurz, name);
            }
        }

        private IEnumerable<Eintrag> CallBapiDekramerkmale(User user, App app, Page page, string art)
        {
            DataTable result;
            try
            {
                string BapiName = "Z_DPM_REM_READ_DEKRAMERKMALE";
                var localUser = user;
                var localApp = app;

                var myProxy = DynSapProxy.getProxy(BapiName, ref localApp, ref localUser, ref page);

                myProxy.setImportParameter("I_KUNNR", localUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ART", art);

                myProxy.callBapi();

                result = myProxy.getExportTable("GT_OUT");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Beim Erstellen des Reportes ist ein Fehler aufgetreten.\n(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                //WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }

            foreach (DataRow row in result.Rows)
            {
                var nr = (string)row["NR"];
                var text = (string)row["TEXT"];
                yield return new Eintrag(nr, text);
            }
        }

        public class Eintrag
        {
            public Eintrag(string kuerzel, string name)
            {
                Kuerzel = kuerzel;
                Name = name;
            }

            public string Kuerzel { get; private set; }
            public string Name { get; private set; }
        }

    }
}