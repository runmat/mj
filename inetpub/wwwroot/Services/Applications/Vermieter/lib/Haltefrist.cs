using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class Haltefrist : BankBase
    {


        #region " Declarations"
            private DataTable m_tblBestand;
            private string m_strFilename2;
        #endregion


        public DataTable Bestand
        {
            get { return m_tblBestand; }
            set { m_tblBestand = value; }
        }




        public Haltefrist(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_strFilename2 = strFilename;

	    }

        public override void Change()
        {

        }

        public override void Show()
        {

        }



        public void Change(System.Web.UI.Page page)
        {
            m_intStatus = 0;
            m_strMessage = "";

            DataView tmpDataView = null;
            tmpDataView = Bestand.DefaultView;

            tmpDataView.RowFilter = "ActionNOTHING = 0";

            Int32 i = default(Int32);

            for (i = 0; i <= tmpDataView.Count - 1; i++)
            {
               
                string strBemerkung = "Haltefrist ignoriert";
                
                string strActionText = "Löschen";
                try
                {

                    string strKUNNR = m_objUser.KUNNR.PadLeft(10, '0');
                    string strKUNPDI = Convert.ToString(tmpDataView[i]["KUNPDI"]);
                    string strZZFAHRG = Convert.ToString(tmpDataView[i]["ZZFAHRG"]);
                    string strZZKENN = Convert.ToString(tmpDataView[i]["ZZKENN"]);

                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Abmbereit_Laufaen",ref m_objApp,ref m_objUser,ref page);

                    myProxy.setImportParameter("KUNNR", strKUNNR);
                    myProxy.setImportParameter("ZZKENN", strZZKENN);
                    myProxy.setImportParameter("ZZFAHRG", strZZFAHRG);
                    myProxy.setImportParameter("KUNPDI", strKUNPDI);

                    myProxy.callBapi();

                }
                catch (Exception ex)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge";
                    strBemerkung = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                }

                Bestand.AcceptChanges();
                DataRow[] tmpRows = null;
                tmpRows = Bestand.Select("ZZFAHRG = '" + Convert.ToString(tmpDataView[i]["ZZFAHRG"]) + "'");
                if (tmpRows.Length > 0)
                {
                    tmpRows[0].BeginEdit();
                    tmpRows[0]["Action"] = strActionText;
                    tmpRows[0]["Bemerkung"] = strBemerkung;
                    tmpRows[0].EndEdit();
                    Bestand.AcceptChanges();
                }
            }
        }











        public void GetData(System.Web.UI.Page page, string strAppID, string strSessionID)
        {
            m_strClassAndMethod = "Haltefrist.GetData";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ABMBEREIT_LAUFZEIT", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                m_tblBestand = myProxy.getExportTable("AUSGABE");

                m_tblBestand.Columns.Add("ActionNOTHING", typeof(System.Boolean));
                m_tblBestand.Columns.Add("ActionDELE", typeof(System.Boolean));
                m_tblBestand.Columns.Add("Bemerkung", typeof(System.String));
                m_tblBestand.Columns.Add("Action", typeof(System.String));

                m_tblBestand.AcceptChanges();


                foreach (DataRow dr in m_tblBestand.Rows)
                {
                    dr["ActionNOTHING"] = true;
                    dr["ActionDELE"] = false;
                }

                m_tblBestand.AcceptChanges();



                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblBestand);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblBestand);

            }

        }




    }
}
