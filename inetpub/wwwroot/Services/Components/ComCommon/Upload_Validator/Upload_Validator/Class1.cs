using System;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web;
using CKG.Base.Kernel.Security;

namespace Upload_Validator
{
    public class Validator
    {
        //finde Deutsches Standard Kennzeichen
        static string pat = "[a-zäöüA-ZÄÖÜ]{1,3}-[a-zA-Z]{0,2}[0-9]{1,4}";
        static Regex r = new Regex(pat, RegexOptions.IgnoreCase);
        static Match m;

        public DataTable UploadXLSDatei(HttpPostedFile uFile, string ExcelPath, User m_User, ref System.Web.UI.WebControls.Label lblerror, string AppID, string SessionID)
        {
            DataTable tblTemp = null;

            string FileExtension = "";

            if (uFile != null)
            {   
                if (uFile.FileName == "")
                {
                    lblerror.Text = "Bitte wählen Sie eine Datei aus.";
                    return new DataTable();
                }
                else if (uFile.FileName.ToUpper().Substring(uFile.FileName.Length - 4) == ".XLS")
                {
                    FileExtension = ".xls";
                }
                else if (uFile.FileName.ToUpper().Substring(uFile.FileName.Length - 5) == ".XLSX")
                {
                    FileExtension = ".xlsx";
                }
                
                else
                {
                    lblerror.Text = "Bitte verwenden Sie nur Exceldateien(xls oder xlsx).";
                    return new DataTable();
                }
            }
            else
            {
                lblerror.Text = "Bitte wählen Sie eine Datei aus.";
                return new DataTable();       
            }
   
            string filepath = ExcelPath;
            string filename = null;
            System.IO.FileInfo info = null;

            //Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + FileExtension;

            if ((uFile != null))
            {
                uFile.SaveAs(ExcelPath + filename);
                info = new System.IO.FileInfo(filepath + filename);
                if (!(info.Exists))
                {
                    lblerror.Text = "Fehler beim Speichern.";
                    return new DataTable();
                }

                //Datei gespeichert -> Auswertung

                string sConnectionString = "";

                if (FileExtension == ".xls")
                {
                    sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + filename + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }
                else
                {
                    sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                }
                
                OleDbConnection objConn = new OleDbConnection(sConnectionString);
                objConn.Open();

                string[] Tabellenname = null;
                int i = 0;
                while (i < objConn.GetSchema("Tables").Rows.Count)
                {
                    Array.Resize(ref Tabellenname, i + 1);
                    Tabellenname[i] = objConn.GetSchema("Tables").Rows[i]["TABLE_NAME"].ToString();
                    i = i + 1;
                }

                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                DataSet objDataset1 = new DataSet();

                //herausfinden in welchem Sheet Daten sind, bei mehr als einem gefülltem Sheet erfolgt eine Fehlermeldung
                int SheetMitDatenCounter = 0;
                i = 0;
                while (i < objConn.GetSchema("Tables").Rows.Count)
                {
                    OleDbCommand objCmdSelect = new OleDbCommand(string.Format("SELECT * FROM [{0}]", Tabellenname[i]), objConn);
                    objAdapter1 = new OleDbDataAdapter();
                    objAdapter1.SelectCommand = objCmdSelect;

                    objDataset1 = new DataSet();
                    try
                    {
                        objAdapter1.Fill(objDataset1, "XLData");
                    }
                    catch (OleDbException)
                    {
                        i++;
                        continue;
                    }

                    if (objDataset1.Tables[0].Rows.Count > 1)
                    {
                        tblTemp = objDataset1.Tables[0];
                        SheetMitDatenCounter += 1;
                    }

                    i++;
                }

                if (SheetMitDatenCounter > 1)
                {
                    lblerror.Text += "Datei enthielt mehrere gefüllte Tabellen.<br>";
                }

                objConn.Close();

            }
            return tblTemp;
        }

        public DataTable UploadFahrgestellnummern(HttpPostedFile uFile, string ExcelPath, User m_User, ref System.Web.UI.WebControls.Label lblerror, string AppID, string SessionID)
        {
            DataTable tblTemp = null;
            tblTemp = UploadXLSDatei(uFile, ExcelPath, m_User, ref  lblerror, AppID, SessionID);
            if (tblTemp != null && lblerror.Text.IndexOf("Datei enthielt mehrere gefüllte Tabellen.<br>") < 0)
            {
                foreach (DataRow xrow in tblTemp.Rows)
                {
                    xrow[0] = VIN_bereinigen(xrow[0].ToString());
                }

                return tblTemp;

            }
            return new DataTable();
        }

        public DataTable UploadXLSohneModifikation(HttpPostedFile uFile, string ExcelPath, User m_User, ref System.Web.UI.WebControls.Label lblerror, string AppID, string SessionID)
        {
            DataTable tblTemp = null;
            tblTemp = UploadXLSDatei(uFile, ExcelPath, m_User, ref  lblerror, AppID, SessionID);
            if (tblTemp != null && lblerror.Text.IndexOf("Datei enthielt mehrere gefüllte Tabellen.<br>") < 0)
            {
                return tblTemp;
            }
            return new DataTable();
        }

        public string VIN_bereinigen(string strIn)
        {
            string Zahlenreihe = "";
            char[] myChars = strIn.ToCharArray();
            foreach (char ch in myChars)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    Zahlenreihe += ch;
                }
            }
            return Zahlenreihe;
        }

        public int FindeSpalteMitDeutschemKennzeichen(DataRow xrow)
        {
            int i = 0;
            //finde Deutsches Standard Kennzeichen

            for (i = 0; i <= xrow.Table.Columns.Count - 1; i++)
            {
                m = r.Match(xrow[i].ToString());
                if (m.Success)
                {
                    return i;
                }
            }
            //wenn kein Kennzeichen gefunden wird, wird -1 zurückgegeben    
            return -1;
        }

        public string FindeDeutschesKennzeichen(DataRow xrow)
        {
            int i = 0;
            //finde Deutsches Standard Kennzeichen

            for (i = 0; i <= xrow.Table.Columns.Count - 1; i++)
            {
                m = r.Match(xrow[i].ToString());
                if (m.Success)
                {
                    return xrow[i].ToString();
                }
            }
            //wenn kein Kennzeichen gefunden wird, wird "" zurückgegeben    
            return "";
        }

        public int CheckObZeilenMitMehrAlsEinemWertExistieren(DataTable xTable)
        {
            int SpalteMitInhaltCounter = 0;
            int i = 0;
            foreach (DataRow xrow in xTable.Rows)
            {
                SpalteMitInhaltCounter = 0;
                i = 0;
                for (i = 0; i <= xrow.Table.Columns.Count - 1; i++)
                {
                    if (xrow[i].ToString().Length > 1)
                    {
                        SpalteMitInhaltCounter++;
                    }
                }
                if (SpalteMitInhaltCounter > 1 )
                {
                    return SpalteMitInhaltCounter;
                }
            }
            //wenn kein Kennzeichen gefunden wird, wird -1 zurückgegeben    
            return -1;
        }
    }
}
