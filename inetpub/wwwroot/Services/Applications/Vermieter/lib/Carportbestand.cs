using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;

namespace Vermieter.lib
{
    public class Carportbestand : BankBase
    {


        private DataTable mCarports;
        private DataTable mCarportsFiltered;
        private DataTable mFahrzeugeFiltered;


        public DataTable Carports
        {
            get{return mCarports;
            }
        }

        public DataTable CarportsFiltered
        {
            get
            {
                return mCarportsFiltered;
            }
            set
            {
                mCarportsFiltered = value;
            }

        }


        public DataTable FahrzeugeFiltered
        {
            get
            {
                return mFahrzeugeFiltered;
            }
            set
            {
                mFahrzeugeFiltered = value;
            }

        }



        public Carportbestand(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
            
              
            {
            }




        public override void Change()
        {

        }

        public override void Show()
        {

        }








        public void FILL(string strAppID, string strSessionID,System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Carportbestand.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_FZG_BESTAND_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();


                DataTable TempTable = new DataTable();

                TempTable = myProxy.getExportTable("GT_WEB");


                TempTable.DefaultView.Sort = "ZZCARPORT";

                TempTable = TempTable.DefaultView.ToTable();


                mCarports = new DataTable();

                mCarports.Columns.Add("Carport", typeof(System.String));
                mCarports.Columns.Add("Carportname", typeof(System.String));
                mCarports.Columns.Add("AnzFahrzeuge", typeof(System.String));

                mCarports.AcceptChanges();

                

                DataRow CarRow;

                string CpFound = "";


                foreach (DataRow dr in TempTable.Rows)
                {

                    if (CpFound != dr["ZZCARPORT"].ToString())
                    {

                        
                        //Neue Row hinzufügen
                        CarRow = mCarports.NewRow();

                        CpFound = TempTable.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'")[0]["ZZCARPORT"].ToString();

                        CarRow["Carport"] = CpFound;
                        CarRow["Carportname"] = TempTable.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'")[0]["NAME_ORT_CARP"];
                        CarRow["AnzFahrzeuge"] = TempTable.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'").Count().ToString();

                        mCarports.Rows.Add(CarRow);

                        mCarports.AcceptChanges();
                       
                    }

                }


                mCarportsFiltered = mCarports.Copy();
               


                DataTable TempOutput = TempTable.Copy();


                Result = CreateOutPut(TempOutput, m_strAppID);

                mFahrzeugeFiltered = Result.Copy();

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
                WriteLogEntry(false,"KUNNR=" + m_objUser.KUNNR + ", " + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

    }
}
