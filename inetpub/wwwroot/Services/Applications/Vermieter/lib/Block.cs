using System;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class Block
    {

        #region "Declarations"


        #endregion


        public DataTable GetBlockData(ref CKG.Base.Kernel.Security.User objUser, ref CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, string Fahrzeugnummer,Boolean ShowAll, string BankTreuhand, DataTable ImportTable)
        {

           
            string KUNNR = objUser.KUNNR.PadLeft(10, '0');
            //Exportparameter
            DataTable ExportTable = new DataTable();


            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_FZG_DATEN_BLOCK",ref objApp, ref objUser,ref page);


            try
            {

                //Importparameter

                myProxy.setImportParameter("I_AG", KUNNR);

                if (ShowAll == true)
                {
                    myProxy.setImportParameter("I_READ_FZG_POOL", "X");
                }
                else
                {
                    myProxy.setImportParameter("I_FZG_NR", Fahrzeugnummer);
                }
                
                if (BankTreuhand != "0")
                    myProxy.setImportParameter("I_BANK_TH_SEL", BankTreuhand);




                DataTable ImportSAPTable = myProxy.getImportTable("GT_IN");

                if ((ImportTable != null))
                {

                    if (ImportTable.Rows.Count > 0)
                    {
                        DataRow dr = null;



                        foreach (DataRow dRow in ImportTable.Rows)
                        {
                            dr = ImportSAPTable.NewRow();

                            dr["CHASSIS_NUM"] = dRow["Fahrgestellnummer"];
                            dr["LICENSE_NUM"] = dRow["Kennzeichen"];
                            dr["LIZNR"] = dRow["Leasingvertragsnummer"];
                            dr["FZG_NR"] = dRow["FahrzeugnummerAlt"];
                            dr["BLOCK_NR_NEU"] = dRow["FahrzeugnummerNeu"];

                            ImportSAPTable.Rows.Add(dr);

                        }


                        ImportSAPTable.AcceptChanges();


                    }
                }


                myProxy.callBapi();


                ExportTable = myProxy.getExportTable("GT_OUT");

                ExportTable.Columns.Add("Status", typeof(string));


            }
            catch
            {
            }
            finally
            {
                myProxy = null;
            }

            return ExportTable;
        }

        public DataTable SetBlockData(ref CKG.Base.Kernel.Security.User objUser, ref CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, DataTable ImportTable)
        {

            string KUNNR = objUser.KUNNR.PadLeft(10, '0');
            //Exportparameter
            DataTable ExportTable = new DataTable();


            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_SAVE_FZG_DATEN_BLOCK",ref objApp,ref objUser,ref page);


            try
            {

                //Importparameter

                myProxy.setImportParameter("I_AG", KUNNR);
                myProxy.setImportParameter("FLAG_FZG_NR", "X");



                DataTable ImportSAPTable = myProxy.getImportTable("GT_WEB");

                if ((ImportTable != null))
                {

                    if (ImportTable.Rows.Count > 0)
                    {
                        DataRow dr = null;



                        foreach (DataRow dRow in ImportTable.Rows)
                        {
                            if (dRow["BLOCK_ALT_LOE"].ToString() == "X" || dRow["BLOCK_NR_NEU"].ToString().Length > 0)
                            {
                                dr = ImportSAPTable.NewRow();

                                dr["EQUNR"] = dRow["EQUNR"];


                                if (dRow["BLOCK_ALT_LOE"].ToString() != "X")
                                {
                                    dr["FZG_NR"] = dRow["BLOCK_NR_NEU"];
                                }
                                
                               

                                ImportSAPTable.Rows.Add(dr);
                            }



                        }


                        ImportSAPTable.AcceptChanges();


                    }
                }


                myProxy.callBapi();


                string ExportError = myProxy.getExportParameter("FLAG_ERROR").ToUpper();

                ExportTable = myProxy.getExportTable("GT_WEB");

                ExportTable.DefaultView.RowFilter = "ZBEM <> ''";
                ExportTable = ExportTable.DefaultView.ToTable();



            }
            catch
            {
            }
            finally
            {
                myProxy = null;
            }

            return ExportTable;



        }


    }



}
