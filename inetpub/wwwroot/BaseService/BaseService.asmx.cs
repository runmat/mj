using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;

namespace BaseService
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für Service1
    /// </summary>
    [WebService(Namespace = "http://kroschke.de/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf. 
    // [System.Web.Script.Services.ScriptService]
    public class BaseService : System.Web.Services.WebService
    {

        string mUsername;
        string mUserreferenz;

        private const string Numbers = "0123456789";

        [WebMethod]
        public StatusC Beauftragungen(string Username, string Password, Zulassungen impZulassungen)
        //public StatusC Beauftragungen(string Username, string Password, string Userreferenz)
        {
            StatusC RetStatus = new StatusC();
            Status RetDetail;

            mUsername = Username;



            if (ValidateUser(Username,Password) == false)
            {
                RetDetail = new Status();

                RetDetail.FGNU = "";
                RetDetail.ERROR = true;
                RetDetail.ERRORDESC = "Anmeldedaten nicht korrekt.";

                RetStatus.Add(RetDetail);

                return RetStatus;

            }


            if (mUserreferenz.Length != 8)
            {
                RetDetail = new Status();

                RetDetail.FGNU = "";
                RetDetail.ERROR = true;
                RetDetail.ERRORDESC = "Userreferenz nicht achtstellig.";

                RetStatus.Add(RetDetail);

                return RetStatus;

            }

            for (int i = 0; i < 8; i++)
            {
                if (Numbers.Contains(mUserreferenz.Substring(i, 1)) == false)
                {
                    RetDetail = new Status();

                    RetDetail.FGNU = "";
                    RetDetail.ERROR = true;
                    RetDetail.ERRORDESC = "Ungueltige Userreferenz.";

                    RetStatus.Add(RetDetail);

                    return RetStatus;
                }
            }



            Zulassungen SapZulassungen = new Zulassungen();
           

            foreach (Zulassung item in impZulassungen)
            {

                string Err = ValidateImpZulassung(item);

                if (Err != "")
                {

                    //Fehlermeldung in Returntable eintragen
                    RetDetail = new Status();

                    RetDetail.FGNU = item.FGNU;
                    RetDetail.ERROR = true;
                    RetDetail.ERRORDESC = Err;

                    RetStatus.Add(RetDetail);

                }
                else
                {
                    RetDetail = new Status();

                    RetDetail.FGNU = item.FGNU;
                    RetDetail.ERROR = false;
                    RetDetail.ERRORDESC = "";

                    RetStatus.Add(RetDetail);

                    //Alle Daten in Ordnung

                    item.ZZHALTER = "";


                    SapZulassungen.Add(item);

                   


                }


            }


            //An SAP senden

            if (SapZulassungen.Count > 0)
            {
                DataTable tblTemp = SendToSAP2(SapZulassungen);

                foreach (DataRow dr in tblTemp.Rows)
                {

                    if (dr["FEHLERTEXT"].ToString() != string.Empty)
                    {
                        foreach (Status CheckDetail in RetStatus)
                        {
                            if (dr["FGNU"].ToString() == CheckDetail.FGNU.ToString())
                            {
                                CheckDetail.ERROR = true;
                                CheckDetail.ERRORDESC = dr["FEHLERTEXT"].ToString();
                                break;
                            }
                        }
                    }


                }
            }

            

            return RetStatus;
        }

        bool ValidateUser(string Username, string Password)
        {


            //User, Password abfragen

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                

                con.Open();


                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select Reference from WebUser where Username = @Username and Password = @Password and AccountIsLockedOut = @Locked and Approved = @Approved");

                cmd.Connection = con;

                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "sha1"));
                cmd.Parameters.AddWithValue("@Locked", false);
                cmd.Parameters.AddWithValue("@Approved", true);


                DataTable tblReturn = new DataTable();

                System.Data.SqlClient.SqlDataAdapter UserAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

                UserAdapter.Fill(tblReturn);

                con.Close();


                if (tblReturn.Rows.Count < 1)
                {
                    return false;
                }
                else
                {
                    mUserreferenz = tblReturn.Rows[0]["Reference"].ToString();
                }
                

            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
           
            
            return true;
        }


        string ValidateImpZulassung(Zulassung valItem)
        {

            string ErrDesc = "";

            try
            {

                if (valItem.KREIS == string.Empty)
                {
                    ErrDesc = "KREIS nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.KUNNR == string.Empty)
                {
                    ErrDesc = "KUNNR nicht gefuellt.";
                    return ErrDesc;
                }


                
                if (valItem.MATNR == string.Empty)
                {
                    ErrDesc = "MATNR nicht gefuellt.";
                    return ErrDesc;
                }

                
                //if (valItem.ZZKENN == string.Empty)
                //{
                //    ErrDesc = "ZZKENN nicht gefuellt.";
                //    return ErrDesc;
                //}

                
                if (valItem.ZZZLDAT == string.Empty)
                {
                    ErrDesc = "ZZZLDAT nicht gefuellt.";
                    return ErrDesc;
                }


                
                if (valItem.KENNZTYP == string.Empty)
                {
                    ErrDesc = "KENNZTYP nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.ZZFAHRG == string.Empty)
                {
                    ErrDesc = "ZZFAHRG nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.FGNU == string.Empty)
                {
                    ErrDesc = "FGNU nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.BRNR == string.Empty)
                {
                    ErrDesc = "BRNR nicht gefuellt.";
                    return ErrDesc;
                }


                if (valItem.ZZZLDAT == string.Empty)
                {
                    ErrDesc = "ZZZLDAT nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.HERS == string.Empty)
                {
                    ErrDesc = "HERS nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.TYP == string.Empty)
                {
                    ErrDesc = "TYP nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.VVS == string.Empty)
                {
                    ErrDesc = "VVS nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.TYPZ == string.Empty)
                {
                    ErrDesc = "TYPZ nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.FGPZ == string.Empty)
                {
                    ErrDesc = "FGPZ nicht gefuellt.";
                    return ErrDesc;
                }

                //******Barcode prüfen******
                
                if (valItem.ZZREFERENZCODE == string.Empty)
                {
                    ErrDesc = "ZZREFERENZCODE nicht gefuellt.";
                    return ErrDesc;
                }
                if (CheckBarcode(valItem.ZZREFERENZCODE) == false)
                {
                    ErrDesc = "ZZREFERENZCODE(Barcode) nicht korrekt."; return ErrDesc;

                }
                //*******
                
                if (valItem.ZZGROSSKUNDNR == string.Empty)
                {
                    ErrDesc = "ZZGROSSKUNDNR nicht gefuellt.";
                    return ErrDesc;
                }

                
                if (valItem.ZZEVB == string.Empty)
                {
                    ErrDesc = "ZZEVB nicht gefuellt.";
                    return ErrDesc;
                }
            }
            catch (Exception)
            {

                return ErrDesc;
            }

            return ErrDesc;

        }


        static bool isDate(string DateString)
        {
            try
            {
                    Convert.ToDateTime(DateString);
                    return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        bool CheckBarcode(string Barcode)
        {

            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings["ErpConnectLicense"]);

            ERPConnect.R3Connection SapCon = new ERPConnect.R3Connection(ConfigurationManager.AppSettings["SAPAppServerHost"],
                                                                        Convert.ToInt16(ConfigurationManager.AppSettings["SAPSystemNumber"]),
                                                                        ConfigurationManager.AppSettings["SAPUsername"], 
                                                                        ConfigurationManager.AppSettings["SAPPassword"], 
                                                                        "DE",
                                                                        ConfigurationManager.AppSettings["SAPClient"]);

            SapCon.Open();

            ERPConnect.RFCFunction func = SapCon.CreateFunction("Z_ZLD_STATUS_REFCODE");
          
            ERPConnect.RFCParameter pBarcode = func.Exports["I_ZZREFERENZCODE"];
            ERPConnect.RFCParameter pReturn = func.Imports["E_RETURNCODE"];


            pBarcode.ParamValue = Barcode;

            func.Execute();
            

            SapCon.Close();

            if ((string)pReturn.ParamValue == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }


        DataTable SendToSAP(Zulassungen Zul)
        {
            

            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings["ErpConnectLicense"]);

            ERPConnect.R3Connection SapCon = new ERPConnect.R3Connection(ConfigurationManager.AppSettings["SAPAppServerHost"],
                                                                        Convert.ToInt16(ConfigurationManager.AppSettings["SAPSystemNumber"]),
                                                                        ConfigurationManager.AppSettings["SAPUsername"],
                                                                        ConfigurationManager.AppSettings["SAPPassword"],
                                                                        "DE",
                                                                        ConfigurationManager.AppSettings["SAPClient"]);

            SapCon.Open();

            ERPConnect.RFCFunction func = SapCon.CreateFunction("Z_ZLD_VORERFASSUNG_EKFZ");

            ERPConnect.RFCTable IT_BELEG = func.Tables["IT_BELEG"];
            
            ERPConnect.RFCTable Vorerfassung = func.Tables["I_VORERFASSUNG_EKFZ_01"];
            
            ERPConnect.RFCTable ExTable = func.Tables["ET_FEHLER"];

            Int32 NewID;

            
            Zulassungen TempZulassungen = new Zulassungen();
            Zulassung TemZulassung;

            DataTable tblTemp = new DataTable();

            ERPConnect.RFCTableCollection itemsCollect = new ERPConnect.RFCTableCollection();
            ERPConnect.RFCStructure BelegRow;
            ERPConnect.RFCStructure VorerfassungRow;
            
            


            foreach (Zulassung item in Zul)
            {


                NewID = DBGiveNewZulassungsID();

                TemZulassung = new Zulassung();

                TemZulassung.ABE = NewID.ToString();
                TemZulassung.FGNU = item.FGNU;

                TempZulassungen.Add(TemZulassung);

                BelegRow = new ERPConnect.RFCStructure(IT_BELEG.Columns);
                BelegRow = IT_BELEG.AddRow();


                BelegRow["ID"] = NewID;
                BelegRow["VKORG"] = mUserreferenz.Substring(0, 4);
                BelegRow["VKBUR"] = mUserreferenz.Substring(4, 4);
                BelegRow["KREIS"] = item.KREIS;
                BelegRow["KUNNR"] = item.KUNNR.PadLeft(10,'0');
                BelegRow["MATNR"] = item.MATNR.PadLeft(18,'0');

                if (item.ZZKENN == string.Empty)
                {
                    item.ZZKENN = item.KREIS + "-";
                }

                BelegRow["ZZKENN"] = item.ZZKENN;
                BelegRow["ZZHALTER"] = item.ZZHALTER;
                BelegRow["ZZZLDAT"] = item.ZZZLDAT;
                BelegRow["RESERVIERT"] = item.RESERVIERT;
                BelegRow["RESERVID"] = item.RESERVID;
                BelegRow["KENNZTYP"] = item.KENNZTYP;
                BelegRow["EINKZ"] = item.EINKZ;
                BelegRow["ZZFAHRG"] = item.ZZFAHRG;
                BelegRow["ZZWUNSCH"] = item.ZZWUNSCH;
                BelegRow["ZZTEXT"] = item.ZZTEXT;
                BelegRow["NAME1"] = item.NAME1;
                BelegRow["NAME2"] = item.NAME2;
                BelegRow["STRAS"] = item.STRAS;
                BelegRow["PSTLZ"] = item.PSTLZ;
                BelegRow["ORT01"] = item.ORT01;
                BelegRow["ZVERKAEUFER"] = item.ZVERKAEUFER;
                BelegRow["ZKUNDREF"] = item.ZKUNDREF;
                BelegRow["ZKUNDNOTIZ"] = item.ZKUNDNOTIZ;
                BelegRow["ZFEINSTAUB_KZ"] = item.ZFEINSTAUB_KZ;
                BelegRow["ZKRAD_KZ"] = item.ZKRAD_KZ;
                BelegRow["USERNAME"] = mUsername;


                VorerfassungRow = new ERPConnect.RFCStructure(Vorerfassung.Columns);
                

                VorerfassungRow = Vorerfassung.AddRow();

                VorerfassungRow["ID"] = NewID;
                VorerfassungRow["FGNU"] = item.FGNU;
                VorerfassungRow["ANR"] = item.ANR;
                VorerfassungRow["VNAM"] = item.VNAM;
                VorerfassungRow["RNAM"] = item.RNAM;
                VorerfassungRow["FNAM"] = item.FNAM;
                VorerfassungRow["FNAMB"] = item.FNAMB;
                VorerfassungRow["STRN"] = item.STRN;
                VorerfassungRow["STRH"] = item.STRH;
                VorerfassungRow["STRB"] = item.STRB;
                VorerfassungRow["PLZ"] = item.PLZ;
                VorerfassungRow["ORT"] = item.ORT;
                VorerfassungRow["BRNR"] = item.BRNR;
                VorerfassungRow["STDA"] = item.STDA;
                VorerfassungRow["EZU"] = item.EZU;
                VorerfassungRow["ZUDA"] = item.ZZZLDAT;
                VorerfassungRow["HERS"] = item.HERS;
                VorerfassungRow["TYP"] = item.TYP;
                VorerfassungRow["VVS"] = item.VVS;
                VorerfassungRow["TYPZ"] = item.TYPZ;
                VorerfassungRow["FGPZ"] = item.FGPZ;
                VorerfassungRow["ABE"] = item.ABE;
                VorerfassungRow["NAHU"] = item.NAHU;
                VorerfassungRow["ASU"] = item.ASU;
                VorerfassungRow["ZB1_NR"] = item.ZB1_NR;
                VorerfassungRow["ZZREFERENZCODE"] = item.ZZREFERENZCODE;
                VorerfassungRow["ZZGROSSKUNDNR"] = item.ZZGROSSKUNDNR;
                VorerfassungRow["ZZEVB"] = item.ZZEVB;
                VorerfassungRow["KONTO"] = item.KONTO;
                VorerfassungRow["BLZ"] = item.BLZ;
                VorerfassungRow["GEBDAT"] = item.GEBDAT;
                VorerfassungRow["GEBORT"] = item.GEBORT;

                

            }

            itemsCollect.Add(IT_BELEG);
            itemsCollect.Add(Vorerfassung);

            try
            {
                func.Execute();
                tblTemp = ExTable.ToADOTable();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SapCon.Close();
            }

           
            tblTemp.Columns.Add("FGNU",typeof(string));

            tblTemp.AcceptChanges();

            foreach (DataRow dt in tblTemp.Rows)
            {

                foreach (Zulassung item in TempZulassungen)
                {

                    if (dt["ID"].ToString() == item.ABE.PadLeft(10,'0'))
                    {
                        dt["FGNU"] = item.FGNU.ToString();
                        break;
                    }
                }

             
            }


            return tblTemp;

        }

        DataTable SendToSAP2(Zulassungen Zul)
        {


            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings["ErpConnectLicense"]);

            ERPConnect.R3Connection SapCon = new ERPConnect.R3Connection(ConfigurationManager.AppSettings["SAPAppServerHost"],
                                                                        Convert.ToInt16(ConfigurationManager.AppSettings["SAPSystemNumber"]),
                                                                        ConfigurationManager.AppSettings["SAPUsername"],
                                                                        ConfigurationManager.AppSettings["SAPPassword"],
                                                                        "DE",
                                                                        ConfigurationManager.AppSettings["SAPClient"]);

            SapCon.Open();

            ERPConnect.RFCFunction func = SapCon.CreateFunction("Z_ZLD_IMPORT_VORERFASSUNG_EKFZ");

            ERPConnect.RFCTable IT_BELEG = func.Tables["IT_BELEG"];

            ERPConnect.RFCTable ExTable = func.Tables["ET_FEHLER"];

            Int32 NewID;


            Zulassungen TempZulassungen = new Zulassungen();
            Zulassung TemZulassung;

            DataTable tblTemp = new DataTable();

            ERPConnect.RFCTableCollection itemsCollect = new ERPConnect.RFCTableCollection();
            ERPConnect.RFCStructure BelegRow;
            ERPConnect.RFCStructure VorerfassungRow;




            foreach (Zulassung item in Zul)
            {


                //NewID = DBGiveNewZulassungsID();

                //TemZulassung = new Zulassung();

                //TemZulassung.ABE = NewID.ToString();
                //TemZulassung.FGNU = item.FGNU;

                //TempZulassungen.Add(TemZulassung);

                BelegRow = new ERPConnect.RFCStructure(IT_BELEG.Columns);
                BelegRow = IT_BELEG.AddRow();


                //BelegRow["ID"] = NewID;
                BelegRow["VKORG"] = mUserreferenz.Substring(0, 4);
                BelegRow["VKBUR"] = mUserreferenz.Substring(4, 4);
                BelegRow["KREIS"] = item.KREIS;
                BelegRow["KUNNR"] = item.KUNNR.PadLeft(10, '0');
                BelegRow["MATNR"] = item.MATNR.PadLeft(18, '0');

                if (item.ZZKENN == string.Empty)
                {
                    item.ZZKENN = item.KREIS + "-";
                }

                BelegRow["ZZKENN"] = item.ZZKENN;
                BelegRow["ZZHALTER"] = item.ZZHALTER;
                BelegRow["ZZZLDAT"] = item.ZZZLDAT;
                BelegRow["RESERVIERT"] = item.RESERVIERT;
                BelegRow["RESERVID"] = item.RESERVID;
                BelegRow["KENNZTYP"] = item.KENNZTYP;
                BelegRow["EINKZ"] = item.EINKZ;
                BelegRow["ZZFAHRG"] = item.ZZFAHRG;
                BelegRow["ZZWUNSCH"] = item.ZZWUNSCH;
                BelegRow["ZZTEXT"] = item.ZZTEXT;
                BelegRow["NAME1"] = item.NAME1;
                BelegRow["NAME2"] = item.NAME2;
                BelegRow["STRAS"] = item.STRAS;
                BelegRow["PSTLZ"] = item.PSTLZ;
                BelegRow["ORT01"] = item.ORT01;
                BelegRow["ZVERKAEUFER"] = item.ZVERKAEUFER;
                BelegRow["ZKUNDREF"] = item.ZKUNDREF;
                BelegRow["ZKUNDNOTIZ"] = item.ZKUNDNOTIZ;
                BelegRow["ZFEINSTAUB_KZ"] = item.ZFEINSTAUB_KZ;
                BelegRow["ZKRAD_KZ"] = item.ZKRAD_KZ;
                BelegRow["USERNAME"] = mUsername;

                BelegRow["FGNU"] = item.FGNU;
                BelegRow["ANR"] = item.ANR;
                BelegRow["VNAM"] = item.VNAM;
                BelegRow["RNAM"] = item.RNAM;
                BelegRow["FNAM"] = item.FNAM;
                BelegRow["FNAMB"] = item.FNAMB;
                BelegRow["STRN"] = item.STRN;
                BelegRow["STRH"] = item.STRH;
                BelegRow["STRB"] = item.STRB;
                BelegRow["PLZ"] = item.PLZ;
                BelegRow["ORT"] = item.ORT;
                BelegRow["BRNR"] = item.BRNR;
                BelegRow["STDA"] = item.STDA;
                BelegRow["EZU"] = item.EZU;
                BelegRow["ZUDA"] = item.ZZZLDAT;
                BelegRow["HERS"] = item.HERS;
                BelegRow["TYP"] = item.TYP;
                BelegRow["VVS"] = item.VVS;
                BelegRow["TYPZ"] = item.TYPZ;
                BelegRow["FGPZ"] = item.FGPZ;
                BelegRow["ABE"] = item.ABE;
                BelegRow["NAHU"] = item.NAHU;
                BelegRow["ASU"] = item.ASU;
                BelegRow["ZB1_NR"] = item.ZB1_NR;
                BelegRow["ZZREFERENZCODE"] = item.ZZREFERENZCODE;
                BelegRow["ZZGROSSKUNDNR"] = item.ZZGROSSKUNDNR;
                BelegRow["ZZEVB"] = item.ZZEVB;
                BelegRow["KONTO"] = item.KONTO;
                BelegRow["BLZ"] = item.BLZ;
                BelegRow["GEBDAT"] = item.GEBDAT;
                BelegRow["GEBORT"] = item.GEBORT;
                
            }

            itemsCollect.Add(IT_BELEG);

            try
            {
                func.Execute();
                tblTemp = ExTable.ToADOTable();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SapCon.Close();
            }


            //tblTemp.Columns.Add("FGNU", typeof(string));

            //tblTemp.AcceptChanges();

            //foreach (DataRow dt in tblTemp.Rows)
            //{

            //    foreach (Zulassung item in TempZulassungen)
            //    {

            //        if (dt["ID"].ToString() == item.ABE.PadLeft(10, '0'))
            //        {
            //            dt["FGNU"] = item.FGNU.ToString();
            //            break;
            //        }
            //    }


            //}


            return tblTemp;

        }
        ERPConnect.RFCTable BELEG2()
        {
            ERPConnect.RFCTable IT_BELEG = new ERPConnect.RFCTable("IT_BELEG");

            IT_BELEG.Name = "IT_BELEG";
            IT_BELEG.Columns.Add("ABE", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ALT_AKZ", 15, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ANR", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ARTGE", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ASU", 6, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("BANKL", 15, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("BANKN", 18, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("BLZ", 8, ERPConnect.RFCTYPE.NUM);
            IT_BELEG.Columns.Add("BRNR", 8, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("EINKZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("EZU", 8, ERPConnect.RFCTYPE.DATE);
            IT_BELEG.Columns.Add("FGNU", 30, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("FGPZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("FNAM", 45, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("FNAMB", 45, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("GEBDAT", 8, ERPConnect.RFCTYPE.DATE);
            IT_BELEG.Columns.Add("GEBORT", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("HERS", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KENNZTYP", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KONTO", 10, ERPConnect.RFCTYPE.NUM);
            IT_BELEG.Columns.Add("KREIS", 3, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KUNNR", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("MATNR", 18, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NAHU", 6, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NAME1", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NAME2", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NRGUT", 30, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ORT", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ORT01", 25, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("PLZ", 5, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("RESERVID", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("RESERVIERT", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("RNAM", 45, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("STDA", 8, ERPConnect.RFCTYPE.DATE);
            IT_BELEG.Columns.Add("STRAS", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("STRB", 7, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("STRH", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("STRN", 25, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("TYP", 3, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("TYPZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("USERID", 10, ERPConnect.RFCTYPE.NUM);
            IT_BELEG.Columns.Add("USERNAME", 50, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VKBUR", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VKORG", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VNAM", 60, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VORG", 3, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VVS", 5, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("XEZER", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZB1_NR", 30, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZFEINSTAUB_KZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKRAD_KZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKUNDNOTIZ", 100, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKUNDREF", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZUDA", 8, ERPConnect.RFCTYPE.DATE);
            IT_BELEG.Columns.Add("ZVERKAEUFER", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZEVB", 7, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZFAHRG", 20, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZGROSSKUNDNR", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZHALTER", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZKENN", 20, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZREFERENZCODE", 13, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZTEXT", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZWUNSCH", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZZLDAT", 8, ERPConnect.RFCTYPE.DATE);
            return IT_BELEG;
        }

        ERPConnect.RFCTable BELEG()
        {
            ERPConnect.RFCTable IT_BELEG = new ERPConnect.RFCTable("IT_BELEG");

            IT_BELEG.Name = "IT_BELEG";

            IT_BELEG.Columns.Add("VKORG", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("VKBUR", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ID", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KREIS", 3, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KUNNR", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("MATNR", 18, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("MENGE", 13, ERPConnect.RFCTYPE.FLOAT);
            IT_BELEG.Columns.Add("PREISDL", 11, ERPConnect.RFCTYPE.BCD);
            IT_BELEG.Columns.Add("GEBAUSL", 13, ERPConnect.RFCTYPE.BCD);
            IT_BELEG.Columns.Add("ZZKENN", 20, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZHALTER", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZZLDAT", 8, ERPConnect.RFCTYPE.DATE);
            IT_BELEG.Columns.Add("ZZSPERR", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("USERID", 10, ERPConnect.RFCTYPE.NUM);
            IT_BELEG.Columns.Add("USERNAME", 50, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZLOESCH", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("PREISKZ", 13, ERPConnect.RFCTYPE.BCD);
            IT_BELEG.Columns.Add("PREISPAUSCH", 13, ERPConnect.RFCTYPE.BCD);
            IT_BELEG.Columns.Add("RESERVIERT", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("RESERVID", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KENNZTYP", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KENNZFREMD", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KALKS", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KVGR4", 3, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ALTKN", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("EINKZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZFAHRG", 20, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZWUNSCH", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZTEXT", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NAME1", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("NAME2", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("STRAS", 35, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("PSTLZ", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ORT01", 25, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("KATR1", 2, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZZKASSENGBA", 8, ERPConnect.RFCTYPE.BCD);
            IT_BELEG.Columns.Add("ZVERKAEUFER", 4, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKUNDREF", 40, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKUNDNOTIZ", 100, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZVERBVBELN", 10, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("FAKSK", 2, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZFEINSTAUB_KZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("ZKRAD_KZ", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("XEZER", 1, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("BANKL", 15, ERPConnect.RFCTYPE.CHAR);
            IT_BELEG.Columns.Add("BANKN", 18, ERPConnect.RFCTYPE.CHAR);


            return IT_BELEG;
        }


        ERPConnect.RFCTable RfcVorerfassung()
        {

            ERPConnect.RFCTable Vorerfassung = new ERPConnect.RFCTable("VORERFASSUNG");

            Vorerfassung.Name = "VORERFASSUNG";

            Vorerfassung.Columns.Add("ID", 10, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("VORG", 2, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("AKZ", 15, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("FGNU", 30, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ANR", 1, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("VNAM", 20, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("RNAM", 20, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("FNAM", 45, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("FNAMB", 45, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("STRN", 25, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("STRH", 4, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("STRB", 7, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("PLZ", 5, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ORT", 35, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("BRNR", 8, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("STDA", 8, ERPConnect.RFCTYPE.DATE);
            Vorerfassung.Columns.Add("EZU", 8, ERPConnect.RFCTYPE.DATE);
            Vorerfassung.Columns.Add("ZUDA", 8, ERPConnect.RFCTYPE.DATE);
            Vorerfassung.Columns.Add("HERS", 4, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("TYP", 3, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("VVS", 5, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("TYPZ", 1, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("FGPZ", 1, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ABE", 1, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("NAHU", 6, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ASU", 6, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ZB1_NR", 30, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ZZREFERENZCODE", 13, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ZZGROSSKUNDNR", 10, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("ZZEVB", 7, ERPConnect.RFCTYPE.CHAR);
            Vorerfassung.Columns.Add("KONTO", 10, ERPConnect.RFCTYPE.NUM);
            Vorerfassung.Columns.Add("BLZ", 8, ERPConnect.RFCTYPE.NUM);
            Vorerfassung.Columns.Add("GEBDAT", 8, ERPConnect.RFCTYPE.DATE);
            Vorerfassung.Columns.Add("GEBORT", 40, ERPConnect.RFCTYPE.CHAR);


            return Vorerfassung;


        }


        private Int32 DBGiveNewZulassungsID()
        {

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            con.Open();

            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')";

            Int32 NewID = Convert.ToInt32(command.ExecuteScalar()) + 1;

            command.CommandText = "Update Parameters set PValue = " + NewID + " WHERE  (PName = 'HoechsteZulassungsID')";

            command.ExecuteNonQuery();
            con.Close();

            return NewID;
        }





    }
}
