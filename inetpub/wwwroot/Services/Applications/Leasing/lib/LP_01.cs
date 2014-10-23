using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;

namespace Leasing.lib
{
    public class LP_01 : CKG.Base.Business.DatenimportBase
    {

        #region "Declarations"

        String m_Fahrgestellnummer;
        String m_Kennzeichen;
        String m_Leasingvertragsnummer;
        String m_Leasingnehmer;
        String m_AuftragsnrRef3;
        String m_Action;
        String m_KUNNR_WL;

        DataTable m_Fahrzeuge;
        DataTable SapExportTable;
        DataTable mSapImportTable;

        #endregion

        #region "Properties"

        public DataTable SapImportTable
        {
            get { return mSapImportTable; }
            set { mSapImportTable = value; }
        }


        public String Fahrgestellnummer
        {
            get { return m_Fahrgestellnummer; }
            set { m_Fahrgestellnummer = value; }
        }

        public String Kennzeichen
        {
            get { return m_Kennzeichen; }
            set { m_Kennzeichen = value; }
        }


        public String Leasingvertragsnummer
        {
            get { return m_Leasingvertragsnummer; }
            set { m_Leasingvertragsnummer = value; }
        }

        public String Leasingnehmer
        {
            get { return m_Leasingnehmer; }
            set { m_Leasingnehmer = value; }
        }

        public String AuftragsnrRef3
        {
            get { return m_AuftragsnrRef3; }
            set { m_AuftragsnrRef3 = value; }
        }

        public String Aktion
        {
            get { return m_Action; }
            set { m_Action = value; }
        }
        public String KUNNR_WL
        {
            get { return m_KUNNR_WL; }
            set { m_KUNNR_WL = value; }
        }

        public DataTable Fahrzeuge
        {
            get { return m_Fahrzeuge; }
            set { m_Fahrzeuge = value; }
        }

        public string DatumVon { get; set; }
        public string DatumBis { get; set; }
        public string Haendlernummer { get; set; }
        public string Vertragsnummer { get; set; }
        public string ABCKennzeichen { get; set; }
        public string Equinr { get; set; }
        public string KunnrZL { get; set; }
        public string Haltername { get; set; }
        public string Vollst { get; set; }
        public string EVBNr { get; set; }
        public string KunnrSAP { get; set; }
        public bool IstGeloescht { get; set; }

        public DataTable Adressen { get; set; }
        public DataTable VertragTable { get; set; }

        #endregion

        public LP_01(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void GiveCars(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.GiveCars";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Offene_Liefertermine_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_REF2_WL", m_objUser.Reference);
                myProxy.setImportParameter("I_LIZNR", m_Leasingvertragsnummer);
                myProxy.setImportParameter("I_REF3", m_AuftragsnrRef3);
                myProxy.setImportParameter("I_NAME1_KUNDE_LP", m_Leasingnehmer);
                myProxy.setImportParameter("I_ACTION", m_Action);


                myProxy.callBapi();

                m_Fahrzeuge = myProxy.getExportTable("GT_WEB");


                m_Fahrzeuge.Columns.Add("DATLWWEEK", typeof(System.String));
                m_Fahrzeuge.Columns.Add("DATLWYEAR", typeof(System.String));
                m_Fahrzeuge.Columns.Add("LIEFERTERMIN", typeof(System.String));
                m_Fahrzeuge.Columns.Add("STATUS", typeof(System.String));
                m_Fahrzeuge.Columns.Add("LWCONFIRM", typeof(System.Boolean));

                foreach (DataRow dRow in m_Fahrzeuge.Rows)
                {
                    KUNNR_WL = dRow["Kunnr_Wl"].ToString();

                    if (dRow["Lief_Avis"].ToString() != "000000")
                    {
                        if (dRow["Dat_Lw"].ToString().Length >= 6)
                        {
                            dRow["LIEF_AVIS"] = dRow["Lief_Avis"].ToString().Substring(dRow["Lief_Avis"].ToString().Length - 2) +
                                  "-" + dRow["Dat_Lw"].ToString().Substring(0, 4);
                        }
                        else
                        {
                            dRow["LIEF_AVIS"] = dRow["Lief_Avis"].ToString().Substring(dRow["Lief_Avis"].ToString().Length - 2) +
                                      "-" + System.DateTime.Now.Year.ToString();
                        }
                    }

                    if (dRow["Lief_Avis"].ToString() != "000000")
                    {
                        if (dRow["Dat_Lw"].ToString().Length >= 6)
                        {
                            dRow["DATLWWEEK"] = dRow["Dat_Lw"].ToString().Substring(dRow["Dat_Lw"].ToString().Length - 2);
                            dRow["DATLWYEAR"] = dRow["Dat_Lw"].ToString().Substring(0, 4);
                        }
                        else
                        {
                            dRow["DATLWWEEK"] = "";
                            dRow["DATLWYEAR"] = "";
                        }
                    }
                    else
                    {
                        dRow["DATLWYEAR"] = System.DateTime.Now.Year;
                    }
                    dRow["LWCONFIRM"] = false;

                    Linebreaker(dRow, "NAME1_KUNDE_LP", 20);
                    Linebreaker(dRow, "NAME1_FAHRER", 20);

                }

                CreateOutPut(m_Fahrzeuge, strAppID);
                m_Fahrzeuge = Result.Copy();
                mSapImportTable = new DataTable();
                mSapImportTable.Columns.Add("LIZNR", typeof(System.String));
                mSapImportTable.Columns.Add("DAT_LW", typeof(System.String));
                mSapImportTable.Columns.Add("DAT_LT", typeof(System.String));
                mSapImportTable.Columns.Add("REF3_HERST", typeof(System.String));

                WriteLogEntry(true, "LVNr.=" + m_Leasingvertragsnummer + ", KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;

                        break;
                }


            }

        }

