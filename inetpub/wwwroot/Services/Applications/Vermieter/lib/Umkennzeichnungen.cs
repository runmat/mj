using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class Umkennzeichnungen : DatenimportBase
    {

        #region "Declarations"
        string m_EquiTyp = "016";

        string m_CHASSIS_NUM = "";
        string m_LIZNR = "";
        string m_VDATU_VON = "";
        string m_VDATU_BIS = "";
        string m_NUR_OFFENE_UK = "";
        string m_NUR_KLAERFAELLE = "";


        DataTable m_ResultTable = new DataTable();
        #endregion

        #region Properties
        public string Typ
        {
            get { return m_EquiTyp; }
            set { m_EquiTyp = value; }
        }
        public DataTable ResultTable
        {
            get { return m_ResultTable; }
            set { m_ResultTable = value; }
        }

        public string I_CHASSIS_NUM
        {
            get { return m_CHASSIS_NUM; }
            set { m_CHASSIS_NUM = value; }
        }
        public string I_LIZNR
        {
            get { return m_LIZNR; }
            set { m_LIZNR = value; }
        }
        public string I_VDATU_VON
        {
            get { return m_VDATU_VON; }
            set { m_VDATU_VON = value; }
        }
        public string I_VDATU_BIS
        {
            get { return m_VDATU_BIS; }
            set { m_VDATU_BIS = value; }
        }
        public string I_NUR_OFFENE_UK
        {
            get { return m_NUR_OFFENE_UK; }
            set { m_NUR_OFFENE_UK = value; }
        }
        public string I_NUR_KLAERFAELLE
        {
            get { return m_NUR_KLAERFAELLE; }
            set { m_NUR_KLAERFAELLE = value; }
        }


        #endregion


        public Umkennzeichnungen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        {
        }


        public void GetUmkennzeichnungen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Umkennzeichnungen.GetUmkennzeichnungen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                //DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_DEZDIENSTL_001", ref m_objApp, ref m_objUser, ref page);
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_AUFTRAGSDAT_01", ref m_objApp, ref m_objUser, ref page);




                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                myProxy.setImportParameter("I_AUGRU", Typ);

                myProxy.setImportParameter("I_CHASSIS_NUM", I_CHASSIS_NUM);
                myProxy.setImportParameter("I_LIZNR", I_LIZNR);
                myProxy.setImportParameter("I_VDATU_VON", I_VDATU_VON);
                myProxy.setImportParameter("I_VDATU_BIS", I_VDATU_BIS);

                myProxy.setImportParameter("I_NUR_OFFENE_UK", I_NUR_OFFENE_UK);
                myProxy.setImportParameter("I_NUR_KLAERFAELLE", I_NUR_KLAERFAELLE);


                myProxy.callBapi();

                DataTable TempTable;

                //TempTable = myProxy.getExportTable("GT_WEB");
                TempTable = myProxy.getExportTable("GT_OUT");

                TempTable.Columns.Add("EQTYP", typeof(System.String));
                TempTable.Columns.Add("AUGRU", typeof(System.String));

                TempTable.AcceptChanges();

                //if (Typ == "019")//Umkennzeichnungen
                //{
                //    TempTable.DefaultView.RowFilter = "ZZBRFVERS <> 0";

                //    TempTable = TempTable.DefaultView.ToTable();

                //}
                //else //Sonstige Aufträge
                //{
                //    TempTable.DefaultView.RowFilter = "ZZSCHLVERS <> 0";

                //    TempTable = TempTable.DefaultView.ToTable();

                //}


                //TempTable.Columns["ZZBRFVERS"].MaxLength = 15;
                //TempTable.Columns["ZZSCHLVERS"].MaxLength = 15;
                //TempTable.Columns["ZZNAME1_ZS"].MaxLength = 120;

                TempTable.AcceptChanges();

                foreach (DataRow Row in TempTable.Rows)
                {
                //    if (Row["ZZBRFVERS"].ToString().Trim() == "1")
                //    {
                //        Row["EQTYP"] = "Brief";
                //    }

                //    if (Row["ZZSCHLVERS"].ToString().Trim() == "1")
                //    {
                //        Row["EQTYP"] = "Schlüssel";
                //    }

                //    Row["ZZNAME1_ZS"] = Row["ZZNAME1_ZS"] + "<br>" + Row["ZZSTRAS_ZS"] + "," + Row["ZZPSTLZ_ZS"] + "&nbsp;" + Row["ZZORT01_ZS"];
                    Row["AUGRU"] = Typ;
                }

                //TempTable.AcceptChanges();

                //CreateOutPut(TempTable, strAppID);
                m_ResultTable = TempTable;

                //m_tblResult.Columns.Add("Delete", typeof(System.Boolean));
                //m_tblResult.Columns.Add("DeleteEnable", typeof(System.Boolean));
                //m_tblResult.Columns.Add("Status", typeof(System.String));

                //foreach (DataRow row in m_tblResult.Rows)
                //{
                //    row["Delete"] = false;
                //    row["DeleteEnable"] = true;
                //}

                //m_tblResult.AcceptChanges();

                //                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }




        }


        public void Delete(string strAppID, string strSessionID, System.Web.UI.Page page, string kennz, string fahrg, string brief, string schluess, string komponentennummer, string anforderungsnummer)
        {
            m_strClassAndMethod = "Versandbeauftragungen.Delete";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            DataTable Dummy = new DataTable();

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;


                string strKUNNR = m_objUser.KUNNR.PadLeft(10, '0');


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
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.";
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

    }


}
