using System;
using System.Data;
using CKG.Base.Business;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class StatusaenderungVorgang : SapOrmBusinessBase
    {
        #region "Properties"

        public string IDSuche { get; set; }
        public string ID { get; set; }
        public string Belegtyp { get; set; }
        public DateTime? Zulassungsdatum { get; set; }
        public string Kundennummer { get; set; }
        public string Kreis { get; set; }
        public string Kennzeichen { get; set; }
        public string BEBStatus { get; set; }

        public string BEBStatusText
        {
            get
            {
                if (tblBEBStatusWerte != null)
                {
                    var statusRows = tblBEBStatusWerte.Select("DOMVALUE_L = '" + BEBStatus + "'");
                    if (statusRows.Length > 0)
                    {
                        return statusRows[0]["DDTEXT"].ToString();
                    }
                }

                return BEBStatus;
            }
        }

        public string BEBStatusNeu { get; set; }

        public DataTable tblBEBStatusWerte { get; private set; }
        public DataTable tblBelegTypen { get; private set; }

        public string BelegtypLangtext
        {
            get
            {
                if (tblBelegTypen != null)
                {
                    DataRow[] drow = tblBelegTypen.Select("DOMVALUE_L = '" + Belegtyp + "'");
                    if (drow.Length > 0)
                    {
                        return drow[0]["DDTEXT"].ToString();
                    }
                }

                return Belegtyp;
            }
        }

        #endregion

        #region "Methods"

        public StatusaenderungVorgang(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void LoadStatuswerte()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_DOMAENEN_WERTE.Init(SAP, "I_DOMNAME", "ZZLD_BEB_STATUS");

                    CallBapi();

                    tblBEBStatusWerte = SAP.GetExportTable("GT_WERTE");
                });
        }

        public void LoadBelegtypen()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_DOMAENEN_WERTE.Init(SAP, "I_DOMNAME", "ZZLD_BLTYP");

                    CallBapi();

                    tblBelegTypen = SAP.GetExportTable("GT_WERTE");
                });
        }

        public void LoadVorgang()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_GET_VG_FOR_UPD.Init(SAP, "I_ZULBELN, I_VKBUR", IDSuche.PadLeft0(10), VKBUR);

                    CallBapi();

                    var tmpTable = SAP.GetExportTable("GT_VG_STAT");

                    if (!ErrorOccured)
                    {
                        var row = tmpTable.Rows[0];
                        ID = row["ZULBELN"].ToString();
                        Belegtyp = row["BLTYP"].ToString();
                        DateTime tmpDat;
                        if (DateTime.TryParse(row["ZZZLDAT"].ToString(), out tmpDat))
                        {
                            Zulassungsdatum = tmpDat;
                        }
                        else
                        {
                            Zulassungsdatum = null;
                        }
                        Kundennummer = row["KUNNR"].ToString();
                        Kreis = row["KREISKZ"].ToString();
                        Kennzeichen = row["ZZKENN"].ToString();
                        BEBStatus = row["BEB_STATUS"].ToString();
                    }
                });
        }

        public void SaveVorgang()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_SET_VG_STATUS.Init(SAP, "I_ZULBELN, I_STATUS", ID.PadLeft0(10), BEBStatusNeu);

                    CallBapi();

                    if (!ErrorOccured)
                        BEBStatus = BEBStatusNeu;
                });
        }

        #endregion
    }
}