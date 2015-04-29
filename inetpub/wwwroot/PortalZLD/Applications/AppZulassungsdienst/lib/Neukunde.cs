using System;
using System.Data;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class Neukunde : SapOrmBusinessBase
    {
        public DataTable tblLaender { get; set; }
        public DataTable tblBranchen { get; set; }
        public DataTable tblFunktion { get; set; }

        public String MitarbeiterNr { get; set; }
        public String Abruftyp { get; set; }
        public String EinzugEr { get; set; }
        public String Anrede { get; set; }
        public String Branche { get; set; }
        public String BrancheFreitext { get; set; }
        public String Name1 { get; set; }
        public String Name2 { get; set; }
        public String Strasse { get; set; }
        public String Ort { get; set; }
        public String HausNr { get; set; }
        public String PLZ { get; set; }
        public String Land { get; set; }
        public String UIDNummer { get; set; }
        public String ASPVorname { get; set; }
        public String ASPName { get; set; }
        public String Funktion { get; set; }
        public String Telefon { get; set; }
        public String Mobil { get; set; }
        public String Mail { get; set; }
        public String Fax { get; set; }
        public String NeueKUNNR { get; set; }
        public String BLZ { get; set; }
        public String Kontonr { get; set; }
        public String Bankname { get; set; }
        public String Bankkey { get; set; }
        public String IBAN { get; set; }
        public String SWIFT { get; set; }
        public Boolean Einzug { get; set; }
        public Boolean UmSteuer { get; set; }
        public Boolean Kreditvers { get; set; }
        public Boolean Auskunft { get; set; }
        public String TourID { get; set; }
        public String UmStErwartung { get; set; }
        public String Bemerkung { get; set; }

        public Neukunde(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void Fill()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ALL_DEBI_CHECK_TABLES.Init(SAP);

                    CallBapi();

                    tblLaender = SAP.GetExportTable("GT_T005");
                    tblBranchen = SAP.GetExportTable("GT_T016");
                    tblFunktion = SAP.GetExportTable("GT_TPFK");
                });
        }

        public void Change(string userName)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ALL_DEBI_VORERFASSUNG_WEB.Init(SAP);

                    DataTable tblSAP = SAP.GetImportTable("GS_IN");

                    DataRow SapRow = tblSAP.NewRow();
                    SapRow["BUKRS"] = VKORG;
                    SapRow["VKORG"] = VKORG;
                    SapRow["VKBUR"] = VKBUR;
                    SapRow["KALKS"] = Abruftyp;
                    SapRow["EZERM"] = EinzugEr;
                    SapRow["TITLE"] = Anrede;
                    SapRow["BRSCH"] = Branche;
                    SapRow["BRSCH_FREITXT"] = BrancheFreitext;
                    SapRow["NAME1"] = Name1;
                    SapRow["NAME2"] = Name2;
                    SapRow["NAME3"] = "";
                    SapRow["NAME4"] = "";
                    SapRow["STREET"] = Strasse;
                    SapRow["CITY1"] = Ort;
                    SapRow["HOUSE_NUM1"] = HausNr;
                    SapRow["POST_CODE1"] = PLZ;
                    SapRow["LAND1"] = Land;
                    SapRow["STCEG"] = UIDNummer;
                    SapRow["AP_NAMEV"] = ASPVorname;
                    SapRow["AP_NAME1"] = ASPName;
                    SapRow["AP_PAFKT"] = Funktion;
                    SapRow["AP_TEL_NUMBER"] = Telefon;
                    SapRow["AP_MOB_NUMBER"] = Mobil;
                    SapRow["AP_SMTP_ADDR"] = Mail;
                    SapRow["AP_FAX_NUMBER"] = Fax;
                    SapRow["QUELLE"] = "ZLD-Neu";
                    SapRow["ERNAM"] = userName;
                    if (Bankkey.Length > 0)
                    {
                        SapRow["BANKS"] = "DE";
                        SapRow["BANKL"] = Bankkey;
                        SapRow["BNKLZ"] = BLZ;
                        SapRow["BANKN"] = Kontonr;
                        SapRow["IBAN"] = IBAN;
                        SapRow["SWIFT"] = SWIFT;
                    }
                    if (TourID.Length > 0)
                    {
                        TourID = TourID.PadLeft(10, '0');
                    }
                    SapRow["GRUPPE_T"] = TourID;
                    SapRow["UMS_P_MON"] = UmStErwartung;
                    SapRow["GEB_M_UST"] = UmSteuer.BoolToX();
                    SapRow["KREDITVS"] = Kreditvers.BoolToX();
                    SapRow["AUSKUNFT"] = Auskunft.BoolToX();
                    SapRow["BEMERKUNG"] = Bemerkung;
                    tblSAP.Rows.Add(SapRow);

                    CallBapi();

                    NeueKUNNR = SAP.GetExportParameter("E_VKUNNR");
                });
        }

        public void ProofIBAN()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FI_CONV_IBAN_2_BANK_ACCOUNT.Init(SAP, "I_IBAN", IBAN);

                    CallBapi();

                    Bankname = SAP.GetExportParameter("E_BANKA");
                    BLZ = SAP.GetExportParameter("E_BANK_NUMBER");
                    Bankkey = BLZ;
                    Kontonr = SAP.GetExportParameter("E_BANK_ACCOUNT");
                    SWIFT = SAP.GetExportParameter("E_SWIFT");

                    if (ErrorOccured)
                        m_strErrorMessage = "IBAN fehlerhaft: " + m_strErrorMessage;
                });
        }
    }
}
