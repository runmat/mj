using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace Leasing.lib
{

    public class Fahrzeugdaten : CKG.Base.Business.DatenimportBase
    {

        public String Fahrgestellnummer { get; set; }
        public String Vertragsnummer { get; set; }
        public DataTable UploadTable { get; set; }

        public Fahrzeugdaten(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
            { }

        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Fahrzeugdaten.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_EXPORT_EMIKL_HALTER_LP", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("EQTYP", "B");

                if (UploadTable != null && UploadTable.Rows.Count > 0)
                {
                    DataTable ImportTable = myProxy.getImportTable("GT_WEB");
                    DataRow NewRow;

                    foreach (DataRow dr in UploadTable.Rows)
                    {
                        NewRow = ImportTable.NewRow();

                        NewRow["CHASSIS_NUM"] = dr[0];
                        if (UploadTable.Columns.Count > 1)
                        {
                            NewRow["LIZNR"] = dr[1];
                        }
                        if (UploadTable.Columns.Count > 2)
                        {
                            NewRow["KD_EMIKL"] = dr[2];

                        }

                        ImportTable.Rows.Add(NewRow);

                    }


                }
               

                myProxy.callBapi();


                ResultTable = myProxy.getExportTable("GT_WEB");

 
            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Fahrzeuge gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }

        }



    }
}