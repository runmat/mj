using System;
using System.Data;
using System.Globalization;
using ERPConnect;

namespace SAPExecutor
{
    /// <summary>
    /// Die SAPExecutor Klasse dient dem Verbindungaufbau und dem Datenaustausch in Richtung SAP.
    /// </summary>
    public class SAPExecutor
    {
        private string sConStr;
        private string _sESubrc;
        private string _sEMessage;
        private DataTable _dtGtMessage;
        private bool _bError = false;


        #region Properties

        /// <summary>
        /// Die SAPExecutor Klasse dient dem Verbindungaufbau und dem Datenaustausch in Richtung SAP.
        /// </summary>
        /// <param name="sapConnectionString">ConnectionString für das Ziel-SAP-System</param>
        public SAPExecutor(string sapConnectionString)
        {
            sConStr = sapConnectionString;
        }

        public string E_SUBRC
        {
            get { return _sESubrc; }
        }

        public string E_MESSAGE
        {
            get { return _sEMessage; }
        }

        public bool ErrorOccured
        {
            get { return _bError; }
        }

        #endregion


        /// <summary>
        /// Liefert ein DataTable-Objekt das dem Schema für die Execute-Anweisung entspricht.
        /// Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, FeldLänge}
        /// </summary>
        /// <returns>Ein leeres DataTable-Objekt</returns>
        public static DataTable getSAPExecutorTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("Fieldname", Type.GetType("System.String"));
            dt.Columns.Add("OutputField", Type.GetType("System.Boolean"));
            dt.Columns.Add("Data", Type.GetType("System.Object"));
            dt.Columns.Add("Fieldlength", Type.GetType("System.Int32"));

            return dt;
        }

        /// <summary>
        /// Übersetzt DateTime in SAPDate-String
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string MakeSAPDate(DateTime date)
        {
            var strSAPDate = "";
            var strDate = date.ToShortDateString();
            strSAPDate += strDate.Substring(6, 4); // Jahr
            strSAPDate += strDate.Substring(3, 2); // Monat
            strSAPDate += strDate.Substring(0, 2); // Tag

            return strSAPDate;
        }

        /// <summary>
        /// Übersetzt SAPDate-String in DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MakeDateTime(string date)
        {
            DateTime datSAPDate;
          
            //if (date.Length != 8 ||date == "00000000")
            //{
            //    return null;
            //}
            
            var strSAPYear = date.Substring(0, 4);   // Jahr
            var strSAPMonth = date.Substring(4, 2); // Monat
            var strSAPDay = date.Substring(6, 2);   // Tag

            var strSAPDate = strSAPDay + "." + strSAPMonth + "." + strSAPYear;

            if (!DateTime.TryParse(strSAPDate, out datSAPDate))
            {
                datSAPDate = new DateTime(1, 1, 1, 0, 0, 0);
            }

            return datSAPDate;
        }

