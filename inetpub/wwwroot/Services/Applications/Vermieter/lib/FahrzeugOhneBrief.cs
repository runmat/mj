using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class FahrzeugOhneBrief : DatenimportBase
    {
        #region " Declarations"
            private DataTable m_tblResultPDIs;
        #endregion

        #region " Properties"
            public DataTable ResultPDIs
                {
                    get { return m_tblResultPDIs; }
                }
        #endregion

            public FahrzeugOhneBrief(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            
                : base(ref objUser, objApp, strFilename)
            {
            }


            public void FILL(string strAppID, string strSessionID, string strPDI, DateTime datEingangsdatumVon, DateTime datEingangsdatumBis, string strFahrgestellnummer, string strModell,System.Web.UI.Page page)
            {
                m_strClassAndMethod = "FahrzeugOhneBrief.FILL";
                m_strAppID = strAppID;
                m_strSessionID = strSessionID;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Ec_Avm_Fzg_Ohne_Brief",ref m_objApp, ref m_objUser,ref page);

                    myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10,'0'));
                    myProxy.setImportParameter("I_PDI", strPDI);
                    myProxy.setImportParameter("I_ZZDAT_EIN_VON", datEingangsdatumVon.ToShortDateString());
                    myProxy.setImportParameter("I_ZZDAT_EIN_BIS", datEingangsdatumBis.ToShortDateString());
                    myProxy.setImportParameter("I_ZZFAHRG", strFahrgestellnummer);
                    myProxy.setImportParameter("I_ZMODELL", strModell);

                    myProxy.callBapi();

                    m_tblResultPDIs = new DataTable();
                    m_tblResultPDIs.Columns.Add("PDI Nummer", System.Type.GetType("System.String"));
                    m_tblResultPDIs.Columns.Add("PDI Name", System.Type.GetType("System.String"));
                    m_tblResultPDIs.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"));

                    DataColumn[] keys = new DataColumn[2];
                    keys[0] = m_tblResultPDIs.Columns[0];

                    m_tblResultPDIs.PrimaryKey = keys;


                    m_tblResult = new DataTable();

                    DataTable tblTemp2 = myProxy.getExportTable("GT_WEB");
                    DataView dv = null;

                    dv = tblTemp2.DefaultView;

                    dv.Sort = "ZZCARPORT, ZZHERST, ZZMODELL, ZZDAT_EIN,ZZFAHRG";

                    DataRow row = null;
                    DataRow PdiRow = null;
                    DataRow FindRow = null;
                    Int32 e = default(Int32);

                    row = tblTemp2.NewRow();


                    DataTable tblSortDetails = null;


                    tblSortDetails = tblTemp2.Clone();

                    dv.Sort = "ZZCARPORT asc, ZZHERST asc, ZZMODELL asc, ZZDAT_EIN asc ,ZZFAHRG asc";


                    e = 0;

                    while (e < dv.Count)
                    {
                        row = tblSortDetails.NewRow();

                        row[0] = dv[e]["ZZCARPORT"];
                        row[1] = dv[e]["ZZHERST"];
                        row[2] = dv[e]["ZKLTXT"];
                        row[3] = dv[e]["ZZMODELL"];
                        row[4] = dv[e]["ZZBEZEI"];
                        row[5] = dv[e]["ZZFAHRG"];
                        row[6] = dv[e]["ZZDAT_EIN"];
                        row[7] = dv[e]["ZZDAT_BER"];
                        row[8] = dv[e]["ZNAME1"];

                        tblSortDetails.Rows.Add(row);

                        e = e + 1;
                    }


                    foreach (DataRow row_loopVariable in tblSortDetails.Rows)
                    {
                        row = row_loopVariable;
                        PdiRow = m_tblResultPDIs.NewRow();

                        FindRow = m_tblResultPDIs.Rows.Find(row[0]);

                        if (FindRow == null == true)
                        {
                            PdiRow[0] = row[0];
                            PdiRow[1] = row[8];
                            PdiRow[2] = 1;

                            m_tblResultPDIs.Rows.Add(PdiRow);
                        }
                        else
                        {
                            FindRow.BeginEdit();
                            FindRow[2] = Convert.ToInt32(FindRow[2]) + 1;
                            FindRow.EndEdit();
                        }
                    }


                    tblTemp2 = null;

                    m_tblResult = tblSortDetails;

                    CreateOutPut(m_tblResult, strAppID);

                    WriteLogEntry(true, "ZZDAT_EIN_VON=" + datEingangsdatumVon.ToShortDateString() + ", ZZDAT_EIN_BIS=" + datEingangsdatumBis.ToShortDateString() + ", KUNNR=" + m_objUser.KUNNR + ", ZZKUNPDI=" + strPDI + ", ZZFAHRG=" + strFahrgestellnummer + ", ZZBEZEI=" + strModell,ref m_tblResult, false);
                }
                catch (Exception ex)
                {
                    m_intStatus = -4444;
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_strMessage = "Keine Daten gefunden.";
                            break;
                        default:
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                    WriteLogEntry(false, "ZZDAT_EIN_VON=" + datEingangsdatumVon.ToShortDateString() + ", ZZDAT_EIN_BIS=" + datEingangsdatumBis.ToShortDateString() + ", KUNNR=" + m_objUser.KUNNR + ", ZZKUNPDI=" + strPDI + ", ZZFAHRG=" + strFahrgestellnummer + ", ZZBEZEI=" + strModell + ", " + m_strMessage.Replace("<br>", " "),ref m_tblResult, false);
                }
            }









    }
}
