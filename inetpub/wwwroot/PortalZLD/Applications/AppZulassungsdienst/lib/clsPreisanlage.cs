using System;
using System.Data;
using CKG.Base.Business;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
	public class clsPreisanlage : SapOrmBusinessBase
	{
		#region "Properties"

        public DataTable tblNeueKunden { get; set; }

        public String NeueKundenNr { get; set; }
        public String NeueKundenName { get; set; }

		#endregion

		#region "Methods"

        public clsPreisanlage(string userReferenz)
		{
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz); 
		}

		public void getNeueKunden()
		{
			ExecuteSapZugriff(() =>
			    {
                    Z_ZLD_EXPORT_NEW_DEBI.Init(SAP, "I_VKORG, I_VKBUR", VKORG, VKBUR);

                    CallBapi();

                    tblNeueKunden = SAP.GetExportTable("GT_KUNDEN");

                    foreach (DataRow item in tblNeueKunden.Rows)
                    {
                        item["KUNNR"] = item["KUNNR"].ToString().TrimStart('0');
                    }
			    });
		}

		public void SaveNeueKunden(DataRow NewRow)
		{
			ExecuteSapZugriff(() =>
			    {
                    Z_ZLD_SETNEW_DEBI_ERL.Init(SAP);

                    DataTable SapTable = SAP.GetImportTable("GT_KUNDEN");

                    DataRow SapNewRow = SapTable.NewRow();

                    SapNewRow["KUNNR"] = NewRow["KUNNR"].ToString();
                    SapNewRow["VKUNNR"] = NewRow["VKUNNR"].ToString();
                    SapNewRow["KONDA"] = NewRow["KONDA"].ToString();
                    SapNewRow["NAME1"] = NewRow["NAME1"].ToString();
                    SapNewRow["PREIS_ZLD"] = "X";

                    SapTable.Rows.Add(SapNewRow);

                    CallBapi();
			    });
		}

		#endregion
    }
}