        /// <summary>
        /// Führt den SAP-Aufruf mit den gegebenen Parametern unter Verwendung des ERP-Connectors aus.
        /// </summary>
        /// <param name="bapiName">Name des Bapis das aufgerufen werden soll.</param>
        /// <param name="fields">DataTable-Objekt das eine Auflistung der Felder enthält die übergeben werden sollen.
        /// Feldname: SAPFeldname als <code>String</code>, ParameterRichtung: Input=<code>false</code> / Output=<code>true</code> als <code>bool</code>, 
        /// Feldwert: Der Wert der an SAP übergeben werden soll als <code>Objekt</code>, daher sind auch DataTable Objekte möglich.,
        /// Feldlänge: Die Länge des SAP Feldes als <code>int</code> 0=unbestimmte Feldlänge.
        /// </param>
        public void ExecuteERP(string bapiName, ref DataTable fields)
        {
            ResetError();
            //SAPConnection con = new SAPConnection(sConStr);
            var con = new R3Connection(sConStr);
            var itemsCollect = new RFCTableCollection();
            LIC.SetLic("5DVZ5588DC-25444");
            //String sLog="";

            try
            {
                con.Open(false);
                con.Logging = true;

                RFCFunction func = con.CreateFunction(bapiName);
                DataRow[] importField = fields.Select("OutputField = 0");
                for (int i = 0; i < importField.Length; i++)
                {
                    var paraName = fields.Rows[i][0].ToString();
                    var exportPara = (bool)fields.Rows[i][1];
                    object tempParamValue = fields.Rows[i][2];
                    var blnTableFound = false;
                    var blnTableInExportsFound = false;
                    var blnStructFound = false;

                    foreach (RFCTable rfcT in func.Tables)
                    {
                        if (rfcT.Name == paraName)
                        {
                            blnTableFound = true;
                            break;
                        }
                    }

                    foreach (RFCParameter rfcParam in func.Exports)
                    {
                        if (rfcParam.Name == paraName && rfcParam.IsStructure())
                        {
                            blnStructFound = true;
                            break;
                        }

                        if (rfcParam.Name == paraName && rfcParam.IsTable())
                        {
                            blnTableInExportsFound = true;
                            break;
                        }
                    }

                    if (!blnTableFound && !blnStructFound && !blnTableInExportsFound)
                    {

                        var type = RFCTYPE.CHAR;

                        if (IsDate(tempParamValue.ToString()))
                        {
                            type = RFCTYPE.DATE;
                        }
                        else if (tempParamValue is DBNull)
                        {
                            tempParamValue = string.Empty;
                        }
                        else if (tempParamValue is int)
                        {
                            type = RFCTYPE.INT;
                        }

                        if (type == RFCTYPE.DATE)
                        {
                            if (tempParamValue.ToString() != "00000000" && tempParamValue.ToString() != "")
                            {
                                DateTime netDate;
                                DateTime.TryParse(tempParamValue.ToString(), out netDate);
                                tempParamValue = ConversionUtils.NetDate2SAPDate(netDate);
                            }
                            //else {tempParamValue = null;}
                        }

                        if (exportPara) //Exportparameter ja = add to Imports!! nein = add to Exports!!!
                        {
                            func.Imports[paraName].ParamValue = tempParamValue;
                        }
                        else
                        {
                            func.Exports[paraName].ParamValue = tempParamValue;
                        }
                    }
                    else
                    {
                        if (exportPara == false) //Importtabelle oder Importstruktur
                        {
                            //Schauen, ob die Importtabelle Bestandteil der SAP-Tabellen ist.
                            RFCStructure item;
                            if (blnStructFound)
                            {
                                if (func.Exports[paraName].IsStructure())
                                {
                                    item = func.Exports[paraName].ToStructure();
                                    var tempTable = (DataTable)tempParamValue;
                                    foreach (DataRow dr in tempTable.Rows)
                                    {
                                        InsertValues(ref item, dr);
                                        func.Exports[paraName].ParamValue = item;
                                    }
                                }

                            }

                            else if (blnTableInExportsFound)
                            {
                                RFCTable items = func.Exports[paraName].ToTable();

                                var tempTable = (DataTable)tempParamValue;
                                foreach (DataRow dr in tempTable.Rows)
                                {
                                    item = items.AddRow();
                                    InsertValues(ref item, dr);
                                    itemsCollect.Add(items);
                                }
                            }
                            else if (blnTableFound)
                            {
                                RFCTable items = func.Tables[paraName];

                                var tempTable = (DataTable)tempParamValue;
                                foreach (DataRow dr in tempTable.Rows)
                                {
                                    item = items.AddRow();
                                    InsertValues(ref item, dr);
                                    itemsCollect.Add(items);
                                }
                            }
                        }
                    }
                }

                func.Execute();

                var blnHasSubrc = false;
                var blnHasMessage = false;
                var blnHasMessageTable = false;

                _sESubrc = "0";
                _sEMessage = String.Empty;

                foreach (RFCParameter para in func.Imports)
                {
                    if (para.Name == "E_SUBRC") { blnHasSubrc = true; }
                    if (para.Name == "E_MESSAGE") { blnHasMessage = true; }
                }

                if (blnHasSubrc)
                {
                    if (func.Imports["E_SUBRC"].ToInt32() != 0)
                    {
                        _sESubrc = func.Imports["E_SUBRC"].ToString();
                        if (_sESubrc != "0")
                        {
                            _bError = true;
                        }
                    }
                    if (blnHasMessage)
                    {
                        if (func.Imports["E_MESSAGE"].ToString() != string.Empty)
                        {
                            _sEMessage = func.Imports["E_MESSAGE"].ToString().TrimEnd(' ');
                        }
                    }
                }

                foreach (RFCTable tbl in func.Tables)
                {
                    if (tbl.Name == "GT_MESSAGE")
                    {
                        blnHasMessageTable = true;
                    }
                }

                if (blnHasMessageTable)
                {
                    if (func.Tables["GT_MESSAGE"].RowCount > 0)
                    {
                        _dtGtMessage = func.Tables["GT_MESSAGE"].ToADOTable();
                        if (_dtGtMessage.Rows.Count > 0)
                        {
                            _bError = true;
                        }
                    }
                }
                //Export-Werte auswerten
                foreach (RFCParameter par in func.Imports)
                {
                    DataRow[] rows = fields.Select("Fieldname = '" + par.Name + "'");
                    if (rows.GetLength(0) > 0)
                    {
                        if (par.IsStructure())
                        {
                            rows[0][2] = ConvertStructureValues(func.Imports[par.Name].ToStructure(), par.Name);
                        }
                        else if (par.IsTable())
                        {
                            rows[0][2] = ConvertTableValues(func.Tables[par.Name]);
                        }
                        else
                        {
                            rows[0][2] = par.ParamValue;

                            // Umstellung erst nach ausführlichem Test
                            // rows[0][2] = ConvertParameterValue(par);
                        }

                    }
                }
                //Tabellen auswerten
                foreach (RFCTable tbl in func.Tables)
                {
                    DataRow[] rows = fields.Select("Fieldname = '" + tbl.Name + "' AND OutputField = 1");
                    if (rows.GetLength(0) > 0)
                    {
                        //ConvertTableValues(func, tbl.Name);
                        rows[0][2] = ConvertTableValues(func.Tables[tbl.Name]);
                    }
                }
            }
            catch (Exception ex)
            {
                _bError = true;
                _sESubrc = "-9999";
                _sEMessage = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void ResetError()
        {
            _bError = false;
            _sESubrc = "0";
            _sEMessage = string.Empty;
            if (_dtGtMessage != null)
            {
                _dtGtMessage.Clear();
            }
        }

        private static void InsertValues(ref RFCStructure item, DataRow dr)
        {

            foreach (RFCTableColumn col in item.Columns)
            {
                switch (col.Type)
                {
                    case RFCTYPE.NUM:
                        if (!(dr[col.Name] is DBNull))
                        {
                            item[col.Name] = dr[col.Name].ToString();
                        }
                        break;
                    case RFCTYPE.INT:
                        if (!(dr[col.Name] is DBNull))
                        {
                            item[col.Name] = dr[col.Name].ToString();
                        }
                        break;
                    case RFCTYPE.CHAR:
                        item[col.Name] = dr[col.Name].ToString();
                        break;
                    case RFCTYPE.BCD:
                        if (!(dr[col.Name] is DBNull))
                        {
                            //If IsNumeric(dr(col.Name).ToString) Then
                            decimal dec;
                            decimal.TryParse(dr[col.Name].ToString(), out dec);
                            item[col.Name] = dec;
                            //End If
                        }
                        break;
                    case RFCTYPE.DATE:
                        if (dr[col.Name].ToString() == "00000000" || dr[col.Name].ToString() == "")
                        {
                            item[col.Name] = "";
                        }
                        else
                        {
                            DateTime dat;
                            var deDe = new CultureInfo("de-DE");
                            if (DateTime.TryParseExact(dr[col.Name].ToString(), "dd/mm/yyyy", deDe, DateTimeStyles.AssumeLocal, out dat))
                            {
                                item[col.Name] = ConversionUtils.NetDate2SAPDate(dat);
                            }
                        }
                        break;
                    case RFCTYPE.TIME:
                        if (dr[col.Name].ToString() == "000000" || dr[col.Name].ToString() == "")
                        {
                            item[col.Name] = "";
                        }
                        else
                        {
                            item[col.Name] = dr[col.Name].ToString();
                        }
                        break;
                }
            }
        }

        private static DataTable ConvertStructureValues(RFCStructure item, String structureName)
        {
            var tblTemp = new DataTable { TableName = structureName };
            foreach (RFCTableColumn col in item.Columns)
            {
                tblTemp.Columns.Add(col.Name, typeof(System.String));
            }

            var row = tblTemp.NewRow();
            foreach (RFCTableColumn col in item.Columns)
            {
                switch (col.Type)
                {
                    case RFCTYPE.NUM:
                        row[col.Name] = item[col.Name].ToString();
                        break;
                    case RFCTYPE.INT:
                        row[col.Name] = item[col.Name].ToString();
                        break;
                    case RFCTYPE.CHAR:
                        row[col.Name] = item[col.Name].ToString();
                        break;
                    case RFCTYPE.BCD:
                        if ((item[col.Name] is DBNull))
                        {
                            row[col.Name] = "";
                        }
                        else
                        {
                            row[col.Name] = item[col.Name];
                        }
                        break;
                    case RFCTYPE.DATE:
                        if (item[col.Name].ToString() == "00000000" || item[col.Name].ToString() == "")
                        {
                            row[col.Name] = "";
                        }
                        else
                        {
                            row[col.Name] = ConversionUtils.SAPDate2NetDate(item[col.Name].ToString()).ToShortDateString();
                        }
                        break;
                    case RFCTYPE.TIME:
                        if (item[col.Name].ToString() == "000000" || item[col.Name].ToString() == "")
                        {
                            row[col.Name] = "";
                        }
                        else
                        {
                            row[col.Name] = item[col.Name].ToString();
                        }
                        break;
                }
            }
            tblTemp.Rows.Add(row);
            tblTemp.AcceptChanges();
            return tblTemp;
        }

        private static DataTable ConvertTableValues(RFCTable item)
        {
            DataTable tblTemp = null;
            tblTemp = item.ToADOTable();

            foreach (RFCTableColumn col in item.Columns)
            {
                foreach (DataRow row in tblTemp.Rows)
                {
                    switch (col.Type)
                    {
                        case RFCTYPE.NUM:
                            row[col.Name] = row[col.Name].ToString();
                            break;
                        case RFCTYPE.INT:
                            row[col.Name] = row[col.Name].ToString();
                            break;
                        case RFCTYPE.CHAR:
                            row[col.Name] = row[col.Name].ToString();
                            break;
                        case RFCTYPE.BCD:
                            if ((row[col.Name] is DBNull))
                            {
                                row[col.Name] = "";
                            }
                            break;
                        case RFCTYPE.DATE:
                            if (row[col.Name].ToString() == "00000000" || row[col.Name].ToString() == "")
                            {
                                row[col.Name] = "";
                            }
                            else
                            {
                                row[col.Name] = ConversionUtils.SAPDate2NetDate(row[col.Name].ToString()).ToShortDateString();
                            }
                            break;
                        case RFCTYPE.TIME:
                            if (row[col.Name].ToString() == "000000" || row[col.Name].ToString() == "")
                            {
                                row[col.Name] = "";
                            }
                            else
                            {
                                row[col.Name] = row[col.Name].ToString();
                            }
                            break;
                    }
                }

            }
            tblTemp.AcceptChanges();
            return tblTemp;
        }

        private static object ConvertParameterValue(RFCParameter par)
        {
            object result = null;

            switch (par.Type)
            {
                case RFCTYPE.NUM:
                    result = par.ParamValue.ToString();
                    break;
                case RFCTYPE.INT:
                    result = par.ParamValue.ToString();
                    break;
                case RFCTYPE.CHAR:
                    result = par.ParamValue.ToString();
                    break;
                case RFCTYPE.BCD:
                    result = par.ParamValue.ToString();
                    break;
                case RFCTYPE.DATE:
                    if (par.ParamValue.ToString() == "00000000" || par.ParamValue.ToString() == "")
                    {
                        result = "";
                    }
                    else
                    {
                        result = ConversionUtils.SAPDate2NetDate(par.ParamValue.ToString()).ToShortDateString();
                    }
                    break;
                case RFCTYPE.TIME:
                    if (par.ParamValue.ToString() == "000000" || par.ParamValue.ToString() == "")
                    {
                        result = "";
                    }
                    else
                    {
                        result = par.ParamValue.ToString();
                    }
                    break;
            }

            return result;
        }

        public static bool IsDate(String inValue)
        {
            bool result;

            try
            {
                DateTime myDt;
                result = DateTime.TryParse(inValue, out myDt);
            }
            catch (FormatException e)
            {
                result = false;
            }

            return result;
        }
    }
}
