using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class Versandbeauftragungen : DatenimportBase
    {

        #region "Declarations"
        private Int16 m_EquiTyp = 0;
        private DataTable m_tblCustomer;
        private string m_Kunde;
        #endregion

        #region Properties
        public Int16 Typ
        {
            get { return m_EquiTyp; }
            set { m_EquiTyp = value; }
        }
        public DataTable tblCustomer { get { return m_tblCustomer; } }
        public string Kunde
        {
            get { return m_Kunde; }
            set { m_Kunde = value; }
        }
        #endregion


        public Versandbeauftragungen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)

            : base(ref objUser, objApp, strFilename)
        {
        }


        public void GetVersandbeauftragungen(String strAppID, String strSessionID, System.Web.UI.Page page, Boolean Beauftragte, Boolean FlagVers, Boolean FlagVersSperr)
        {
            m_strClassAndMethod = "Versandbeauftragungen.GetVersandbeauftragungen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                if (Beauftragte)
                {
                    myProxy.setImportParameter("I_BEAUFTR", "X");
                }

                if (FlagVers)
                {
                    myProxy.setImportParameter("FLAG_VERS", "X");
                }

                if (FlagVersSperr)
                {
                    myProxy.setImportParameter("FLAG_VERS_SPERR", "X");
                }

                myProxy.callBapi();

                DataTable TempTable;

                TempTable = myProxy.getExportTable("GT_WEB");

                TempTable.Columns.Add("EQTYP", typeof(System.String));

                TempTable.AcceptChanges();

                if (Typ == 1)//Nur Briefe
                {
                    TempTable.DefaultView.RowFilter = "ZZBRFVERS <> 0";
                    TempTable = TempTable.DefaultView.ToTable();
                }

                if (Typ == 2)//Nur Schlüssel
                {
                    TempTable.DefaultView.RowFilter = "ZZSCHLVERS <> 0";
                    TempTable = TempTable.DefaultView.ToTable();
                }

                TempTable.Columns["ZZBRFVERS"].MaxLength = 15;
                TempTable.Columns["ZZSCHLVERS"].MaxLength = 15;
                TempTable.Columns["ZZNAME1_ZS"].MaxLength = 120;

                TempTable.AcceptChanges();

                foreach (DataRow Row in TempTable.Rows)
                {
                    if (Row["ZZBRFVERS"].ToString().Trim() == "1")
                    {
                        Row["EQTYP"] = "Brief";
                    }

                    if (Row["ZZSCHLVERS"].ToString().Trim() == "1")
                    {
                        Row["EQTYP"] = "Schlüssel";
                    }

                    Row["ZZNAME1_ZS"] = Row["ZZNAME1_ZS"] + "<br>" + Row["ZZSTRAS_ZS"] + "," + Row["ZZPSTLZ_ZS"] + "&nbsp;" + Row["ZZORT01_ZS"];
                }

                TempTable.AcceptChanges();

                CreateOutPut(TempTable, strAppID);

                m_tblResult.Columns.Add("Delete", typeof(System.Boolean));
                m_tblResult.Columns.Add("DeleteEnable", typeof(System.Boolean));
                m_tblResult.Columns.Add("Status", typeof(System.String));

                foreach (DataRow row in m_tblResult.Rows)
                {
                    row["Delete"] = false;
                    row["DeleteEnable"] = true;
                }

                m_tblResult.AcceptChanges();

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";
            }
        }


        public void GetVersandbeauftragungenTreugeber(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandbeauftragungen.GetVersandbeauftragungenTreugeber";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_Kunde.PadLeft(10, '0'));
                myProxy.setImportParameter("I_TREUGEBER_VERS", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_BEAUFTR", "X");

                myProxy.callBapi();

                DataTable TempTable;

                TempTable = myProxy.getExportTable("GT_WEB");

                TempTable.Columns.Add("EQTYP", typeof(System.String));

                TempTable.AcceptChanges();

                //Nur Briefe
                TempTable.DefaultView.RowFilter = "ZZBRFVERS <> 0";
                TempTable = TempTable.DefaultView.ToTable();

                TempTable.Columns["ZZBRFVERS"].MaxLength = 15;
                TempTable.Columns["ZZSCHLVERS"].MaxLength = 15;
                TempTable.Columns["ZZNAME1_ZS"].MaxLength = 120;

                TempTable.AcceptChanges();

                foreach (DataRow Row in TempTable.Rows)
                {
                    Row["EQTYP"] = "Brief";

                    Row["ZZNAME1_ZS"] = Row["ZZNAME1_ZS"] + "<br>" + Row["ZZSTRAS_ZS"] + "," + Row["ZZPSTLZ_ZS"] + "&nbsp;" + Row["ZZORT01_ZS"];
                }

                TempTable.AcceptChanges();

                CreateOutPut(TempTable, strAppID);

                m_tblResult.Columns.Add("Delete", typeof(System.Boolean));
                m_tblResult.Columns.Add("DeleteEnable", typeof(System.Boolean));
                m_tblResult.Columns.Add("Status", typeof(System.String));

                foreach (DataRow row in m_tblResult.Rows)
                {
                    row["Delete"] = false;
                    row["DeleteEnable"] = true;
                }

                m_tblResult.AcceptChanges();

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";
            }
        }


        public void Delete(string strAppID, string strSessionID, System.Web.UI.Page page, string kennz, string fahrg, string brief, string schluess, string komponentennummer, string anforderungsnummer, bool treugeber = false)
        {
            m_strClassAndMethod = "Versandbeauftragungen.Delete";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            DataTable Dummy = new DataTable();

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                string strKUNNR;
                if (treugeber)
                {
                    strKUNNR = m_Kunde.PadLeft(10, '0');
                }
                else
                {
                    strKUNNR = m_objUser.KUNNR.PadLeft(10, '0');
                }
                
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE_DEL", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_KUNNR", strKUNNR);
                    myProxy.setImportParameter("I_LICENSE_NUM", kennz);
                    myProxy.setImportParameter("I_CHASSIS_NUM", fahrg);
                    myProxy.setImportParameter("I_ZZBRFVERS", brief);
                    myProxy.setImportParameter("I_ZZSCHLVERS", schluess);
                    myProxy.setImportParameter("I_IDNRK", komponentennummer);
                    myProxy.setImportParameter("I_ZANF_NR", anforderungsnummer);
                    myProxy.callBapi();

                }
                catch (Exception ex)
                {
                    m_intStatus = -9999;

                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DELETE":
                            m_strMessage = "Datensatz konnte nicht gelöscht werden.";
                            break;
                        case "NO_DATA":
                            m_strMessage = "Datensatz nicht gefunden.";
                            break;
                        default:
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten: " + ex.Message;
                            break;
                    }
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref Dummy, false);

                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }


        public void GetCustomer(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandbeauftragungen.GetCustomer";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_GET_TREUH_AG", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "B");
                myProxy.callBapi();

                m_tblCustomer = myProxy.getExportTable("GT_EXP");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblCustomer);
            }
            catch
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblCustomer);
            }
        }

    }


}