        public void GiveBriefversand(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.GiveBriefversand";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Versbriefe", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_DATVON", DatumVon);
                myProxy.setImportParameter("I_DATBIS", DatumBis);
                myProxy.setImportParameter("I_VERSART", ABCKennzeichen);

                myProxy.callBapi();


                DataTable dt = myProxy.getExportTable("T_RETURN");


                dt.Columns.Add("Adresse");
                dt.AcceptChanges();



                if (dt.Rows.Count > 0)
                {
                    DataRow row = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = dr;
                        string strTemp = row["NAME1"].ToString() + ", " + row["NAME2"].ToString() + ", " + row["STRAS"].ToString() + " " + row["HNR"].ToString() + ", " + row["PLZ"].ToString() + " " + row["ORT"].ToString();
                        row["ADRESSE"] = strTemp.Replace(", ,  ,  ", "");



                        if (row["VERSART"].ToString() == "1")
                        {
                            row["VERSART"] = "Temporär";
                        }
                        else
                        {
                            row["VERSART"] = "Endgültig";
                        }
                    }
                }


                CreateOutPut(dt, strAppID);

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Es wurden für den angegebenen Zeitraum keine Daten gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;

                        break;
                }


            }

        }

        public void Linebreaker(DataRow Row, string ColumnName, int ZeichenBreite)
        {

            if (Convert.ToString(Row[ColumnName]).Length > ZeichenBreite)
            {
                string strIn = Convert.ToString(Row[ColumnName]);
                string strOut = "";

                string[] strArray = strIn.Split(' ');
                string strtemp = "";
                int iLängeStr = 0;
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    if (iLängeStr + strArray[i].Length <= ZeichenBreite)
                    {
                        strtemp += strArray[i] + " ";
                        iLängeStr += strArray[i].Length + 1;
                    }
                    else
                    {
                        if (strArray[i].Length >= ZeichenBreite)
                        {
                            strOut += strtemp + "<br />" + strArray[i] + "<br />";
                            iLängeStr = 0;
                            strtemp = "";
                        }
                        else
                        {
                            strOut += strtemp + "<br />";
                            iLängeStr = strArray[i].Length + 1;
                            strtemp = strArray[i] + " ";
                        }
                    }
                }
                if (strtemp.Length > 0)
                {
                    strOut += strtemp;
                }

                Row[ColumnName] = strOut;
            }
        }

        public void SaveLiefertermin(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.SaveLiefertermin";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Liefertermine_Setzen_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KUNNR_WL", m_KUNNR_WL);
                String sUserName = m_objUser.UserName;
                if (sUserName.Length > 12)
                {
                    myProxy.setImportParameter("I_UNAME", sUserName.Substring(0, 11));

                }
                else
                {
                    myProxy.setImportParameter("I_UNAME", sUserName);
                }
                DataTable tmptable = myProxy.getImportTable("GT_WEB");


                DataRow tmpSAPRow = tmptable.NewRow();

                foreach (DataRow row in mSapImportTable.Rows)
                {
                    DataRow newRow = tmptable.NewRow();
                    newRow["LIZNR"] = row["LIZNR"];
                    newRow["DAT_LW"] = row["DAT_LW"];
                    newRow["DAT_LT"] = row["DAT_LT"];
                    newRow["REF3_HERST"] = row["REF3_HERST"];
                    tmptable.Rows.Add(newRow);
                }

                myProxy.callBapi();

                SapExportTable = myProxy.getExportTable("GT_WEB");

                String strStatus = "";
                foreach (DataRow SapExportRow in SapExportTable.Rows)
                {
                    DataRow FahrzeugeRow;
                    FahrzeugeRow = m_Fahrzeuge.Select("Leasingnummer='" + SapExportRow["Liznr"].ToString() + "'")[0];

                    switch (SapExportRow["Zreturn"].ToString())
                    {
                        case "0":
                            strStatus = "Vorgang gespeichert.";
                            break;
                        case "1":
                            strStatus = "Vertrag existiert nicht.";
                            break;
                        case "2":
                            strStatus = "Liefertermin bereits gesetzt.";
                            break;
                        case "3":
                            strStatus = "Fehler bei Update.";
                            break;
                        case "4":
                            strStatus = "Vorgang gespeichert.";
                            break;
                        case "5":
                            strStatus = "Objektnummer ist leer.";
                            break;
                        default:
                            strStatus = "Fehler.";
                            break;
                    }


                    if (SapExportRow["Dat_Lw"].ToString() != "000000")
                    {
                        FahrzeugeRow["LWWoche"] = SapExportRow["Dat_Lw"].ToString().Substring(SapExportRow["Dat_Lw"].ToString().Length - 2);
                        FahrzeugeRow["LWJahr"] = SapExportRow["Dat_Lw"].ToString().Substring(0, 4);
                    }
                    else
                    {
                        FahrzeugeRow["LWWoche"] = "";
                        FahrzeugeRow["LWJahr"] = System.DateTime.Now.Year;
                    }
                    if (SapExportRow["Dat_Lt"].ToString() != "")
                    {
                        FahrzeugeRow["LIEFERTERMIN"] = Convert.ToDateTime(SapExportRow["Dat_Lt"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        FahrzeugeRow["LIEFERTERMIN"] = DBNull.Value;
                    }
                    FahrzeugeRow["NUMMERINTERN"] = SapExportRow["Ref3_Herst"].ToString();

                    FahrzeugeRow["STATUS"] = strStatus;
                    m_Fahrzeuge.AcceptChanges();
                    SapExportTable = null;
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -3331;
                        m_strMessage = "Keine Daten gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Speichern der Daten: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }


            }

        }

        public void FillArval(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.FillArval";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_UNZUGELASSENE_FZGE_ARVAL", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                m_tblResult = myProxy.getExportTable("T_DATA");



            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Es wurden keine Daten gefunden.";
                        break;
                    case "ERR_INV_AG":
                        m_intStatus = -2222;
                        m_strMessage = "Ungültiger Auftraggeber.";
                        break;
                    default:
                        m_intStatus = -9999;

                        break;
                }
            }

        }

        public void FillZuFahrzeuge(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.FillZuFahrzeuge";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ZUGELASSENE_FZGE_ARVAL", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_ZF", Haendlernummer);
                myProxy.setImportParameter("I_LVNR", Vertragsnummer);
                myProxy.setImportParameter("I_VDATU_VON", DatumVon);
                myProxy.setImportParameter("I_VDATU_BIS", DatumBis);


                myProxy.callBapi();

                m_tblResult = myProxy.getExportTable("ITAB");



            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Es wurden keine Daten gefunden.";
                        break;
                    case "ERR_INV_AG":
                        m_intStatus = -2222;
                        m_strMessage = "Ungültiger Auftraggeber.";
                        break;
                    case "ERR_INV_ZF":
                        m_intStatus = -3333;
                        m_strMessage = "Ungültiger Händler.";
                        break;
                    default:
                        m_intStatus = -9999;

                        break;
                }
            }

        }


        public void FillVertragsdaten(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.FillVertragsdaten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_EQUI_001", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_LIZNR", Vertragsnummer);
                myProxy.setImportParameter("I_LICENSE_NUM", Kennzeichen);
                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer);


                myProxy.callBapi();

                VertragTable = myProxy.getExportTable("GT_OUT");



                //Keine Vertragsdaten gefunden - aussteigen.

                if (VertragTable.Rows.Count < 1)
                {
                    throw new Exception("ERR_NO_DATA");
                }



                Adressen = myProxy.getExportTable("GT_ZL");


                string str = "";

                foreach (DataRow Row in Adressen.Rows)
                {

                    if (String.IsNullOrEmpty(Row["NAME2"].ToString()) == false)
                    {
                        Row["NAME1"] += Row["NAME2"].ToString() + " ";

                    }

                    Row["NAME1"] += ", ";


                    Row["NAME1"] += Row["STREET"].ToString();


                    if (String.IsNullOrEmpty(Row["HOUSE_NUM1"].ToString()) == false)
                    {
                        Row["NAME1"] += " ";
                        Row["NAME1"] += Row["HOUSE_NUM1"].ToString();
                    }

                    Row["NAME1"] += ", ";


                    str = "200000000" + Row["KUNNR"].ToString();



                    Row["NAME1"] += Row["POST_CODE1"].ToString() + " " + Row["CITY1"].ToString() + " ~ " + str.Substring(str.Length - 9).ToString();


                }



                string ErrMessage = myProxy.getExportParameter("E_MESSAGE");


                if (ErrMessage.Length > 0)
                {
                    throw new Exception(ErrMessage);
                }


            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Es wurden keine Daten gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = ex.Message;
                        break;
                }
            }

        }


        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_SAVE_EQUI_001", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_LIZNR", Vertragsnummer);
                myProxy.setImportParameter("I_EQUNR", Equinr);

                myProxy.setImportParameter("I_KUNNR_ZL", KunnrZL.PadLeft(10, '0'));


                myProxy.callBapi();

                string ErrMessage = myProxy.getExportParameter("E_MESSAGE");

                if (ErrMessage.Length > 0)
                {
                    throw new Exception(ErrMessage);
                }


            }
            catch (Exception ex)
            {

                m_intStatus = -9999;
                m_strMessage = ex.Message;
            }

        }


        public void FillVorZulassungen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.FillVorZulassungen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_ZHNAME", Haltername.ToUpper());

                myProxy.setImportParameter("I_UNVOLLSTAENDIG", Vollst);


                myProxy.callBapi();

                m_tblResult = myProxy.getExportTable("T_ZULDOKUMENTE");

                DataColumn ColumnIstGeloescht = new DataColumn();
                ColumnIstGeloescht.AllowDBNull = true;
                ColumnIstGeloescht.ColumnName = "IstGeloescht";
                ColumnIstGeloescht.DefaultValue = false;
                m_tblResult.Columns.Add(ColumnIstGeloescht);


                foreach (DataRow row in m_tblResult.Rows)
                {
                    if (row["KARTE"].ToString() == "000")
                    {
                        row["KARTE"] = "";

                    }
                    else
                    {
                        row["KARTE"] = "X";
                    }
                }
            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_INV_AG":
                        m_intStatus = -3331;
                        m_strMessage = "Ungültige Kundennummer.";
                        break;
                    case "ERR_NO_DATA":
                        m_intStatus = -3332;
                        m_strMessage = "Keine Daten gefunden.";
                        break;
                    case "ERR_NO_PARAMETER":
                        m_intStatus = -3333;
                        m_strMessage = "Unzureichende Parameter.";
                        break;
                    default:
                        m_intStatus = -9999;

                        break;
                }
            }

        }

        public void ChangeEVB(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_01.ChangeEVB";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            //change EVB
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ZULDOKUMENTE_EVB", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_HALTER", KunnrSAP.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EVB_NR", EVBNr);
                myProxy.setImportParameter("I_EVB_VON", DatumVon);
                myProxy.setImportParameter("I_EVB_BIS", DatumBis);

                myProxy.callBapi();
            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_UPDATE":
                        m_intStatus = -3331;
                        m_strMessage = "EVB-Nummer konnte nicht gespeichert werden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Speichern der Daten ist ein Fehler aufgetreten.";
                        break;
                }
            }

            //setzen wenn gelöscht wurde
            if (IstGeloescht) {
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_ZULDOKUMENTE_LOESCH_01", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_HALTER", KunnrSAP.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName);

                    myProxy.callBapi();
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_UPDATE":
                            m_intStatus = -3331;
                            m_strMessage = "Beim l&ouml;schen ist ein Fehler aufgetreten.";
                            break;

                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Speichern der Daten ist ein Fehler aufgetreten.";
                            break;
                    }
                }
            }
        }
    }
}
