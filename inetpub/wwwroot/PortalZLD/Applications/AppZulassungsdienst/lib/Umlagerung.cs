using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Business;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class clsUmlagerung : SapOrmBusinessBase
    {
        public DataTable tblUmlagerung { get; set; }
        public DataTable tblArtikel { get; set; }
        public DataTable tblKennzForm { get; set; }
        public String KostStelleNeu { get; set; }
        public String KostText { get; set; }
        public String BelegNR { get; set; }

        public clsUmlagerung(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            tblUmlagerung = new DataTable();

            tblUmlagerung.Columns.Add("MATNR", typeof(String));
            tblUmlagerung.Columns.Add("UMLGO", typeof(String));
            tblUmlagerung.Columns.Add("MAKTX", typeof(String));
            tblUmlagerung.Columns.Add("EAN11", typeof(String));
            tblUmlagerung.Columns.Add("Menge", typeof(Int32));
            tblUmlagerung.Columns.Add("LTEXT_NR", typeof(String));
            tblUmlagerung.Columns.Add("LTEXT", typeof(String));
            tblUmlagerung.Columns.Add("KENNZFORM", typeof(String));
            KostStelleNeu = "";
        }

        public void Show()
        {
           ExecuteSapZugriff(() =>
               {
                   Z_FIL_EFA_UML_MAT.Init(SAP, "I_KOSTL", VKBUR);

                   CallBapi();

                   tblArtikel = SAP.GetExportTable("GT_MAT");
               });
        }

        public void CheckKostStelle (string NeuKost)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_GET_KOSTL.Init(SAP, "I_KOSTL_SEND, I_KOSTL_RECEIVE", VKBUR.PadLeft0(10), NeuKost.PadLeft0(10));

                    CallBapi();

                    KostText = SAP.GetExportParameter("E_KTEXT");

                    switch (SAP.ResultCode)
                    {
                        case 102:
                            RaiseError(SAP.ResultCode, "KST " + NeuKost + " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben.");
                            break;
                        case 104:
                            RaiseError(SAP.ResultCode, "KST nicht zulässig! Bitte richtige KST eingeben.");
                            break;
                    }
                });
        }

        public void Change()
        {
            // Langtext-IDs vor dem Aufruf des Speicher-Bapis ermitteln, weil verschachtelte SAP-Aufrufe nicht möglich sind
            var langtextIds = new Dictionary<string, string>();

            foreach (DataRow tmpRow in tblUmlagerung.Rows)
            {
                LongStringToSap LSTS = new LongStringToSap();

                var ltextId = "";

                if (tmpRow["LTEXT_NR"].ToString() == "")
                {
                    if (tmpRow["LTEXT"].ToString() != "")
                    {
                        ltextId = LSTS.InsertString(tmpRow["LTEXT"].ToString(), VKBUR);
                    }
                }
                else
                {
                    ltextId = tmpRow["LTEXT"].ToString();
                }

                langtextIds.Add(tmpRow["MATNR"].ToString() + ";" + tmpRow["KENNZFORM"].ToString(), ltextId);
            }

            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_UML_STEP1.Init(SAP, "I_KOSTL_IN, I_KOSTL_OUT", KostStelleNeu, VKBUR);

                    DataTable tblSAP = SAP.GetImportTable("GT_MAT");

                    foreach (DataRow tmpRow in tblUmlagerung.Rows)
                    {
                        DataRow tmpSAPRow = tblSAP.NewRow();
                        tmpSAPRow["MATNR"] = tmpRow["MATNR"].ToString();
                        tmpSAPRow["MENGE"] = tmpRow["MENGE"].ToString();
                        tmpSAPRow["EAN11"] = tmpRow["EAN11"].ToString();
                        tmpSAPRow["LTEXT_NR"] = langtextIds[tmpRow["MATNR"].ToString() + ";" + tmpRow["KENNZFORM"].ToString()];
                        tmpSAPRow["KENNZFORM"] = tmpRow["KENNZFORM"].ToString();
                        tblSAP.Rows.Add(tmpSAPRow);
                    }

                    CallBapi();

                    DataTable BelegTable = SAP.GetExportTable("GT_BELNR");

                    if (BelegTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < BelegTable.Rows.Count; i++)
                        {
                            BelegNR += BelegTable.Rows[i]["BELNR"].ToString() + Environment.NewLine;
                        }
                    }
                });
        }

        public void insertIntoBestellungen(String Artikelnr, String Menge, String Artikelbezeichnung, String EAN, String LTextNR, String LText, String Kennzform)
        {
            DataRow[] Rows;
            if (Kennzform == "") 
            {
                Rows = tblUmlagerung.Select("MATNR='" + Artikelnr + "'");
            }
            else
            {
                Rows = tblUmlagerung.Select("MATNR='" + Artikelnr + "' AND KennzForm = '" + Kennzform + "'");
            }
            
            DataRow tmpRow;

            if (Rows.GetLength(0) > 0)
            {
                tmpRow = Rows[0];
                tmpRow["UMLGO"] = KostStelleNeu;
                tmpRow["MATNR"] = Artikelnr;
                tmpRow["MAKTX"] = Artikelbezeichnung;
                tmpRow["EAN11"] = EAN;
                tmpRow["Menge"] = Menge;
                tmpRow["LTEXT_NR"] = LTextNR;
                tmpRow["LTEXT"] = LText;
                tmpRow["KENNZFORM"] = Kennzform;
            }
            else
            {
                tmpRow = tblUmlagerung.NewRow();
                tmpRow["UMLGO"] = KostStelleNeu;
                tmpRow["MATNR"] = Artikelnr;
                tmpRow["MAKTX"] = Artikelbezeichnung;
                tmpRow["EAN11"] = EAN;
                tmpRow["Menge"] = Menge;
                tmpRow["LTEXT_NR"] = LTextNR;
                tmpRow["LTEXT"] = LText;
                tmpRow["KENNZFORM"] = Kennzform;
                tblUmlagerung.Rows.Add(tmpRow);
            }
        }

        public void GetKennzForm(string MATNR)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_UML_MAT_GROESSE.Init(SAP, "I_MATNR", MATNR);

                    CallBapi();

                    tblKennzForm = SAP.GetExportTable("GT_MAT");
                });
        }
    }
}
