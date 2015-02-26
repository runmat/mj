using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
	public class VorVersand : SapOrmBusinessBase
	{
		#region "Properties"

        public DataTable BestLieferanten { get; set; }
        public DataTable Liste { get; set; }
        public DataTable ExcelListe { get; set; }

        public String SelDatum { get; set; }
        public String SelDatumBis { get; set; }
        public String SelID { get; set; }
        public String SelKunde { get; set; }
        public String SelKreis { get; set; }
        public String SelLief { get; set; }
        public String SelStatus { get; set; }

		#endregion

        public VorVersand(string userReferenz)
		{
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
		}

		public void getBestLieferant()
		{
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_INFOPOOL.Init(SAP, "I_KREISKZ", SelKreis);

                    CallBapi();

                    BestLieferanten = SAP.GetExportTable("GT_EX_ZUSTLIEF");

                    DataRow NewLief = BestLieferanten.NewRow();
                    NewLief["LIFNR"] = "0";
                    NewLief["NAME1"] = "";
                    BestLieferanten.Rows.Add(NewLief);
                });
		}

        public void FillVersanZul(List<Kundenstammdaten> kundenStamm)
		{
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_VZOZUERL.Init(SAP);

                    if (!String.IsNullOrEmpty(SelKunde))
                        SAP.SetImportParameter("I_KUNNR", SelKunde.ToSapKunnr());

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_VKORG", VKORG);
                    SAP.SetImportParameter("I_KREISKZ", SelKreis);
                    SAP.SetImportParameter("I_ZZZLDAT_VON", SelDatum);
                    SAP.SetImportParameter("I_ZZZLDAT_BIS", SelDatumBis);

                    if (!String.IsNullOrEmpty(SelID))
                        SAP.SetImportParameter("I_ZULBELN", SelID.PadLeft0(10));

                    SAP.SetImportParameter("I_STATUS", "");
                    SAP.SetImportParameter("I_LISTE", SelStatus);

                    CallBapi();

                    Liste = SAP.GetExportTable("GT_EX_ZUERL");
                    Liste.Columns.Add("KUNNAME", typeof(String));
                    Liste.Columns.Add("GebPreis", typeof(String));
                    Liste.Columns.Add("PreisKZ", typeof(String));
                    Liste.Columns.Add("Steuer", typeof(String));
                    Liste.Columns.Add("WBKunde", typeof(String));

                    foreach (DataRow item in Liste.Rows)
                    {
                        item["WBKunde"] = (item["VZB_STATUS"].ToString() == "VD" ? "J" : "N");

                        item["ZULPOSNR"] = item["ZULPOSNR"].ToString().TrimStart('0');
                        item["GebPreis"] = "0,00";
                        item["PreisKZ"] = "0,00";
                        item["Steuer"] = "0,00";
                        item["PREIS"] = String.Format("{0:0.00}", item["PREIS"]);
                        item["KBETR"] = String.Format("{0:0.00}", item["KBETR"]);

                        if (item["WEBMTART"].ToString() == "D")
                        {
                            DataRow[] SelItem = Liste.Select("ZULBELN='" + item["ZULBELN"].ToString() + "' AND UEPOS ='" + item["ZULPOSNR"].ToString().PadLeft(6, '0') + "'");
                            if (SelItem.Length > 0)
                            {
                                for (int i = 0; i < SelItem.Length; i++)
                                {
                                    if (SelItem[i]["WEBMTART"].ToString() == "G")
                                    {
                                        item["GebPreis"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
                                    }
                                    if (SelItem[i]["WEBMTART"].ToString() == "K")
                                    {
                                        item["PreisKZ"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
                                    }
                                    if (SelItem[i]["WEBMTART"].ToString() == "S")
                                    {
                                        item["Steuer"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
                                    }
                                }
                            }
                            item["KUNNR"] = item["KUNNR"].ToString().TrimStart('0');
                            item["ZULBELN"] = item["ZULBELN"].ToString().TrimStart('0');

                            var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == item["KUNNR"].ToString());
                            if (kunde != null)
                                item["KUNNAME"] = kunde.Name;

                            if (!String.IsNullOrEmpty(item["ZL_LIFNR"].ToString()))
                            {
                                item["ZL_LIFNR"] = item["ZL_LIFNR"].ToString().TrimStart('0');
                                if (item["ZL_LIFNR"].ToString().Substring(0, 2) == "56")
                                {
                                    item["KBETR"] = 0;
                                }
                            }
                        }
                    }

                    for (int i = Liste.Rows.Count - 1; i > 0; i--)
                    {
                        if (Liste.Rows[i]["WEBMTART"].ToString() != "D")
                        {
                            Liste.Rows.RemoveAt(i);
                        }
                    }

                    ExcelListe = Liste.Copy();
                    ExcelListe.Columns.Remove("Belegnr");
                    ExcelListe.Columns.Remove("ZULPOSNR");
                    ExcelListe.Columns.Remove("BLTYP");
                    ExcelListe.Columns["LoeschKZ"].ColumnName = "L/OK";
                });
		}

        public void UpdateStatus(String sID, String sPosNr, String sStatus, String sLoesch, DataTable UpdateListe)
		{
            ExecuteSapZugriff(() =>
                {
                    BestLieferanten = new DataTable();

                    Z_ZLD_CHANGE_VZOZUERL.Init(SAP);

                    DataTable SapTable = SAP.GetImportTable("GT_IMP_VZOZUERL");

                    DataRow NewRow;
                    if (UpdateListe == null)
                    {
                        NewRow = SapTable.NewRow();
                        NewRow["ZULBELN"] = sID.PadLeft(10, '0');
                        NewRow["ZULPOSNR"] = sPosNr.PadLeft(6, '0');
                        NewRow["STATUS"] = sStatus;
                        NewRow["LOEKZ"] = sLoesch;
                        if (sStatus == "S")
                        {
                            NewRow["VZERDAT"] = DateTime.Now.ToShortDateString();
                        }
                        SapTable.Rows.Add(NewRow);
                    }
                    else
                    {
                        foreach (DataRow mRow in UpdateListe.Rows)
                        {
                            NewRow = SapTable.NewRow();
                            NewRow["ZULBELN"] = mRow["ZULBELN"].ToString().PadLeft(10, '0');
                            NewRow["ZULPOSNR"] = mRow["ZULPOSNR"].ToString().PadLeft(6, '0');
                            NewRow["STATUS"] = mRow["STATUS"].ToString();
                            NewRow["LOEKZ"] = mRow["LOEKZ"].ToString();
                            if (mRow["STATUS"].ToString() == "S")
                            {
                                NewRow["VZERDAT"] = DateTime.Now.ToShortDateString();
                            }
                            SapTable.Rows.Add(NewRow);
                        }
                    }

                    if (SapTable.Rows.Count > 0)
                    {
                        CallBapi();
                    }
                });
		}
	}
}
