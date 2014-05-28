using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace Leasing.lib
{

    public class Lieferprognosen : CKG.Base.Business.DatenimportBase
    {
            #region "Declarations"
                DataTable m_Master;
                DataTable m_Detail;
                String m_Marke;
                String m_strAction;
                Int32 m_pIndex ; 
            #endregion

            #region "Properties"
                public String Marke
                {
                    get { return m_Marke; }
                    set { m_Marke = value; }
                }

                public String Aktion
                {
                    get { return m_strAction; }
                    set { m_strAction = value; }
                }

                public DataTable Master
                {
                    get { return m_Master; }
                    set { m_Master = value; }
                }
                public DataTable Detail
                {
                    get { return m_Detail; }
                    set { m_Detail = value; }
                }
                public Int32 pIndex
                {
                    get { return m_pIndex; }
                    set { m_pIndex = value; }
                }
            #endregion

        public Lieferprognosen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void GiveCars(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Lieferprognosen.GiveCars";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Offene_Lieferprognosen_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ZZHAENR", m_objUser.Reference);
                myProxy.setImportParameter("I_MARKE", m_Marke);
                myProxy.setImportParameter("I_ANZEIGE", m_strAction);



                myProxy.callBapi();

                tblFahrzeugeSap = myProxy.getExportTable("GT_WEB");


            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -3331;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;
                    case "NO_HAENDLER":
                        m_intStatus = -3332;
                        m_strMessage = "Kein Händler gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
                

                DataTable  tblTemp;
                tblTemp = tblFahrzeugeSap;
                
                DataRow rowNew ;
                DataRow rowNewMaster ;
                Int32 i ;
                Int32 e ;
                String strCompare = "";
                String strLieferwoche = "";
                Boolean booNew = false;

                m_Master = new DataTable();

                m_Master.Columns.Add("ID", typeof(String));
                m_Master.Columns.Add("MARKE_TXT", typeof(String));
                m_Master.Columns.Add("MODELL_TXT", typeof(String));
                m_Master.Columns.Add("LIEFERWOCHE", typeof(String));
                m_Master.Columns.Add("STATUS", typeof(String));


                m_Detail = new DataTable();

                m_Detail.Columns.Add("UID", typeof(Int32));
                m_Detail.Columns.Add("ID", typeof(String));
                m_Detail.Columns.Add("MARKE", typeof(String));
                m_Detail.Columns.Add("MARKE_TXT", typeof(String));
                m_Detail.Columns.Add("MODELL_TXT", typeof(String));
                m_Detail.Columns.Add("MODELL", typeof(String));
                m_Detail.Columns.Add("BODYTYP", typeof(String));
                m_Detail.Columns.Add("BODYTYP_TEXT", typeof(String));
                m_Detail.Columns.Add("LIEFERWOCHE", typeof(String));
                m_Detail.Columns.Add("BEMERKUNG", typeof(String));
                m_Detail.Columns.Add("STATUS", typeof(String));
                m_Detail.Columns.Add("EDITED", typeof(String));

                i = 0;
                e = 0;
                rowNewMaster = null;

                foreach (DataRow row in tblTemp.Rows)
                {

                    if (strCompare !=  row["MODELL"].ToString())
                    {
                        booNew = true;
                        i += 1;

                    }
                    if (booNew == true)
                    {
                        rowNewMaster = m_Master.NewRow();
                        rowNewMaster["ID"] = i.ToString();
                        rowNewMaster["MARKE_TXT"] = row["MARKE_TXT"].ToString();
                        rowNewMaster["MODELL_TXT"] = row["MODELL_TXT"].ToString();
                        if (row["LIEFERWOCHE"].ToString() != "0")
                        {
                            rowNewMaster["LIEFERWOCHE"] = row["LIEFERWOCHE"].ToString().Substring(row["LIEFERWOCHE"].ToString().Length - 2) + "." +
                                row["LIEFERWOCHE"].ToString().Substring(2, 2);
                        }
                        else
                        {
                            rowNewMaster["LIEFERWOCHE"] = string.Empty;
                        }

                        strLieferwoche = row["LIEFERWOCHE"].ToString();
                        m_Master.Rows.Add(rowNewMaster);
                        m_Master.AcceptChanges();
                    }
                    else
                    {
                        if (row["LIEFERWOCHE"].ToString() != strLieferwoche)
                        {
                            rowNewMaster.BeginEdit();
                            rowNewMaster["LIEFERWOCHE"] = "Diff.";
                            rowNewMaster.EndEdit();
                            m_Master.AcceptChanges();
                        }
                    }

                    rowNew = m_Detail.NewRow();
                    rowNew["UID"] = e;
                    rowNew["ID"] = i.ToString();
                    rowNew["MARKE"] = row["MARKE"];
                    rowNew["MARKE_TXT"] = row["MARKE_TXT"];
                    rowNew["MODELL"] = row["MODELL"];
                    rowNew["MODELL_TXT"] = row["MODELL_TXT"];

                    if (row["LIEFERWOCHE"].ToString() != "0")
                    {
                        rowNew["LIEFERWOCHE"] = row["LIEFERWOCHE"].ToString().Substring(row["LIEFERWOCHE"].ToString().Length - 2) + "." +
                            row["LIEFERWOCHE"].ToString().Substring(2, 2);
                    }
                    else
                    {
                        rowNew["LIEFERWOCHE"] = string.Empty;
                    }
                    rowNew["BODYTYP"] = row["BODYTYP"];
                    rowNew["BODYTYP_TEXT"] = row["BODYTYP_TEXT"];
                    rowNew["BEMERKUNG"] = row["BEMERKUNG"].ToString().Trim();
                    rowNew["EDITED"] = string.Empty;
                    m_Detail.Rows.Add(rowNew);
                    m_Detail.AcceptChanges();
                    strCompare = row["MODELL"].ToString();
                    booNew = false;

                    e += 1;


            }

        }

        public DataTable getMarke(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Lieferprognosen.getMarke";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable SAPTable = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Offene_Lieferprog_Drop_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ZZHAENR", m_objUser.Reference);

                myProxy.callBapi();

                SAPTable = myProxy.getExportTable("GT_WEB");


            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -3331;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;
                    case "NO_HAENDLER":
                        m_intStatus = -3332;
                        m_strMessage = "Kein Händler gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
            return SAPTable ;
        }


        public void SaveLieferprognose(String strAppID, String strSessionID, System.Web.UI.Page page) 
        {
            m_strClassAndMethod = "Lieferprognosen.SaveLieferprognose";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            
            DataRow [] rows;

            Int32 e;
            Int32 i;
            Int32 f;
            foreach (DataRow row in m_Master.Rows)
            {
                rows = Detail.Select("ID = '" + row["ID"].ToString() + "'");
                i = rows.Length;
                rows = null;
                rows = Detail.Select("ID = '" + row["ID"].ToString() + "' and LIEFERWOCHE = '" + row["LIEFERWOCHE"].ToString() + "'");
                e = rows.Length;
                if (e==i)
                {
                    for (f = 0; f <= e - 1; f++)
                    {
                        rows[f].BeginEdit();
                        rows[f]["LIEFERWOCHE"] = row["LIEFERWOCHE"].ToString();
                        rows[f].AcceptChanges();
                    }
                    
                }
            }
            Detail.AcceptChanges();


            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Erf_Lieferprognosen_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                DataRow SapExportRow;
                DataTable SapExportTable   = myProxy.getImportTable("IT_WEB");
                foreach (DataRow row  in Detail.Rows)
                {
                    if (row["EDITED"].ToString() == "X")
                    {
                      SapExportRow = SapExportTable.NewRow();
                    SapExportRow["Kunnr"] = m_objUser.KUNNR.PadLeft(10, '0');
                    SapExportRow["ZZHAENR"] = m_objUser.Reference;
                    SapExportRow["Marke"] = row["MARKE"].ToString();
                    SapExportRow["Modell"] = row["MODELL"].ToString();
                    SapExportRow["Bodytyp"] = row["BODYTYP"].ToString();
                    if (row["LIEFERWOCHE"].ToString() != String.Empty)
                    { SapExportRow["LIEFERWOCHE"] = "20" + row["LIEFERWOCHE"].ToString().Split('.')[1] + row["LIEFERWOCHE"].ToString().Split('.')[0]; }
                    else
                    { SapExportRow["LIEFERWOCHE"] = row["LIEFERWOCHE"].ToString(); }

                    SapExportRow["Bemerkung"] = row["BEMERKUNG"].ToString();
                    SapExportRow["Ernam"] = row["BODYTYP"].ToString();
                    if (m_objUser.UserName.Length > 11)
                    {SapExportRow["Ernam"]= m_objUser.UserName.Substring(0, 11);}
                    else
                    { SapExportRow["Ernam"] = m_objUser.UserName; }
                    SapExportRow["Mandt"] = "";
                
                    SapExportTable.Rows.Add(SapExportRow);
                    SapExportRow = null;                      
                    }

                }
                myProxy.callBapi();
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_UPDATE":
                        m_intStatus = -3331;
                        m_strMessage = "Update fehlgeschlagen.";
                        break;
                   default:
                        m_intStatus = -9999;
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
        }

    }
}
