using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using CKG;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace Leasing.lib
{
	public class LP_03 : CKG.Base.Business.DatenimportBase
	{
		//    REM § Status-Report, Kunde: ASL, BAPI: Z_M_Abm_Abgemeldete_Kfz,
		//    REM § Ausgabetabelle per Zuordnung in Web-DB.

		private DataTable tableNative;

		public DataTable getNativeData()
		{
			return tableNative;
		}

		#region "Declarations"

		private string strKennzeichenVon;
		private string strKennzeichenBis;
		private string strFahrgestellVon;
		private string strFahrgestellBis;
		private string strLeasingVertrNrVon;
		private string strLeasingVertrNrBis;
		private string strkonzernnr;
		private string strkundennr;
		private bool blnKlaerfall;
		private string url;

		#endregion

		#region "Properties"

		public string PKennzeichenVon
		{
			get { return strKennzeichenVon; }
			set { strKennzeichenVon = value; }
		}

		public string PKennzeichenBis
		{
			get { return strKennzeichenBis; }
			set { strKennzeichenBis = value; }
		}

		public string PFahrgestellVon
		{
			get { return strFahrgestellVon; }
			set { strFahrgestellVon = value; }
		}

		public string PFahrgestellBis
		{
			get { return strFahrgestellBis; }
			set { strFahrgestellBis = value; }
		}

		public string PLeasingNrVon
		{
			get { return strLeasingVertrNrVon; }
			set { strLeasingVertrNrVon = value; }
		}

		public string PLeasingNrBis
		{
			get { return strLeasingVertrNrBis; }
			set { strLeasingVertrNrBis = value; }
		}

		public string PKundenNr
		{
			get { return strkundennr; }
			set { strkundennr = value; }
		}

		public string PKonzernNr
		{
			get { return strkonzernnr; }
			set { strkonzernnr = value; }
		}

		public bool PKlaerfall
		{
			get { return blnKlaerfall; }
			set { blnKlaerfall = value; }
		}
		#endregion

		public LP_03(User objUser, App objApp, string strFilename)
			: base(ref objUser, objApp, strFilename)
		{
			base.m_objUser = objUser;
			base.m_objApp = objApp;
			base.m_strFileName = strFilename;
		}

		#region " Methods"


		public DataTable getLangText(string strAppID, string strSessionID, Page page, string equi)
		{
			DataTable DatTable = new DataTable();

			if (!m_blnGestartet)
			{
				m_blnGestartet = true;

				int intID = -1;

				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Asl_Sis_Historie_Langtext", ref m_objApp, ref m_objUser, ref page);

					myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
					myProxy.setImportParameter("I_EQUNR", equi);

					myProxy.callBapi();

					DatTable = myProxy.getExportTable("GT_WEB_LANGTEXT");

				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
						case "NO_DATA":
							m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
							break;
						case "NO_PARAMETER":
							m_strMessage = "Eingabedaten nicht ausreichend.";
							break;
						case "NO_ASL":
							m_strMessage = "Falsche Kundennr.";
							break;
						case "NO_LANGTEXT":
							m_strMessage = "";
							break;
						default:
							m_intStatus = -9999;
							m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
							break;
					}
					WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
					//return DatTable;
				}
				finally
				{
					m_blnGestartet = false;
				}
				return DatTable;

			}
			else { return null; }
		}

		public void Fill(string strAppID, string strSessionID, Page page, string status, string type, Boolean mahn)
		{
			m_strClassAndMethod = "LP_03.FILL";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;

			if (!m_blnGestartet)
			{
				m_blnGestartet = true;

				try
				{
					string strKlaer = "";

					if( blnKlaerfall == true)
					{
						strKlaer = "X";
					}
					else
					{
						strKlaer = string.Empty;
					}
                
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Asl_Sis_Historie", ref m_objApp, ref m_objUser, ref page);

					myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10,'0'));
					myProxy.setImportParameter("I_LIZNR_HIGH", strLeasingVertrNrBis);
					myProxy.setImportParameter("I_LIZNR_LOW", strLeasingVertrNrVon);
					myProxy.setImportParameter("I_STATUS", status);
					myProxy.setImportParameter("I_KLAERFALL", strKlaer);
					myProxy.setImportParameter("I_KONZS_ZL", strkundennr);
					myProxy.setImportParameter("I_KONZS_ZO", strkonzernnr);
					myProxy.setImportParameter("I_LICENSE_NUM_LOW", strKennzeichenVon);
					myProxy.setImportParameter("I_LICENSE_NUM_HIGH", strKennzeichenBis);
					myProxy.setImportParameter("I_CHASSIS_NUM_LOW", strFahrgestellVon);
					myProxy.setImportParameter("I_CHASSIS_NUM_HIGH", strFahrgestellBis);

					myProxy.callBapi();

					DataTable tblTemp2 = myProxy.getExportTable("GT_WEB");
					

                     if (mahn)
                     {
                         // Ticket #2015040810000397, MSc / MJe, 08.04.2015
                         //   " Der Standort 0001 darf nur dann ausgegrenzt werden wenn der Klärfallgrund (Feld ZZLABEL = KF1 oder KF2) ist. "
                         //   " In allen anderen Fällen sollen die Klärfälle trotzdem angezeigt werden. "
                        tblTemp2.DefaultView.RowFilter = "NOT (STORT = '0001' AND (ZZLABEL = 'KF1' OR ZZLABEL = 'KF2'))";
                        tblTemp2 = tblTemp2.DefaultView.ToTable();
                     }

					CreateOutPut(tblTemp2, strAppID);    //Spalten übersetzen

					tableNative = m_tblResult.Copy();
					tblTemp2 = m_tblResult.Copy();
				//#### Überflüssige Spalten löschen

                //Dim col As DataColumn
                //Dim row As DataRow
                //LVNr.,Angelegt,Beginn,Ende,VersandLN,RückgabeLN,VersandVG,RückgabeVG,Versicherung,Status,Klärfall,Info,Equipment merken
					if (type == "H")      //Historie
					{
						foreach (DataColumn col in tblTemp2.Columns)
						{
							if (col.ColumnName != "LVNr" &&
                                col.ColumnName != "Angelegt" &&
                                col.ColumnName != "Beginn" &&
                                col.ColumnName != "GeplEnde" &&
                                col.ColumnName != "Versand" &&
                                col.ColumnName != "Rueckgabe_LN" &&
                                col.ColumnName != "Versand_VG" &&
                                col.ColumnName != "Rueckgabe_VG" &&
                                col.ColumnName != "Versicherung" &&
                                col.ColumnName != "Status" &&
                                col.ColumnName != "Klaerfall" &&
                                col.ColumnName != "Info" &&
                                col.ColumnName != "Equipment") 
							{
								m_tblResult.Columns.Remove(col.ColumnName);
							}                        
						}
					}

                //LVNr.,Angelegt,Beginn,Ende,VersandLN,VersandVG,MahnstufeLN,Mahnstufe-VG,Mahndatum-LN*,Mahndatum-VG*,Status,Klärfall,Info,Equipment merken
					if (type == "M")      //Mahnungen
                    {
						foreach (DataColumn col in tblTemp2.Columns)
						{
							if (col.ColumnName != "LVNr" &&
                                col.ColumnName != "Angelegt" &&
                                col.ColumnName != "Beginn" &&
                                col.ColumnName != "GeplEnde" &&
                                col.ColumnName != "Versand" &&
                                col.ColumnName != "Versand_LN" &&
                                col.ColumnName != "Versand_VG" &&
                                col.ColumnName != "Mahnstufe_LN" &&
                                col.ColumnName != "Mahnstufe_VG" &&
                                col.ColumnName != "Mahndatum_LN" &&
                                col.ColumnName != "Mahndatum_VG" &&
                                col.ColumnName != "Status" &&
                                col.ColumnName != "Klaerfall" &&
                                col.ColumnName != "Info" &&
                                col.ColumnName != "Equipment")
							{
								m_tblResult.Columns.Remove(col.ColumnName);
							}                        
						}
                    }				

					if (type == "HM")      //Klärfälle
                    {
						foreach(DataColumn col in tblTemp2.Columns)
						{
							if (col.ColumnName != "LVNr" &&
                                col.ColumnName != "Angelegt" &&
                                col.ColumnName != "Beginn" &&
                                col.ColumnName != "GeplEnde" &&
                                col.ColumnName != "Versand_LN" &&
                                col.ColumnName != "Kundennummer" &&
                                col.ColumnName != "Name1" &&
                                col.ColumnName != "Kundenbetreuer" &&
                                col.ColumnName != "Klaerfall" &&
                                col.ColumnName != "Schluesselnr_Klaerfall" &&
                                col.ColumnName != "Equipment") 
							{
								m_tblResult.Columns.Remove(col.ColumnName);
							}
						}
					}

					WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + "KennzeichenVon=" + strKennzeichenVon + ", KennzeichenBis=" + 
						strKennzeichenBis + ", LeasingVertrNrVon=" + strLeasingVertrNrVon + ", LeasingVertrNrBis=" + strLeasingVertrNrBis + ", Status=" + 
						status,ref m_tblResult, false);
				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					switch(HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
						case "NO_DATA":
							m_intStatus = -1234;
							m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
							break;
						case "NO_PARAMETER":
							m_intStatus = -3333;
							m_strMessage = "Eingabedaten nicht ausreichend.";
							break;
						default:
							m_intStatus = -9999;
							m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
							break;
					}
					WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "KennzeichenVon=" + strKennzeichenVon + ", KennzeichenBis=" + strKennzeichenBis + 
						", LeasingVertrNrVon=" + strLeasingVertrNrVon + ", LeasingVertrNrBis=" + strLeasingVertrNrBis + ", Status=" + status,ref m_tblResult, false);
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}


		public void saveComments(string strAppID, string strSessionID, Page page, string equi, string c1, string c2, string c3, string c4)
		{
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;

				int intID = -1;

				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Fleet_Bem_Update_001", ref m_objApp, ref m_objUser, ref page);

					myProxy.setImportParameter("I_EQUNR", equi);
					myProxy.setImportParameter("I_ZBE04", c1);
					myProxy.setImportParameter("I_ZBE05", c2);
					myProxy.setImportParameter("I_ZBE06", c3);
					myProxy.setImportParameter("I_ZBE07", c4);


					myProxy.callBapi();

					WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);

				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
						case "NO_DATA":
							m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
							break;
						case "NO_UPDATE":
							m_strMessage = "Fehler bei Update.";
							break;
						default:
							m_intStatus = -9999;
							m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
							break;
					}
					WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}

		public string toShortDateString(string dat)
		{
			string result;

			try
			{
				result = Convert.ToDateTime(dat).ToShortDateString();
				return result;
			}
			catch
			{
				return String.Empty;
			}
		}
		#endregion
	}
}
