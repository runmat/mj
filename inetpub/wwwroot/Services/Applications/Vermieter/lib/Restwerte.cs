using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace Vermieter.lib
{
    public class Restwerte : BankBase
    {

        public string E_SUBRC { get; set; }
        public string E_MESSAGE { get; set; }

        public string AG { get; set; }
        public string TG { get; set; }
        public string TgAg { get; set; }

 

        public Restwerte(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
        }


        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void GetCustomer(System.Web.UI.Page page, string strAppID, string strSessionID)
        {
            m_strClassAndMethod = "Restwerte.GetCustomer";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;


            


            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_GET_TREUH_AG",ref m_objApp,ref m_objUser,ref page);

                myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "B");
                myProxy.setImportParameter("I_ALL_AG", "X");
                myProxy.callBapi();

                Result = myProxy.getExportTable("GT_EXP");

                if (Result.Rows.Count < 1)
                {

                    DataTable custTable = GetCustomerDt(page, strAppID, strSessionID);

                    if (custTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in custTable.Rows)
                        {

                            DataRow newRow = Result.NewRow();

                            foreach (DataColumn dc in Result.Columns)
                            {

                                newRow[dc.ColumnName] = dr[dc.ColumnName];


                            }

                            Result.Rows.Add(newRow);

                        }
                        Result.AcceptChanges();


                    }



                }



                


                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
            }
        }


        DataTable GetCustomerDt(System.Web.UI.Page page, string strAppID, string strSessionID)
        {
            m_strClassAndMethod = "Restwerte.GetCustomerDT";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            DataTable returnTable = new DataTable();


            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_GET_TREUH_AG", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "B");
                myProxy.callBapi();

                returnTable = myProxy.getExportTable("GT_EXP");


                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }

            return returnTable;

        }





        public void GetRestwerte(System.Web.UI.Page page, string strAppID, string strSessionID)
        {
            m_strClassAndMethod = "Restwerte.GetRestwerte";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_MESSBELEGE_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", AG.PadLeft(10, '0'));
                myProxy.setImportParameter("I_TG", TG.PadLeft(10, '0'));
                myProxy.setImportParameter("FLAG_LAST_MB", "X");
                
                DataTable TempTable = new DataTable();



                TempTable = myProxy.getImportTable("GT_IN");

                DataRow dr;

                dr = TempTable.NewRow();
                dr["ATNAM"] = "FZBEZAHLT";
                TempTable.Rows.Add(dr);

                dr = TempTable.NewRow();
                dr["ATNAM"] = "FZRESTWERT";
                TempTable.Rows.Add(dr);

                dr = TempTable.NewRow();
                dr["ATNAM"] = "FZEINKAUFSWERT";
                TempTable.Rows.Add(dr);

                TempTable.AcceptChanges();
                
                myProxy.callBapi();

                DataTable StammTable;
                DataTable MessTable;

                StammTable = myProxy.getExportTable("GT_STAMM");
                MessTable = myProxy.getExportTable("GT_MESSB");


                DataTable OutTable = new DataTable();

                OutTable.Columns.Add("Name",typeof(System.String));
                OutTable.Columns.Add("Fahrgestellnummer", typeof(System.String));
                OutTable.Columns.Add("TIDNR", typeof(System.String));
                OutTable.Columns.Add("Kennzeichen", typeof(System.String));
                OutTable.Columns.Add("Vertragsnummer", typeof(System.String));
                OutTable.Columns.Add("ZZREFERENZ1", typeof(System.String));
                OutTable.Columns.Add("ZZREFERENZ2", typeof(System.String));
                OutTable.Columns.Add("Einkaufswert", typeof(System.String));
                OutTable.Columns.Add("Restwert", typeof(System.String));
                OutTable.Columns.Add("RestwertBerechnet", typeof(System.String));
                OutTable.Columns.Add("Bezahlt", typeof(System.String));
                OutTable.Columns.Add("Laufzeit", typeof(System.String));
                OutTable.Columns.Add("Tranchenmittelwert",typeof(System.String));

                DataRow nRow;

                string DummyName = "AG_NAME1";

                if (TgAg == "TG")
                {
                    DummyName = "TG_NAME1";
                }
                


                foreach (DataRow sRow in StammTable.Rows)
                {

                    nRow = OutTable.NewRow();

                    nRow["Name"] = sRow[DummyName];
                    nRow["Fahrgestellnummer"] = sRow["CHASSIS_NUM"];
                    nRow["TIDNR"] = sRow["TIDNR"];
                    nRow["Kennzeichen"] = sRow["LICENSE_NUM"];
                    nRow["Vertragsnummer"] = sRow["LIZNR"];
                    nRow["ZZREFERENZ1"] = sRow["ZZREFERENZ1"];
                    nRow["ZZREFERENZ2"] = sRow["ZZREFERENZ2"];

                    MessTable.DefaultView.RowFilter = "EQUNR='" + sRow["EQUNR"] + "' AND ATNAM='FZEINKAUFSWERT'";

                    if (MessTable.DefaultView.Count > 0)
                    {
                        nRow["Einkaufswert"] = MessTable.DefaultView[0]["READG_C"];
                    }
                    else
                    {
                        nRow["Einkaufswert"] = "";
                    }

                    MessTable.DefaultView.RowFilter = "";

                    MessTable.DefaultView.RowFilter = "EQUNR='" + sRow["EQUNR"] + "' AND ATNAM='FZRESTWERT'";

                    if (MessTable.DefaultView.Count > 0)
                    {
                        nRow["Restwert"] = MessTable.DefaultView[0]["READG_C"];
                    }
                    else
                    {
                        nRow["Restwert"] = "";
                    }

                    if (nRow["Einkaufswert"].ToString().Length > 0 && nRow["Restwert"].ToString().Length > 0 && sRow["LAUFZEIT"].ToString().Length > 0)
                    {
                        //ToDo: Formel zur Berechnung
                        double Einkaufswert;
                        double Restwert;
                        double Laufzeit;
                        double Ergebnis;
                        double Abschreibungstag;
                        double FlexLaufzeit;

                        if (double.TryParse(nRow["Einkaufswert"].ToString(),out Einkaufswert) && 
                            double.TryParse(nRow["Restwert"].ToString(),out Restwert) &&
                            double.TryParse(sRow["LAUFZEIT"].ToString(),out Laufzeit))
                        {


                            if (double.TryParse(sRow["LAUFZEIT_EQUI"].ToString(), out FlexLaufzeit))
                            {
                                if (sRow["LAUFZEIT_EQUI"].ToString() == "000")
                                {
                                    FlexLaufzeit = 180;
                                    nRow["Laufzeit"] = "180";
                                }
                                else
                                {
                                    nRow["Laufzeit"] = FlexLaufzeit.ToString();
                                }
                               
                            }
                            else
                            {
                                FlexLaufzeit = 180;
                                nRow["Laufzeit"] = "180";
                            }

                           

                            Abschreibungstag = ((Einkaufswert - Restwert) / FlexLaufzeit) * Laufzeit;
                            Ergebnis = (Einkaufswert - Abschreibungstag);

                            //Ergebnis = ((Einkaufswert + Restwert) / 180) * Laufzeit;
                            nRow["RestwertBerechnet"] =  System.Math.Round(Ergebnis,2).ToString();
                        }


                    }

                    MessTable.DefaultView.RowFilter = "";

                    MessTable.DefaultView.RowFilter = "EQUNR='" + sRow["EQUNR"] + "' AND ATNAM='FZBEZAHLT'";

                    if (MessTable.DefaultView.Count > 0)
                    {
                        nRow["Bezahlt"] = "X";

                    }

                    MessTable.DefaultView.RowFilter = "";

                    OutTable.Rows.Add(nRow);

                    OutTable.AcceptChanges();
                }

                OutTable = SummeTranchenmittelwerteErmittelnUndEintragen(OutTable);

                Result = OutTable;


                E_SUBRC = myProxy.getExportParameter("E_SUBRC");
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }

        }


        DataTable SummeTranchenmittelwerteErmittelnUndEintragen(DataTable sourceTable)
        {
            var tableReferenzMittelwertKumuliert = GetTableReferenzMittelwert();
           

            foreach (DataRow sourceTableRow in sourceTable.Rows)
            {
                if ((tableReferenzMittelwertKumuliert.Select("ZZREFERENZ2 = '" + sourceTableRow["ZZREFERENZ2"] + "'").Length < 1) & (sourceTableRow["ZZREFERENZ2"].ToString().Length > 0))  
                {

                    insertTranchenMittelwert(sourceTable, sourceTableRow["ZZREFERENZ2"].ToString());
                }
            }


            return sourceTable;

        }

        static DataTable GetTableReferenzMittelwert()
        {
            var table = new DataTable();
            table.Columns.Add("ZZREFERENZ2", typeof(string));
            table.Columns.Add("Tranchenmittelwert", typeof(int));

            return table;
        }

        void insertTranchenMittelwert(DataTable table, string referenz)
        {

            int tranchenMittelwert = 0;
            decimal tempTranchenMittelwert = 0;

            foreach (DataRow dr in table.Rows)
            {
                if (dr["ZZREFERENZ2"].ToString() == referenz)
                {
                    decimal restwert;
                    decimal einkaufswert;
                    if (decimal.TryParse(dr["Einkaufswert"].ToString(),out einkaufswert) && 
                            decimal.TryParse(dr["Restwert"].ToString(),out restwert))
                    {
                        tempTranchenMittelwert += einkaufswert + restwert;

                    }

                }
            }

            if (tempTranchenMittelwert > 0)
            {
                tempTranchenMittelwert = (tempTranchenMittelwert/2);

                tranchenMittelwert = RoundToNextHigherThousand(tempTranchenMittelwert);

                 foreach (DataRow dr in table.Rows)
                {
                     if (dr["ZZREFERENZ2"].ToString() == referenz)
                     {
                    decimal dummy1;
                    decimal dummy2;
                    if (decimal.TryParse(dr["Einkaufswert"].ToString(),out dummy1) && 
                            decimal.TryParse(dr["Restwert"].ToString(),out dummy2))
                    {
                        dr["Tranchenmittelwert"] = tranchenMittelwert.ToString();
                    }

                     }
                
                }

            }

           


        }

        private int RoundToNextHigherThousand(decimal value)
        {
            var backValue = 0;

            if (value <= 1000)
            {
                backValue = (int) value;
                return backValue;
            }

            value = Math.Round(value, 2);


            decimal hundreds;
            Boolean addThousand = false;


            string tempValue = Right(value.ToString(),6);

            if (decimal.TryParse(tempValue,out hundreds))
            {
                if (hundreds > 0)
                {
                    addThousand = true;
                }
            }

           

            if (addThousand == true)
            {
                tempValue = value.ToString().Substring(0, value.ToString().Length - 6);
                tempValue = tempValue.PadRight(tempValue.Length + 3, '0');
                backValue =  Convert.ToInt32(tempValue);
                backValue = backValue + 1000;
            }
            else
            {
                tempValue = tempValue.PadRight(tempValue.Length + 3, '0');
                backValue = Convert.ToInt32(tempValue);

            }


            return backValue;


        }

        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }



    }
}
