using System;
using System.Data;
using CKG.Base.Business;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class Kassenabrechnung : SapOrmBusinessBase
    {
        public enum VorfallFilter { Einahmen, Ausgaben, Alles }
        public enum Geschaeftsvorfall { EinzahlungBankkonto, AuszahlungBankkonto, Erlös, Aufwand, Debitorbuchung, Kreditorbuchung, KeinVorfall }

        private DataTable _mGeschaeftsvorfalleDebi;
        private DataTable _mGeschaeftsvorfalleKredi;

    #region Properties

        public DataTable Positionen { get; set; }
        public DataTable Geschaeftsvorfaelle { get; set; }
        public DataTable DocHeads { get; set; }
        public DataTable DocPos { get; set; }
        public DataTable tblError { get; set; }

        public DateTime DatumVon { get; set; }
        public DateTime DatumBis { get; set; }
        public decimal Anfangssaldo { get; private set; }
        public decimal Endsaldo { get; private set; }
        public decimal SummeEinnahmen { get; private set; }
        public decimal SummeAusgaben { get; private set; }
        public string Waehrung { get; private set; }
        public String NewPostingNumber { get; set; }
        public String KassenbuchNr { get; set; }
        public VorfallFilter VorfallGewaehlt { get; private set; }

    #endregion

    #region Methods & Functions

        public Kassenabrechnung(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            GetKassenbuchNr();

            CreateVorfaelleTabelle();
        }

        private void CreateVorfaelleTabelle()
        {
            Geschaeftsvorfaelle = new DataTable();

            Geschaeftsvorfaelle.Columns.Add("VorfallNummer");
            Geschaeftsvorfaelle.Columns.Add("VorfallBezeichnung");
            Geschaeftsvorfaelle.Columns.Add("VorfallTyp");
        }

        public void CreateErrorTable ()
        {  
            tblError = new DataTable();
            tblError.Columns.Add("Postingnr",typeof(String));
            tblError.Columns.Add("ErrorNr", typeof(String));
        }

        private void GetKassenbuchNr()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_CJNR.Init(SAP, "I_VKBUR", VKBUR);

                    CallBapi();

                    KassenbuchNr = SAP.GetExportParameter("E_CAJO_NUMBER");
                });
        }

        public void FillGeschaeftsvorfaelleNeu(VorfallFilter filter)
        {
            string filterChar;

            switch (filter)
            {
                case VorfallFilter.Einahmen:
                    filterChar = "E";
                    break;
                case VorfallFilter.Ausgaben:
                    filterChar = "A";
                    break;
                default:
                    filterChar = "*";
                    break;
            }

            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_TRANSACTIONS.Init(SAP, "I_VKBUR, I_EIN_AUS", VKBUR, filterChar);

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        Geschaeftsvorfaelle = SAP.GetExportTable("GT_TRANSACTIONS");

                        foreach (DataRow row in Geschaeftsvorfaelle.Rows)
                        {
                            switch (filter)
                            {
                                case VorfallFilter.Einahmen:
                                    if (Convert.ToString(row["TRANSACT_TYPE"]) == "D")
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleDebi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleDebi.Rows.Add(nRow);
                                    }
                                    break;
                                case VorfallFilter.Ausgaben:
                                    if (Convert.ToString(row["TRANSACT_TYPE"]) == "K")
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleKredi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleKredi.Rows.Add(nRow);
                                    }
                                    break;
                                default:
                                    if (Convert.ToString(row["TRANSACT_TYPE"]) == "D")
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleDebi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleDebi.Rows.Add(nRow);
                                    }
                                    else if (Convert.ToString(row["TRANSACT_TYPE"]) == "K")
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleKredi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleKredi.Rows.Add(nRow);
                                    }
                                    break;
                            }
                        }
                    }
                });
        }

        public void GetPeriodeFromDateNeu(DateTime datum)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_PERIOD.Init(SAP, "I_COMP_CODE, I_DATUM", VKORG, datum.ToShortDateString());

                    CallBapi();

                    DatumVon = Convert.ToDateTime(SAP.GetExportParameter("E_FDAY"));
                    DatumBis = Convert.ToDateTime(SAP.GetExportParameter("E_LDAY"));
                });
        }

        public void SavePosition2(String postingnr)
        {
            string vorfFlag;

            switch (VorfallGewaehlt)
            {
                case VorfallFilter.Einahmen:
                    vorfFlag = "E";
                    break;
                case VorfallFilter.Ausgaben:
                    vorfFlag = "A";
                    break;
                default:
                    vorfFlag = "*";
                    break;
            }

            GetKassenbuchNr();

            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_SAVE_DOC.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_EIN_AUS", vorfFlag);

                    DataTable impTbl = SAP.GetImportTable("IS_DOCS_K");
                    DataTable impTblPos = SAP.GetImportTable("GT_DOCS_P");

                    DataRow[] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");

                    DataRow impRow = impTbl.NewRow();

                    if (rowKopf.Length > 0)
                    {
                        impRow["BUKRS"] = rowKopf[0]["BUKRS"];
                        impRow["CAJO_NUMBER"] = rowKopf[0]["CAJO_NUMBER"];
                        impRow["POSTING_NUMBER"] = rowKopf[0]["POSTING_NUMBER"];
                        impRow["WAERS"] = rowKopf[0]["WAERS"];
                        impRow["BUDAT"] = rowKopf[0]["BUDAT"];
                        impRow["DOCUMENT_NUMBER"] = rowKopf[0]["DOCUMENT_NUMBER"];
                        impRow["STATUS"] = rowKopf[0]["STATUS"];
                        impRow["ASTATUS"] = rowKopf[0]["ASTATUS"];
                        impTbl.Rows.Add(impRow);
                    }

                    DataRow[] rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");

                    foreach (DataRow tmpRow in rowPos)
                    {
                        DataRow impRowPos = impTblPos.NewRow();
                        impRowPos["BUKRS"] = tmpRow["BUKRS"];
                        impRowPos["CAJO_NUMBER"] = tmpRow["CAJO_NUMBER"];
                        impRowPos["POSTING_NUMBER"] = tmpRow["POSTING_NUMBER"];
                        impRowPos["POSITION_NUMBER"] = tmpRow["POSITION_NUMBER"];
                        impRowPos["TRANSACT_NUMBER"] = tmpRow["TRANSACT_NUMBER"];
                        impRowPos["P_RECEIPTS"] = tmpRow["P_RECEIPTS"];
                        impRowPos["P_PAYMENTS"] = tmpRow["P_PAYMENTS"];
                        impRowPos["TAX_CODE"] = "";
                        impRowPos["KOKRS"] = "";
                        impRowPos["KOSTL"] = tmpRow["KOSTL"];
                        if (tmpRow["ORDERID"].ToString().Trim(' ').Length > 0)
                        {
                            impRowPos["ORDERID"] = tmpRow["ORDERID"].ToString().PadLeft(12, '0');
                        }
                        impRowPos["SGTXT"] = tmpRow["SGTXT"];
                        impRowPos["LIFNR"] = tmpRow["LIFNR"];
                        impRowPos["KUNNR"] = tmpRow["KUNNR"];
                        impRowPos["ALLOC_NMBR"] = tmpRow["ALLOC_NMBR"];
                        impTblPos.Rows.Add(impRowPos);

                    }

                    CallBapi();

                    if (ErrorOccured)
                    {
                        DataRow NewError = tblError.NewRow();
                        NewError["Postingnr"] = postingnr;
                        NewError["ErrorNr"] = SAP.ResultCode;
                        tblError.Rows.Add(NewError);
                    }
                });
        }

        public void DeletePosition(ref DataRow row)
        {
            string vorfFlag;

            switch (VorfallGewaehlt)
            {
                case VorfallFilter.Einahmen:
                    vorfFlag = "E";
                    break;
                case VorfallFilter.Ausgaben:
                    vorfFlag = "A";
                    break;
                default:
                    vorfFlag = "*";
                    break;
            }

            GetKassenbuchNr();

            if (row["POSTING_NUMBER"].ToString() == string.Empty || row["POSTING_NUMBER"] is DBNull)
            {
                row.Delete();
            }
            else
            {
                var dRow = row;

                ExecuteSapZugriff(() =>
                    {
                        Z_ZLD_CJ2_SAVE_DOC.Init(SAP);

                        SAP.SetImportParameter("I_VKBUR", VKBUR);
                        SAP.SetImportParameter("I_EIN_AUS", vorfFlag);

                        DataTable tblDocs = SAP.GetImportTable("IS_DOCS");

                        DataRow tmpRow = tblDocs.NewRow();

                        tmpRow["BUKRS"] = dRow["BUKRS"];
                        tmpRow["CAJO_NUMBER"] = KassenbuchNr;
                        tmpRow["POSTING_NUMBER"] = dRow["POSTING_NUMBER"];
                        tmpRow["TRANSACT_NUMBER"] = dRow["TRANSACT_NUMBER"];
                        tmpRow["TRANSACT_NAME"] = dRow["TRANSACT_NAME"];
                        tmpRow["WAERS"] = "";
                        tmpRow["BUDAT"] = dRow["BUDAT"];

                        decimal dec;
                        decimal.TryParse(dRow["H_RECEIPTS"].ToString(), out dec);
                        tmpRow["H_RECEIPTS"] = dec;
                        decimal.TryParse(dRow["H_PAYMENTS"].ToString(), out dec);
                        tmpRow["H_PAYMENTS"] = dec;
                        decimal.TryParse(dRow["H_NET_AMOUNT"].ToString(), out dec);
                        tmpRow["H_NET_AMOUNT"] = dec;
                        decimal.TryParse(dRow["H_TAX_AMOUNT"].ToString(), out dec);
                        tmpRow["H_TAX_AMOUNT"] = dec;

                        tmpRow["TAX_CODE"] = "";
                        tmpRow["KOKRS"] = "";
                        tmpRow["KOSTL"] = dRow["KOSTL"].ToString().PadLeft(10, '0');
                        tmpRow["ZUONR"] = dRow["ZUONR"];
                        tmpRow["SGTXT"] = dRow["SGTXT"];
                        tmpRow["DOCUMENT_NUMBER"] = dRow["DOCUMENT_NUMBER"];

                        if (dRow["LIFNR"] is DBNull || dRow["LIFNR"].ToString() == string.Empty)
                        {
                            tmpRow["LIFNR"] = "";
                        }
                        else
                        {
                            tmpRow["LIFNR"] = dRow["LIFNR"].ToString().PadLeft(10, '0');
                        }

                        if (dRow["KUNNR"] is DBNull || dRow["KUNNR"].ToString() == string.Empty)
                        {
                            tmpRow["KUNNR"] = "";
                        }
                        else
                        {
                            tmpRow["KUNNR"] = dRow["KUNNR"].ToString().PadLeft(10, '0');
                        }

                        tmpRow["GL_ACCOUNT"] = "";
                        tmpRow["STATUS"] = "ZL";

                        tblDocs.Rows.Add(tmpRow);

                        CallBapi();
                    });

                if (!ErrorOccured)
                    FillPositionen2(VorfallGewaehlt);
            }
        }

        public void DeleteHead2(String postingnr)
        {
            DataRow[] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");
            DataRow[] rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");

            if (rowKopf.Length > 0)
            {

                if (rowKopf[0]["New"].ToString() == "1")
                {
                    DocHeads.Rows.Remove(rowKopf[0]);
                  
                    foreach (DataRow tmpRow in rowPos)
                    {
                        DocPos.Rows.Remove(tmpRow);
                    }
                    return;
                }
            }

            string vorfFlag;

            switch (VorfallGewaehlt)
            {
                case VorfallFilter.Einahmen:
                    vorfFlag = "E";
                    break;
                case VorfallFilter.Ausgaben:
                    vorfFlag = "A";
                    break;
                default:
                    vorfFlag = "*";
                    break;
            }

            GetKassenbuchNr();

            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_SAVE_DOC.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_EIN_AUS", vorfFlag);

                    DataTable impTbl = SAP.GetImportTable("IS_DOCS_K");
                    DataTable impTblPos = SAP.GetImportTable("GT_DOCS_P");

                    DataRow impRow = impTbl.NewRow();

                    if (rowKopf.Length > 0)
                    {
                        impRow["BUKRS"] = rowKopf[0]["BUKRS"];
                        impRow["CAJO_NUMBER"] = rowKopf[0]["CAJO_NUMBER"];
                        impRow["POSTING_NUMBER"] = rowKopf[0]["POSTING_NUMBER"];
                        impRow["WAERS"] = rowKopf[0]["WAERS"];
                        impRow["BUDAT"] = rowKopf[0]["BUDAT"];

                        impRow["DOCUMENT_NUMBER"] = rowKopf[0]["DOCUMENT_NUMBER"];
                        impRow["STATUS"] = "ZL";

                        impTbl.Rows.Add(impRow);
                    }

                    foreach (DataRow tmpRow in rowPos)
                    {
                        DataRow impRowPos = impTblPos.NewRow();
                        impRowPos["BUKRS"] = tmpRow["BUKRS"];
                        impRowPos["CAJO_NUMBER"] = tmpRow["CAJO_NUMBER"];
                        impRowPos["POSTING_NUMBER"] = tmpRow["POSTING_NUMBER"];
                        impRowPos["POSITION_NUMBER"] = tmpRow["POSITION_NUMBER"];
                        impRowPos["TRANSACT_NUMBER"] = tmpRow["TRANSACT_NUMBER"];
                        impRowPos["P_RECEIPTS"] = tmpRow["P_RECEIPTS"];
                        impRowPos["P_PAYMENTS"] = tmpRow["P_PAYMENTS"];
                        impRowPos["TAX_CODE"] = tmpRow["TAX_CODE"];
                        impRowPos["KOKRS"] = "";
                        impRowPos["KOSTL"] = tmpRow["KOSTL"];
                        if (tmpRow["ORDERID"].ToString().Length > 0)
                        { impRowPos["ORDERID"] = tmpRow["ORDERID"].ToString().PadLeft(12, '0'); }
                        impRowPos["SGTXT"] = tmpRow["SGTXT"];
                        impRowPos["LIFNR"] = tmpRow["LIFNR"];
                        impRowPos["KUNNR"] = tmpRow["KUNNR"];
                        impRowPos["ALLOC_NMBR"] = tmpRow["ALLOC_NMBR"];
                        impTblPos.Rows.Add(impRowPos);
                    }

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        if (rowKopf.Length > 0)
                        {
                            DocHeads.Rows.Remove(rowKopf[0]);
                        }

                        foreach (DataRow tmpRow in rowPos)
                        {
                            DocPos.Rows.Remove(tmpRow);
                        }
                    }
                });
        }

        public void Buchen2(DataRow rowHead)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_CASHJOURNALDOC_CRE.Init(SAP, "I_VKBUR, I_POSTING_NUMBER", VKBUR, rowHead["POSTING_NUMBER"].ToString());

                    CallBapi();
                });
        }

        public bool CheckDebiKrediNeeded(string transaktNummer)
        {
            bool blnDebi = (_mGeschaeftsvorfalleDebi.Select("TRANSACT_NUMBER = '" + transaktNummer + "'").Length > 0);
            bool blnKredi = (_mGeschaeftsvorfalleKredi.Select("TRANSACT_NUMBER = '" + transaktNummer + "'").Length > 0);

            return (blnDebi || blnKredi);
        }

        public bool CheckDebiNeeded(string transaktNummer)
        {
            bool blnDebi = (_mGeschaeftsvorfalleDebi.Select("TRANSACT_NUMBER = '" + transaktNummer + "'").Length > 0);

            return blnDebi;
        }

        public bool CheckKrediNeeded(string transaktNummer)
        {
            bool blnKredi = (_mGeschaeftsvorfalleKredi.Select("TRANSACT_NUMBER = '" + transaktNummer + "'").Length > 0);

            return blnKredi;
        }

        public void GetMwSt2(String postingnr)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_CALC_MWST.Init(SAP);

                    DataTable impTbl = SAP.GetImportTable("IS_DOCS_K");
                    DataTable impTblPos = SAP.GetImportTable("GT_POS");

                    DataRow[] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");

                    DataRow impRow = impTbl.NewRow();

                    if (rowKopf.Length > 0)
                    {
                        impRow["BUKRS"] = rowKopf[0]["BUKRS"];
                        impRow["CAJO_NUMBER"] = rowKopf[0]["CAJO_NUMBER"];
                        impRow["POSTING_NUMBER"] = rowKopf[0]["POSTING_NUMBER"];
                        impRow["WAERS"] = rowKopf[0]["WAERS"];
                        impRow["BUDAT"] = rowKopf[0]["BUDAT"];
                        impRow["DOCUMENT_NUMBER"] = rowKopf[0]["DOCUMENT_NUMBER"];
                        impRow["STATUS"] = rowKopf[0]["STATUS"];

                        impTbl.Rows.Add(impRow);
                    }

                    DataRow[] rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");

                    foreach (DataRow tmpRow in rowPos)
                    {
                        DataRow impRowPos = impTblPos.NewRow();
                        impRowPos["BUKRS"] = tmpRow["BUKRS"];
                        impRowPos["CAJO_NUMBER"] = tmpRow["CAJO_NUMBER"];
                        impRowPos["POSTING_NUMBER"] = tmpRow["POSTING_NUMBER"];
                        impRowPos["POSITION_NUMBER"] = tmpRow["POSITION_NUMBER"];
                        impRowPos["TRANSACT_NUMBER"] = tmpRow["TRANSACT_NUMBER"];
                        impRowPos["TRANSACT_NAME"] = tmpRow["TRANSACT_NAME"];
                        impRowPos["P_RECEIPTS"] = tmpRow["P_RECEIPTS"];
                        impRowPos["P_PAYMENTS"] = tmpRow["P_PAYMENTS"];
                        impRowPos["P_NET_AMOUNT"] = "0";
                        impRowPos["P_TAX_AMOUNT"] = "0";
                        impRowPos["KOSTL"] = tmpRow["KOSTL"];
                        if (tmpRow["ORDERID"].ToString().Trim(' ').Length > 0)
                        {
                            impRowPos["ORDERID"] = tmpRow["ORDERID"].ToString().PadLeft(12, '0');
                        }

                        impRowPos["SGTXT"] = tmpRow["SGTXT"];
                        impRowPos["LIFNR"] = tmpRow["LIFNR"];
                        impRowPos["KUNNR"] = tmpRow["KUNNR"];
                        impRowPos["ALLOC_NMBR"] = tmpRow["ALLOC_NMBR"];
                        impTblPos.Rows.Add(impRowPos);
                    }

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        DataTable table = SAP.GetExportTable("GT_POS");
                        rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");
                        int i = 0;
                        foreach (DataRow tmpRow in rowPos)
                        {
                            tmpRow["P_NET_AMOUNT"] = table.Rows[i]["P_NET_AMOUNT"];
                            tmpRow["P_TAX_AMOUNT"] = table.Rows[i]["P_TAX_AMOUNT"];
                            i++;
                        }
                    }
                });
        }

        public string GetTaxCode(string tranaktionsnummer)
        {
            string tc = "";

            try
            {
                DataRow [] row = Geschaeftsvorfaelle.Select("TRANSACT_NUMBER='" + tranaktionsnummer + "'");
                if (row.Length > 0)
                    tc = row[0]["TAX_CODE"].ToString();
            }
            catch 
            {
            }

            return tc;
        }

        public string TranslateStatus(string status)
        {
            string statusImg;

            switch (status.ToUpper())
            {
                case "ZE":
                    statusImg = "/PortalZLD/Images/onebit_07.png";    //gelb
                    break;
                case "ZL":
                    statusImg = "/PortalZLD/Images/onebit_06.png";    //grün
                    break;
                case "":
                    statusImg = "/PortalZLD/Images/onebit_10.png";    //rot
                    break;
                case "ZG":
                    statusImg = "/PortalZLD/Images/onebit_07.png";    //gelb
                    break;
                default:
                    statusImg = "/PortalZLD/Images/onebit_06.png";    //grün
                    break;
            }

            return statusImg;
        }

        public void FillWerte2(DateTime dateVon, DateTime dateBis)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_WERTE.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_BEG_DATE", dateVon.ToShortDateString());
                    SAP.SetImportParameter("I_END_DATE", dateBis.ToShortDateString());

                    CallBapi();

                    Anfangssaldo = Convert.ToDecimal(SAP.GetExportParameter("E_BEG_SALDO"));
                    Endsaldo = Convert.ToDecimal(SAP.GetExportParameter("E_END_SALDO"));
                    SummeEinnahmen = Convert.ToDecimal(SAP.GetExportParameter("E_SUM_EIN"));
                    SummeAusgaben = Convert.ToDecimal(SAP.GetExportParameter("E_SUM_AUS"));
                    Waehrung = SAP.GetExportParameter("E_WAERS");
                });
        }

        public void FillPositionen2(VorfallFilter filter)
        {
            VorfallGewaehlt = filter;
            string filterChar;

            switch (filter)
            {
                case VorfallFilter.Einahmen:
                    filterChar = "E";
                    break;
                case VorfallFilter.Ausgaben:
                    filterChar = "A";
                    break;
                default:
                    filterChar = "*";
                    break;
            }

            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_ALL_DOCS.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_BEG_DATE", DatumVon.ToShortDateString());
                    SAP.SetImportParameter("I_END_DATE", DatumBis.ToShortDateString());
                    SAP.SetImportParameter("I_EIN_AUS", filterChar);

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        // Kopftabelle
                        DataTable tblDocs = SAP.GetExportTable("GT_DOCS");
                        DataTable tblDocsPos = SAP.GetExportTable("GT_DOCS_P");

                        tblDocs.Columns.Add("Ampel");
                        tblDocs.Columns.Add("Date", typeof(String));
                        tblDocs.Columns.Add("New", typeof(String));
                        tblDocs.Columns.Add("Auswahl", typeof(Boolean));

                        foreach (DataRow row in tblDocs.Rows)
                        {
                            row["Ampel"] = TranslateStatus((string)row["Status"]);
                            row["Date"] = Convert.ToDateTime(row["BUDAT"]).ToShortDateString();
                            row["BUDAT"] = Convert.ToDateTime(row["BUDAT"]).ToShortDateString();
                            if (row["KOSTL"].ToString().Length == 10)
                            {
                                row["KOSTL"] = row["KOSTL"].ToString().Substring(6, 4);
                            }
                            row["LIFNR"] = row["LIFNR"].ToString().TrimStart('0');
                            row["KUNNR"] = row["KUNNR"].ToString().TrimStart('0');
                            row["ORDERID"] = row["ORDERID"].ToString().TrimStart('0');
                            row["DOCUMENT_NUMBER"] = row["DOCUMENT_NUMBER"].ToString().TrimStart('0');
                            row["New"] = "0";
                            row["Auswahl"] = false;
                            if (row["ASTATUS"].ToString().Length > 0)
                            {
                                if (row["ASTATUS"].ToString() == "ZA")
                                {
                                    row["Ampel"] = "/PortalZLD/Images/InfoAuto.gif";
                                }
                            }
                        }

                        DocHeads = tblDocs;

                        //Positionen
                        tblDocsPos.Columns.Add("Status", typeof(String));

                        foreach (DataRow row in tblDocsPos.Rows)
                        {
                            if (row["KOSTL"].ToString().Length == 10)
                            {
                                row["KOSTL"] = row["KOSTL"].ToString().Substring(6, 4);
                            }
                            row["LIFNR"] = row["LIFNR"].ToString().TrimStart('0');
                            row["KUNNR"] = row["KUNNR"].ToString().TrimStart('0');
                            row["ORDERID"] = row["ORDERID"].ToString().TrimStart('0');
                            DataRow[] kopfRow = tblDocs.Select("POSTING_NUMBER = '" + row["POSTING_NUMBER"] + "'");
                            row["Status"] = kopfRow[0]["Status"].ToString();// gibt nur eine Kopfzeile
                        }

                        DocPos = tblDocsPos;

                        // Geschäftsvorfälle
                        DataTable tblTrans = SAP.GetExportTable("GT_TRANSACTIONS");

                        Geschaeftsvorfaelle = tblTrans;
                        _mGeschaeftsvorfalleDebi = Geschaeftsvorfaelle.Clone();
                        _mGeschaeftsvorfalleKredi = Geschaeftsvorfaelle.Clone();
                        DataRow nRowStar = Geschaeftsvorfaelle.NewRow();
                        nRowStar["BUKRS"] = "1010";
                        nRowStar["TRANSACT_NUMBER"] = "*";
                        nRowStar["TRANSACT_NAME"] = "*";
                        nRowStar["TRANSACT_TYPE"] = "";
                        nRowStar["TAX_CODE"] = "";
                        Geschaeftsvorfaelle.Rows.Add(nRowStar);

                        nRowStar = _mGeschaeftsvorfalleDebi.NewRow();
                        nRowStar["BUKRS"] = "1010";
                        nRowStar["TRANSACT_NUMBER"] = "*";
                        nRowStar["TRANSACT_NAME"] = "*";
                        nRowStar["TRANSACT_TYPE"] = "";
                        nRowStar["TAX_CODE"] = "";
                        _mGeschaeftsvorfalleDebi.Rows.Add(nRowStar);

                        nRowStar = _mGeschaeftsvorfalleKredi.NewRow();
                        nRowStar["BUKRS"] = "1010";
                        nRowStar["TRANSACT_NUMBER"] = "*";
                        nRowStar["TRANSACT_NAME"] = "*";
                        nRowStar["TRANSACT_TYPE"] = "";
                        nRowStar["TAX_CODE"] = "";
                        _mGeschaeftsvorfalleKredi.Rows.Add(nRowStar);

                        foreach (DataRow row in Geschaeftsvorfaelle.Rows)
                        {
                            switch (Convert.ToString(row["TRANSACT_TYPE"]))
                            {
                                case "D":
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleDebi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleDebi.Rows.Add(nRow);
                                    }
                                    break;
                                case "K":
                                    {
                                        DataRow nRow = _mGeschaeftsvorfalleKredi.NewRow();
                                        nRow["BUKRS"] = row["BUKRS"];
                                        nRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                                        nRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                                        nRow["TRANSACT_TYPE"] = row["TRANSACT_TYPE"];
                                        nRow["TAX_CODE"] = row["TAX_CODE"];
                                        _mGeschaeftsvorfalleKredi.Rows.Add(nRow);
                                    }
                                    break;
                            }
                        }
                    }
                });
        }
        
        public void GetNewPostingNumber()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_CJ2_GET_NEW_NUMBER.Init(SAP);

                    CallBapi();

                    NewPostingNumber = SAP.GetExportParameter("E_POSTING_NUMBER");
                });
        }

        #endregion
    }
}