using System;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class MeldungDAD : SapOrmBusinessBase
    {
        #region "Properties"

        public string IDSuche { get; set; }
        public string FahrgestellnummerSuche { get; set; }
        public string BriefnummerSuche { get; set; }
        public string ID { get; set; }
        public string Bestellnummer { get; set; }
        public string Frachtbriefnummer { get; set; }
        public string Fahrgestellnummer { get; set; }
        public string Briefnummer { get; set; }
        public string Zulassungsdatum { get; set; }
        public string Kennzeichen { get; set; }
        public string Gebuehr { get; set; }
        public bool Auslieferung { get; set; }

        #endregion

        #region "Methods"

        public MeldungDAD(string userReferenz)
        {
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void LoadVorgang()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_FIND_DAD_SD_ORDER.Init(SAP);

                    SAP.SetImportParameter("I_VKORG", "1510");
                    SAP.SetImportParameter("I_VKBUR", VKBUR);

                    if (!String.IsNullOrEmpty(IDSuche))
                        SAP.SetImportParameter("I_VBELN", IDSuche.PadLeft0(10));

                    if (!String.IsNullOrEmpty(FahrgestellnummerSuche))
                        SAP.SetImportParameter("I_FAHRG", FahrgestellnummerSuche);

                    if (!String.IsNullOrEmpty(BriefnummerSuche))
                        SAP.SetImportParameter("I_BRIEF", BriefnummerSuche);

                    CallBapi();

                    var tmpTable = SAP.GetExportTable("E_VBAK");

                    if (!ErrorOccured)
                    {
                        var row = tmpTable.Rows[0];
                        ID = row["VBELN"].ToString();
                        Bestellnummer = row["EBELN"].ToString();
                        Frachtbriefnummer = row["ZZSEND2"].ToString();
                        Fahrgestellnummer = row["ZZFAHRG"].ToString();
                        Briefnummer = row["ZZBRIEF"].ToString();
                        Zulassungsdatum = row["VDATU"].ToString();
                        Kennzeichen = row["ZZKENN"].ToString();
                    }
                });
        }

        public void ClearFields()
        {
            ID = "";
            Bestellnummer = "";
            Frachtbriefnummer = "";
            Fahrgestellnummer = "";
            Briefnummer = "";
            Zulassungsdatum = "";
            Kennzeichen = "";
        }

        public void SaveVorgang(string userName)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_SAVE_TAGGLEICHE_MELDUNG.Init(SAP);

                    var tmpTable = SAP.GetImportTable("IS_TG_MELDUNG");

                    var newRow = tmpTable.NewRow();
                    newRow["VKORG"] = "1510";
                    newRow["VBELN"] = ID.PadLeft(10, '0');
                    newRow["EBELN"] = Bestellnummer;
                    newRow["FAHRG"] = Fahrgestellnummer;
                    newRow["BRIEF"] = Briefnummer;
                    if (Zulassungsdatum.IsDate()) { newRow["ZZZLDAT"] = Zulassungsdatum; }
                    newRow["ZZKENN"] = Kennzeichen;
                    newRow["GEB_AMT"] = Gebuehr;
                    newRow["AUSLIEF"] = Auslieferung.BoolToX();
                    newRow["ZZSEND2"] = Frachtbriefnummer;
                    newRow["ERNAM"] = userName;
                    newRow["SAVE_STATUS"] = "A";
                    tmpTable.Rows.Add(newRow);

                    CallBapi();
                });
        }

        #endregion
    }
}