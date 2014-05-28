using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace Vermieter.lib
{
    public class Fahrzeugbestand : BankBase
    {
        
        public Fahrzeugbestand(ref User objUser, App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
        }

        public string Filterstring { get; set; }

        public DataTable Bestand { get; set; }
        public DataTable BestandFiltered { get; set; }
        public DataTable Carports { get; set; }
        public DataTable Hersteller { get; set; }
        public DataTable Lieferant { get; set; }
        public DataTable Modell { get; set; }
        public DataTable Antrieb { get; set; }
        public DataTable Farben { get; set; }
        public DataTable HalterZul { get; set; }
        public DataTable HalterFilter { get; set; }
        public DataTable EmpfZulUnterlagenHalter { get; set; }
        public DataTable Versicherer { get; set; }
        public DataTable Kennzeichenserie { get; set; }
        public DataTable EmpfZulUnterlagen { get; set; }
        public DataTable EmpfZulUnterlagenFilter { get; set; }
        public DataTable ReturnTableSave { get; set; }

        public string HalterSave { get; set; }
        public string VersicherungSave { get; set; }
        public string KennzeichenserieSave { get; set; }
        public string ZulassungsdatumSave { get; set; }
        public string KbaNrSave { get; set; }
        public string EmpfZulUnterlSave { get; set; }
        public string VerarbeitungZulassungSave { get; set; }
        public int CountAll { get; set; }
        public int CountError { get; set; }

        public override void Change()
        {
            throw new NotImplementedException();
        }

        public override void Show()
        {
            throw new NotImplementedException();
        }

        public void SetZulassung(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.SetZulassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            CountAll = 0;
            CountError = 0;
            try
            {
                var proxy = DynSapProxy.getProxy("Z_MASSENZULASSUNG", ref m_objApp, ref m_objUser, ref page);

                var import = proxy.getImportTable("INTERNTAB");

                var selected = Bestand.Select("Selected");
                foreach (var selRow in selected)
                {
                    var row = import.NewRow();

                    row["I_KUNNR_AG"] = m_objUser.KUNNR.PadLeft(10, '0');
                    row["I_ZZFAHRG"] = selRow["CHASSIS_NUM"];
                    row["I_EDATU"] = ZulassungsdatumSave;
                    row["ERDAT"] = DateTime.Today.ToShortDateString();
                    row["ERZET"] = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    row["I_KUNNR_ZV"] = VersicherungSave;
                    row["I_KUNNR_ZH"] = string.IsNullOrEmpty(HalterSave) ? selRow["KUNNR_ZH"] : HalterSave;
                    if (string.IsNullOrEmpty(KennzeichenserieSave))
                    {
                        row["I_ZZSONDER"] = selRow["SONDERSERIE"];
                        row["I_KBANR"] = selRow["KBANR"];
                    }
                    else
                    {
                        row["I_ZZSONDER"] = KennzeichenserieSave.Substring(7, KennzeichenserieSave.Length - 7);
                        row["I_KBANR"] = KennzeichenserieSave.Substring(0, 7);
                    }
                    row["I_ZZCARPORT"] = string.IsNullOrEmpty(EmpfZulUnterlSave) ? selRow["ZZCARPORT"] : EmpfZulUnterlSave;
                    row["I_Kunnr_Za"] = selRow["KUNNR_ZA"] != null ? selRow["KUNNR_ZA"] : string.Empty;

                    import.Rows.Add(row);
                }

                proxy.callBapi();

                ReturnTableSave = proxy.getExportTable("OUTPUT");
                CountAll = selected.Length;
                foreach (var row in selected)
                {
                    if (ReturnTableSave.Select("ID = '" + row["CHASSIS_NUM"] + "'").Length > 0)
                    {
                        row["Status"] = ReturnTableSave.Select("ID = '" + row["CHASSIS_NUM"] + "'")[0]["MESSAGE"].ToString();
                        CountError++;
                    }
                    else
                    {
                        row["Status"] = "OK";
                    }
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                        m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void SetPlanung(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.SetPlanung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            CountAll = 0;
            CountError = 0;
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_IMP_PLANZUL_01", ref m_objApp, ref m_objUser, ref page);

                proxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                var import = proxy.getImportTable("GT_IN");

                var selected = Bestand.Select("Selected");
                foreach (DataRow selRow in selected)
                {
                    var row = import.NewRow();

                    row["CHASSIS_NUM"] = selRow["CHASSIS_NUM"];
                    row["ERUSER"] = m_objUser.UserName;
                    row["ERDAT"] = DateTime.Today.ToShortDateString();
                    row["ERZEIT"] = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    row["PLZULDAT"] = ZulassungsdatumSave;
                    row["DURCHFD"] = VerarbeitungZulassungSave;
                    row["KUNNR_ZV"] = VersicherungSave;
                    row["KUNNR_ZH"] = string.IsNullOrEmpty(HalterSave) ? selRow["KUNNR_ZH"] : HalterSave;
                    if (string.IsNullOrEmpty(KennzeichenserieSave))
                    {
                        row["SONDERSERIE"] = selRow["SONDERSERIE"];
                        row["KBANR"] = selRow["KBANR"];
                    }
                    else
                    {
                        row["SONDERSERIE"] = KennzeichenserieSave.Substring(7, KennzeichenserieSave.Length - 7);
                        row["KBANR"] = KennzeichenserieSave.Substring(0, 7);
                    }
                    row["ZZCARPORT_PLAN"] = string.IsNullOrEmpty(EmpfZulUnterlSave) ? selRow["ZZCARPORT"] : EmpfZulUnterlSave;
                    row["KUNNR_ZA"] = selRow["KUNNR_ZA"] != null ? selRow["KUNNR_ZA"] : string.Empty;

                    import.Rows.Add(row);
                }

                proxy.callBapi();

                ReturnTableSave = proxy.getExportTable("GT_OUT");

                CountAll = selected.Length;
                foreach (var row in selected)
                {
                    if (ReturnTableSave.Select("CHASSIS_NUM = '" + row["CHASSIS_NUM"] + "'")[0]["BEM"].ToString().Contains("Planzulassung angelegt / geändert."))
                    {
                        row["Status"] = "OK";
                    }
                    else
                    {
                        row["Status"] = "Fehler";
                        CountError++;
                    }
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                        m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void FILL(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_READ_ZFAHRZEUGPOOL_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                Bestand = myProxy.getExportTable("GT_OUT");

                Bestand.Columns.Add("ID", typeof(System.Int32));
                Bestand.Columns.Add("Selected", typeof(System.Boolean));
                Bestand.Columns.Add("FARBTEXT", typeof(System.String));
                Bestand.Columns.Add("Status", typeof(System.String));
                Bestand.Columns.Add("Halter", typeof(System.String));
                Bestand.Columns.Add("Versicherung", typeof(System.String));
                Bestand.Columns.Add("Kennzeichenserie", typeof(System.String));
                Bestand.Columns.Add("EmpfZulUnterlagenText", typeof(System.String));

                var dr = Bestand.NewRow();

                dr["EQUNR"] = "0";
                dr["KUNPDI"] = " - Auswahl -";
                dr["ZZHERST_TEXT"] = " - Auswahl -";
                dr["ZZHERSTELLER_SCH"] = "0";
                dr["ZZHANDELSNAME"] = " - Auswahl -";
                dr["ZZKRAFTSTOFF_TXT"] = " - Auswahl -";
                dr["FARBTEXT"] = " - Auswahl -";
                dr["NAME1_ZP"] = " - Auswahl -";
                dr["Halter"] = " - Auswahl -";
                dr["Versicherung"] = " - Auswahl -";
                dr["EmpfZulUnterlagenText"] = " - Auswahl -";

                Bestand.Rows.Add(dr);

                Carports = Bestand.DefaultView.ToTable(true, "KUNPDI");
                Carports.DefaultView.RowFilter = "KUNPDI <> ''";
                Carports = Carports.DefaultView.ToTable();

                var columnNames = new[] { "ZZHERST_TEXT", "ZZHERSTELLER_SCH" };
                Hersteller = Bestand.DefaultView.ToTable(true, columnNames);
                Hersteller.DefaultView.RowFilter = "ZZHERST_TEXT <> ''";
                Hersteller = Hersteller.DefaultView.ToTable();

                Modell = Bestand.DefaultView.ToTable(true, "ZZHANDELSNAME");
                Modell.DefaultView.RowFilter = "ZZHANDELSNAME <> '' AND ZZHANDELSNAME <> '-'";
                Modell = Modell.DefaultView.ToTable();

                Antrieb = Bestand.DefaultView.ToTable(true, "ZZKRAFTSTOFF_TXT");
                Antrieb.DefaultView.RowFilter = "ZZKRAFTSTOFF_TXT <> ''";
                Antrieb = Antrieb.DefaultView.ToTable();

                Lieferant = Bestand.DefaultView.ToTable(true, "KUNNR_ZP", "NAME1_ZP", "ORT01_ZP");
                Lieferant.DefaultView.Sort = "NAME1_ZP";
                Lieferant.DefaultView.RowFilter = "NAME1_ZP <> ''";
                Lieferant = Lieferant.DefaultView.ToTable();
                Lieferant.Columns.Add("Display", typeof(string));
                foreach (DataRow row in Lieferant.Rows)
                    row["Display"] = row["NAME1_ZP"] + (DBNull.Value.Equals(row["ORT01_ZP"]) ? string.Empty : (" " + row["ORT01_ZP"]));
                Lieferant.AcceptChanges();

                var i = 0;

                foreach (DataRow drow in Bestand.Rows)
                {
                    //Halter hinzufügen'
                    if (HalterZul != null)
                    {
                        if (!string.IsNullOrEmpty(drow["KUNNR_ZH"].ToString()))
                        {
                            if (HalterZul.Select("HALTER = '" + drow["KUNNR_ZH"].ToString() + "'").Length > 0)
                            {
                                drow["Halter"] = HalterZul.Select("HALTER = '" + drow["KUNNR_ZH"].ToString() + "'")[0]["NAME1"];
                            }
                        }
                    }

                    //Versicherung hinzufügen'
                    if (Versicherer != null)
                    {
                        if (!string.IsNullOrEmpty(drow["KUNNR_ZV"].ToString()))
                        {
                            if (Versicherer.Select("VERSICHERER = '" + drow["KUNNR_ZV"].ToString() + "'").Length > 0)
                            {
                                drow["Versicherung"] = Versicherer.Select("VERSICHERER = '" + drow["KUNNR_ZV"].ToString() + "'")[0]["NAME1"];
                            }
                        }
                    }

                    //Empfänger Schein u. Schilder
                    if (EmpfZulUnterlagen != null)
                    {
                        if (!string.IsNullOrEmpty(drow["ZZCARPORT_PLAN"].ToString()))
                        {
                            if (EmpfZulUnterlagen.Select("DADPDI = '" + drow["ZZCARPORT_PLAN"].ToString() + "'").Length > 0)
                            {
                                drow["EmpfZulUnterlagenText"] = EmpfZulUnterlagen.Select("DADPDI = '" + drow["ZZCARPORT_PLAN"].ToString() + "'")[0]["NAME1"];
                                drow["EmpfZulUnterlagenText"] += ", " + EmpfZulUnterlagen.Select("DADPDI = '" + drow["ZZCARPORT_PLAN"].ToString() + "'")[0]["ORT01"].ToString();
                            }
                        }

                    }

                    drow["ID"] = i;
                    drow["Selected"] = false;

                    i += 1;

                    switch (drow["ZFARBE"].ToString())
                    {
                        case "0":
                            drow["FARBTEXT"] = "Weiss";
                            break;
                        case "1":
                            drow["FARBTEXT"] = "Gelb";
                            break;
                        case "2":
                            drow["FARBTEXT"] = "Orange";
                            break;
                        case "3":
                            drow["FARBTEXT"] = "Rot";
                            break;
                        case "4":
                            drow["FARBTEXT"] = "Magenta";
                            break;
                        case "5":
                            drow["FARBTEXT"] = "Blau";
                            break;
                        case "6":
                            drow["FARBTEXT"] = "Grün";
                            break;
                        case "7":
                            drow["FARBTEXT"] = "Grau";
                            break;
                        case "8":
                            drow["FARBTEXT"] = "Braun";
                            break;
                        case "9":
                            drow["FARBTEXT"] = "Schwarz";
                            break;
                        default:
                            break;
                    }
                }

                Farben = Bestand.DefaultView.ToTable(true, "FARBTEXT");
                Farben.DefaultView.RowFilter = "FARBTEXT <> ''";
                Farben = Farben.DefaultView.ToTable();

                HalterFilter = Bestand.DefaultView.ToTable(true, new[]{"KUNNR_ZH", "Halter"});
                HalterFilter.DefaultView.RowFilter = "Halter <> ''";
                HalterFilter.DefaultView.Sort = "Halter";
                HalterFilter = HalterFilter.DefaultView.ToTable();

                EmpfZulUnterlagenFilter = Bestand.DefaultView.ToTable(true, new[] { "ZZCARPORT_PLAN", "EmpfZulUnterlagenText" });
                EmpfZulUnterlagenFilter.DefaultView.RowFilter = "EmpfZulUnterlagenText <> ''";
                EmpfZulUnterlagenFilter.DefaultView.Sort = "EmpfZulUnterlagenText";
                EmpfZulUnterlagenFilter = EmpfZulUnterlagenFilter.DefaultView.ToTable();

                Bestand.Rows.RemoveAt(Bestand.Rows.Count - 1);

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void FillHalter(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.FillHalter";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_M_BLOCKEN_HALTER", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                HalterZul = myProxy.getExportTable("HALTER");

                HalterZul.Columns.Add("Name", typeof(string));
                HalterZul.Columns.Add("Display", typeof(string));

                var HalterRow = HalterZul.NewRow();

                HalterRow["HALTER"] = "0";
                HalterRow["NAME1"] = " - laut Vorschlagswert -";

                HalterZul.Rows.Add(HalterRow);

                foreach (DataRow dr in HalterZul.Rows)
                {
                    dr["Name"] = dr["NAME1"];

                    if (string.IsNullOrEmpty(dr["NAME2"].ToString()) == false)
                    {
                        dr["Name"] += " " + dr["NAME2"];
                    }

                    var ort = dr["ORT01"];
                    dr["Display"] = dr["NAME1"] + (DBNull.Value.Equals(ort) || string.IsNullOrEmpty((string)ort) ? string.Empty : (" " + ort));
                }

                HalterZul.DefaultView.Sort = "Name";

                HalterZul = HalterZul.DefaultView.ToTable();

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void FillVersicherer(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.FillVersicherer";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_M_BLOCKEN_VERSICHERER", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                Versicherer = myProxy.getExportTable("VERSICHERER");

                var VersichererRow = Versicherer.NewRow();

                VersichererRow["VERSICHERER"] = "0";
                VersichererRow["NAME1"] = " - Auswahl -";

                Versicherer.Rows.Add(VersichererRow);

                Versicherer.DefaultView.Sort = "NAME1";

                Versicherer = Versicherer.DefaultView.ToTable();



                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void FillKennzeichenserie(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.FillKennzeichenserie";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_M_EC_AVM_KENNZ_SERIE", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                Kennzeichenserie = myProxy.getExportTable("GT_WEB");
                Kennzeichenserie.Columns.Add("Kennzeichen", typeof(System.String));
                Kennzeichenserie.Columns.Add("ID", typeof(System.String));

                foreach (DataRow row in Kennzeichenserie.Rows)
                {
                    row["Kennzeichen"] = string.Format("{0}-{1}", ((string)row["ORTKENNZ"]).Trim(), ((string)row["MINLETTER"]).Trim());
                    row["Kennzeichen"] += " " + (string)row["SONDERSERIE"];
                    row["ID"] = row["KBANR"].ToString().PadRight(3, ' ') + row["SONDERSERIE"].ToString();
                }

                var KennzRow = Kennzeichenserie.NewRow();

                KennzRow["Kennzeichen"] = " - Auswahl -";
                KennzRow["ID"] = "Auswahl";

                Kennzeichenserie.Rows.Add(KennzRow);

                Kennzeichenserie.DefaultView.Sort = "Kennzeichen";

                Kennzeichenserie = Kennzeichenserie.DefaultView.ToTable();

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void FillEmpfZulUnterlagen(string strAppID, string strSessionID, Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.FillEmpfZulUnterlagen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_READ_CARPORTS_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                EmpfZulUnterlagen = myProxy.getExportTable("GT_PDI");

                EmpfZulUnterlagen.Columns.Add("Name", typeof(System.String));

                var EmpfZulUnterlagenRow = EmpfZulUnterlagen.NewRow();

                EmpfZulUnterlagenRow["DADPDI"] = "0";
                EmpfZulUnterlagenRow["NAME1"] = " - laut Eingangsdaten -";

                EmpfZulUnterlagen.Rows.Add(EmpfZulUnterlagenRow);

                foreach (DataRow dr in EmpfZulUnterlagen.Rows)
                {
                    dr["Name"] = dr["NAME1"];

                    if (!string.IsNullOrEmpty(dr["NAME2"].ToString()))
                    {
                        dr["Name"] += ", " + dr["NAME2"];
                    }

                    if (!string.IsNullOrEmpty(dr["ORT01"].ToString()))
                    {
                        dr["Name"] += ", " + dr["ORT01"];

                        if (!string.IsNullOrEmpty(dr["PSTLZ"].ToString()))
                        {
                            dr["Name"] += " (" + dr["PSTLZ"] + ")";
                        }
                    }
                }

                EmpfZulUnterlagen.DefaultView.Sort = "Name";
                EmpfZulUnterlagen = EmpfZulUnterlagen.DefaultView.ToTable();

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        
    }
}