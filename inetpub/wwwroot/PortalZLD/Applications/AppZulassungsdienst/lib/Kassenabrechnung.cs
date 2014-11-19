using System;
using System.Data;
using System.Web.UI;
using CKG.Base.Common;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.lib
{
    public class Kassenabrechnung : ErrorHandlingClass
    {
        public enum VorfallFilter { Einahmen, Ausgaben, Alles }
        public enum Geschaeftsvorfall {EinzahlungBankkonto,AuszahlungBankkonto,Erlös,Aufwand,Debitorbuchung,Kreditorbuchung,KeinVorfall }

        private App _mApp;
        private User _mUser;
        private Page _mPage;

        private DateTime _mDatumVon;
        private DateTime _mDatumBis;
        private string m_Kst;
        private string m_Buchungskreis;
        private decimal _mAnfangssaldo;
        private decimal _mEndsaldo;
        private decimal _mSummeEinnahmen;
        private decimal _mSummeAusgaben;

        private string _mWaehrung;
        private VorfallFilter _mVorfallGewaehlt;

        private DataTable _mPositionen;
        private DataTable _mGeschaeftsvorfaelle;
        private DataTable _mGeschaeftsvorfalleDebi;
        private DataTable _mGeschaeftsvorfalleKredi;

    #region Properties

        public DateTime DatumVon
        {
            get { return _mDatumVon; }
            set { _mDatumVon = value; }
        }

        public DateTime DatumBis
        {
            get { return _mDatumBis; }
            set { _mDatumBis = value; }
        }

        public string Kostenstelle
        {
            get { return m_Kst; }            
        }

        public string Buchungskreis
        {
            get { return m_Buchungskreis; }
        }

        public decimal Anfangssaldo
        {
            get { return _mAnfangssaldo; }            
        }

        public decimal Endsaldo
        {
            get { return _mEndsaldo; }            
        }

        public decimal SummeEinnahmen
        {
            get { return _mSummeEinnahmen; }            
        }

        public decimal SummeAusgaben
        {
            get { return _mSummeAusgaben; }            
        }

        public string Waehrung
        {
            get { return _mWaehrung; }
        }

        public DataTable Positionen
        {
            get {return _mPositionen ;}
            set { _mPositionen = value; }
        }

        public DataTable Geschaeftsvorfaelle
        {
            get { return _mGeschaeftsvorfaelle; }
            set { _mGeschaeftsvorfaelle = value; }
        }

        public VorfallFilter VorfallGewaehlt
        {
            get { return _mVorfallGewaehlt; }
        }

        public DataTable DocHeads
        {
            get;
            set;
        }

        public DataTable DocPos
        {
            get ; 
            set ; 
        }

        public String NewPostingNumber
        {
            get;
            set;
        }

        public String KassenbuchNr
        {
            get;
            set;
        }

        public DataTable tblError
        {
            get;
            set;
        }

    #endregion

    #region Methods

        public Kassenabrechnung(ref App app,ref User user,Page page)
        {
            _mApp = app;
            _mUser = user;
            _mPage = page;

            m_Kst = _mUser.Reference.Substring(4,4);
            m_Buchungskreis = _mUser.Reference.Substring(0, 4);
            GetKassenbuchNr(m_Kst);

            CreateVorfaelleTabelle();
        }

        /// <summary>
        /// Erzeugt eine leere Tabelle für mögliche Geschäftsvorfälle
        /// </summary>
        private void CreateVorfaelleTabelle()
        {
            _mGeschaeftsvorfaelle = new DataTable();

            _mGeschaeftsvorfaelle.Columns.Add("VorfallNummer");
            _mGeschaeftsvorfaelle.Columns.Add("VorfallBezeichnung");
            _mGeschaeftsvorfaelle.Columns.Add("VorfallTyp");
        }

        /// <summary>
        /// Erzeugt eine leere Fehlertabelle mit Positionsnummer und Fehlernummer
        /// </summary>
        public void CreateErrorTable ()
        {  
            tblError = new DataTable();
            tblError.Columns.Add("Postingnr",typeof(String));
            tblError.Columns.Add("ErrorNr", typeof(String));
        }

        private void GetKassenbuchNr(string filiale)
        {
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_CJNR", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", filiale);

                myProxy.callBapi();

                KassenbuchNr = myProxy.getExportParameter("E_CAJO_NUMBER");
            }
            catch(Exception ex)
            {
                RaiseError("1", ex.Message);
            }
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

            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_TRANSACTIONS", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", m_Kst);
                myProxy.setImportParameter("I_EIN_AUS", filterChar);

                myProxy.callBapi();

                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

                if (!ErrorOccured)
                {
                    _mGeschaeftsvorfaelle = myProxy.getExportTable("GT_TRANSACTIONS");

                    foreach (DataRow row in _mGeschaeftsvorfaelle.Rows)
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
            }
            catch (Exception ex)
            {
                RaiseError("1",ex.Message);               
            }
        }

        public void GetPeriodeFromDateNeu(DateTime datum)
        {
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_PERIOD", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_COMP_CODE", m_Buchungskreis);
                myProxy.setImportParameter("I_DATUM", datum.ToShortDateString());

                myProxy.callBapi();

                _mDatumVon = Convert.ToDateTime(myProxy.getExportParameter("E_FDAY"));
                _mDatumBis = Convert.ToDateTime(myProxy.getExportParameter("E_LDAY"));
            }
            catch (Exception ex)
            {
                RaiseError("1", ex.Message);
            }
        }

        public void SavePosition2(String postingnr)
        {
            string vorfFlag;

            switch (_mVorfallGewaehlt)
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

            GetKassenbuchNr(m_Kst);


            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_SAVE_DOC", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", m_Kst);
                myProxy.setImportParameter("I_EIN_AUS", vorfFlag);
                DataTable impTbl = myProxy.getImportTable("IS_DOCS_K");
                DataTable impTblPos = myProxy.getImportTable("GT_DOCS_P");

                DataRow[] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");

                DataRow impRow = impTbl.NewRow();


                if (rowKopf.Length == 1)
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
                        impRowPos["ORDERID"] = tmpRow["ORDERID"].ToString().PadLeft(12,'0');                        
                    }
                    impRowPos["SGTXT"] = tmpRow["SGTXT"];
                    impRowPos["LIFNR"] = tmpRow["LIFNR"];
                    impRowPos["KUNNR"] = tmpRow["KUNNR"];
                    impRowPos["ALLOC_NMBR"] = tmpRow["ALLOC_NMBR"];
                    impTblPos.Rows.Add(impRowPos);

                }

                myProxy.callBapi();

                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, "Beim Speichern der Positionen sind Fehler aufgetreten (" + E_MESSAGE + ").");
                    
                    DataRow NewError = tblError.NewRow();
                    NewError["Postingnr"] = postingnr;
                    NewError["ErrorNr"] = E_SUBRC;
                    tblError.Rows.Add(NewError);
                }
                                
            }
            catch (Exception ex)
            {
                RaiseError("1", ex.Message);               
            }

        }

        public void DeletePosition(ref DataRow row)
        {
            string vorfFlag;

            switch (_mVorfallGewaehlt)
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

            GetKassenbuchNr(m_Kst);

            if (row["POSTING_NUMBER"].ToString() == string.Empty || row["POSTING_NUMBER"] is DBNull)
            {
                row.Delete();
            }
            else
            {
                ClearErrorState();

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_SAVE_DOC", ref _mApp, ref _mUser, ref _mPage);

                    myProxy.setImportParameter("I_VKBUR", m_Kst);
                    myProxy.setImportParameter("I_EIN_AUS", vorfFlag);

                    DataTable tblDocs = myProxy.getImportTable("IS_DOCS");

                    DataRow tmpRow = tblDocs.NewRow();

                    tmpRow["BUKRS"] = row["BUKRS"];
                    tmpRow["CAJO_NUMBER"] = KassenbuchNr;
                    tmpRow["POSTING_NUMBER"] = row["POSTING_NUMBER"];
                    tmpRow["TRANSACT_NUMBER"] = row["TRANSACT_NUMBER"];
                    tmpRow["TRANSACT_NAME"] = row["TRANSACT_NAME"];
                    tmpRow["WAERS"] = "";
                    tmpRow["BUDAT"] = row["BUDAT"];

                    decimal dec;
                    decimal.TryParse(row["H_RECEIPTS"].ToString(), out dec);
                    tmpRow["H_RECEIPTS"] = dec;
                    decimal.TryParse(row["H_PAYMENTS"].ToString(), out dec);
                    tmpRow["H_PAYMENTS"] = dec;
                    decimal.TryParse(row["H_NET_AMOUNT"].ToString(), out dec);
                    tmpRow["H_NET_AMOUNT"] = dec;
                    decimal.TryParse(row["H_TAX_AMOUNT"].ToString(), out dec);
                    tmpRow["H_TAX_AMOUNT"] = dec;

                    tmpRow["TAX_CODE"] = "";
                    tmpRow["KOKRS"] = "";
                    tmpRow["KOSTL"] = row["KOSTL"].ToString().PadLeft(10, '0');
                    tmpRow["ZUONR"] = row["ZUONR"];
                    tmpRow["SGTXT"] = row["SGTXT"];
                    tmpRow["DOCUMENT_NUMBER"] = row["DOCUMENT_NUMBER"];

                    if (row["LIFNR"] is DBNull || row["LIFNR"].ToString() == string.Empty)
                    {
                        tmpRow["LIFNR"] = "";
                    }
                    else
                    {
                        tmpRow["LIFNR"] = row["LIFNR"].ToString().PadLeft(10, '0');
                    }

                    if (row["KUNNR"] is DBNull || row["KUNNR"].ToString() == string.Empty)
                    {
                        tmpRow["KUNNR"] = "";
                    }
                    else
                    {
                        tmpRow["KUNNR"] = row["KUNNR"].ToString().PadLeft(10, '0');
                    }

                    tmpRow["GL_ACCOUNT"] = "";
                    tmpRow["STATUS"] = "ZL";

                    tblDocs.Rows.Add(tmpRow);

                    myProxy.callBapi();

                    E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

                    if (!ErrorOccured)
                    {
                        FillPositionen2(_mVorfallGewaehlt);
                    }
                }
                catch (Exception ex)
                {
                    RaiseError("1",ex.Message);                    
                }
            }
        }

        public void DeleteHead2(String postingnr)
        {
            DataRow[] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");
            DataRow[] rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");
            
            if (rowKopf.Length == 1)
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

            switch (_mVorfallGewaehlt)
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

            GetKassenbuchNr(m_Kst);


            ClearErrorState();

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_SAVE_DOC", ref _mApp, ref _mUser, ref _mPage);

                    myProxy.setImportParameter("I_VKBUR", m_Kst);
                    myProxy.setImportParameter("I_EIN_AUS", vorfFlag);
                    DataTable impTbl = myProxy.getImportTable("IS_DOCS_K");
                    DataTable impTblPos = myProxy.getImportTable("GT_DOCS_P");

                    DataRow impRow = impTbl.NewRow();


                    if (rowKopf.Length == 1)
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
                        {impRowPos["ORDERID"] = tmpRow["ORDERID"].ToString().PadLeft(12, '0');}
                        impRowPos["SGTXT"] = tmpRow["SGTXT"];
                        impRowPos["LIFNR"] = tmpRow["LIFNR"];
                        impRowPos["KUNNR"] = tmpRow["KUNNR"];
                        impRowPos["ALLOC_NMBR"] = tmpRow["ALLOC_NMBR"];
                        impTblPos.Rows.Add(impRowPos);

                    }

                    myProxy.callBapi();

                    E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

                    if (!ErrorOccured)
                    {
                        if (rowKopf.Length == 1)
                        {
                           DocHeads.Rows.Remove(rowKopf[0]);
                        }

                        foreach (DataRow tmpRow in rowPos)
                        {
                            DocPos.Rows.Remove(tmpRow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    RaiseError("1", ex.Message);                
                }
            
        }

        public void Buchen2(DataRow rowHead)
        {
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_CASHJOURNALDOC_CRE", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", m_Kst);
                myProxy.setImportParameter("I_POSTING_NUMBER", rowHead["POSTING_NUMBER"].ToString());

                myProxy.callBapi();

                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                RaiseError("1",ex.Message) ;                   
            }
        }

    #endregion

    #region Functions

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
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_CALC_MWST", ref _mApp, ref _mUser, ref _mPage);

                DataTable impTbl = myProxy.getImportTable("IS_DOCS_K");
                DataTable impTblPos = myProxy.getImportTable("GT_POS");

                DataRow [] rowKopf = DocHeads.Select("POSTING_NUMBER='" + postingnr + "'");

                DataRow impRow = impTbl.NewRow();


                if (rowKopf.Length == 1)
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
               
                myProxy.callBapi();

               E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

                if (!ErrorOccured)
                {
                    DataTable table = myProxy.getExportTable("GT_POS");
                    rowPos = DocPos.Select("POSTING_NUMBER='" + postingnr + "'");
                    int i = 0;
                    foreach (DataRow tmpRow in rowPos)
                    {
                        tmpRow["P_NET_AMOUNT"] = table.Rows[i]["P_NET_AMOUNT"];
                        tmpRow["P_TAX_AMOUNT"] = table.Rows[i]["P_TAX_AMOUNT"];
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseError("1", ex.Message);
            }

        }

        public string GetTaxCode(string tranaktionsnummer)
        {
            string tc = "";

            try
            {
                DataRow [] row = _mGeschaeftsvorfaelle.Select("TRANSACT_NUMBER='" + tranaktionsnummer + "'");
                if (row.Length == 1)
                {
                    tc = row[0]["TAX_CODE"].ToString();
                }
            }
            catch 
            {
                return tc;
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
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_WERTE", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", m_Kst);
                myProxy.setImportParameter("I_BEG_DATE", dateVon.ToShortDateString());
                myProxy.setImportParameter("I_END_DATE", dateBis.ToShortDateString());

                myProxy.callBapi();

                _mAnfangssaldo = Convert.ToDecimal(myProxy.getExportParameter("E_BEG_SALDO"));
                _mEndsaldo = Convert.ToDecimal(myProxy.getExportParameter("E_END_SALDO"));
                _mSummeEinnahmen = Convert.ToDecimal(myProxy.getExportParameter("E_SUM_EIN"));
                _mSummeAusgaben = Convert.ToDecimal(myProxy.getExportParameter("E_SUM_AUS"));
                _mWaehrung = myProxy.getExportParameter("E_WAERS");
                
                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

            }
            catch (Exception ex)
            {
                RaiseError("1", ex.Message);
            }
        }

        public void FillPositionen2(VorfallFilter filter)
        {
            
            _mVorfallGewaehlt = filter;
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

            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_ALL_DOCS", ref _mApp, ref _mUser, ref _mPage);

                myProxy.setImportParameter("I_VKBUR", m_Kst);
                myProxy.setImportParameter("I_BEG_DATE", _mDatumVon.ToShortDateString());
                myProxy.setImportParameter("I_END_DATE", _mDatumBis.ToShortDateString());
                myProxy.setImportParameter("I_EIN_AUS", filterChar);

                myProxy.callBapi();

                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);                        
                }
                
                if (!ErrorOccured)
                {
                    // Kopftabelle

                    DataTable tblDocs = myProxy.getExportTable("GT_DOCS");
                    DataTable tblDocsPos = myProxy.getExportTable("GT_DOCS_P");

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

                    DataTable tblTrans = myProxy.getExportTable("GT_TRANSACTIONS");

                    _mGeschaeftsvorfaelle = tblTrans;
                    _mGeschaeftsvorfalleDebi = _mGeschaeftsvorfaelle.Clone();
                    _mGeschaeftsvorfalleKredi = _mGeschaeftsvorfaelle.Clone();
                    DataRow nRowStar = _mGeschaeftsvorfaelle.NewRow();
                    nRowStar["BUKRS"] = "1010";
                    nRowStar["TRANSACT_NUMBER"] = "*";
                    nRowStar["TRANSACT_NAME"] = "*";
                    nRowStar["TRANSACT_TYPE"] = "";
                    nRowStar["TAX_CODE"] = "";
                    _mGeschaeftsvorfaelle.Rows.Add(nRowStar);

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

                    foreach (DataRow row in _mGeschaeftsvorfaelle.Rows)
                    {
                        //switch (Filter)
                        //{
                        //    case VorfallFilter.Einahmen:
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
                                //}
                                //break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseError("-1", ex.Message);
            }
        }
        
        public void GetNewPostingNumber()
        {
            ClearErrorState();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CJ2_GET_NEW_NUMBER", ref _mApp, ref _mUser, ref _mPage);
                
                myProxy.callBapi();


                NewPostingNumber = myProxy.getExportParameter("E_POSTING_NUMBER");

                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                // Fehlerbehandlung
                if (E_SUBRC != "0" || E_MESSAGE != string.Empty)
                {
                    RaiseError(E_SUBRC, E_MESSAGE);
                }

            }
            catch (Exception ex)
            {
                RaiseError("1",ex.Message);
            }

        }

        #endregion
    }
}