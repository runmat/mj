using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AppZulassungsdienst.lib.Models;
using System.Data;
using CKG.Base.Business;
using System.Configuration;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib 
{
    public enum GridFilterMode
    {
        Default,
        ShowOnlyOk,
        ShowOnlyError,
        ShowOnlyOandL,
        ShowOnlyAandL
    }

    public enum GridCheckMode
    {
        CheckAll,
        CheckOnlyRelevant,
        CheckNone
    }

    public enum Zahlart
    {
        EC,
        Bar,
        Rechnung
    }

	public class ZLDCommon : SapOrmBusinessBase
    {
        #region Properties

        public static string CONST_IDSONSTIGEDL = "570";

        public List<Kundenstammdaten> KundenStamm { get; private set; }
        public List<Materialstammdaten> MaterialStamm { get; private set; }
        public List<Stva> StvaStamm { get; private set; }
        public List<SonderStva> SonderStvaStamm { get; private set; }
        public DataTable tblKennzGroesse { get; private set; }
        public Filialadresse AdresseFiliale { get; private set; }
        public List<GruppeTour> Touren { get; private set; }
        public List<GruppeTour> Kundengruppen { get; private set; }
        public List<Kundenadresse> KundenZurGruppe { get; private set; }

        public String Kundename { get; set; }
        public String Bezeichnung { get; set; }
        public String GroupOrTourID { get; set; }
        public String SWIFT { get; set; }
        public String IBAN { get; set; }
        public String Bankname { get; set; }
        public String Bankschluessel { get; set; }
        public String BLZ { get; set; }
        public String Kontonr { get; set; }

        #endregion

        #region Methods

        public ZLDCommon(string userReferenz)
        {
            VKORG = GetVkOrgFromUserReference(userReferenz);
            VKBUR = GetVkBurFromUserReference(userReferenz);
            
            tblKennzGroesse = new DataTable();
		}

        public static string GetDocRootPath(bool isTestuser)
        {
            var pathTemplate = ConfigurationManager.AppSettings["ZldDocumentsRootPath"];
            if (String.IsNullOrEmpty(pathTemplate))
                return "";

            return String.Format(pathTemplate, (isTestuser ? "test" : "prod"));
        }

        public void getSAPDatenStamm()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_KUNDE_MAT.Init(SAP, "I_VKORG, I_VKBUR", VKORG, VKBUR);

                    CallBapi();

                    KundenStamm = AppModelMappings.Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_KUNDE_To_Kundenstammdaten.Copy(Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE.GetExportList(SAP)).OrderBy(k => k.Name).ToList();

                    KundenStamm.Insert(0, new Kundenstammdaten
                    {
                        KundenNr = "0",
                        Name1 = " - keine Auswahl - "
                    });

                    MaterialStamm = AppModelMappings.Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_MATERIAL_To_Materialstammdaten.Copy(Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL.GetExportList(SAP)).OrderBy(m => m.Name).ToList();

                    if (MaterialStamm.None())
                        RaiseError(5555, "Keine Materialdaten gefunden!");

                    MaterialStamm.Insert(0, new Materialstammdaten
                    {
                        MaterialNr = "0",
                        MaterialName = " - keine Auswahl - "
                    });
                });
        }

        public void getSAPZulStellen()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_ZULSTEL.Init(SAP);

                    CallBapi();

                    StvaStamm = AppModelMappings.Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Stva.Copy(Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL.GetExportList(SAP)).OrderBy(s => s.Bezeichnung).ToList();

                    SonderStvaStamm = AppModelMappings.Z_ZLD_EXPORT_ZULSTEL_GT_SONDER_To_SonderStva.Copy(Z_ZLD_EXPORT_ZULSTEL.GT_SONDER.GetExportList(SAP)).OrderBy(s => s.Landkreis).ToList();

                    if (StvaStamm.None())
                        RaiseError(5555, "Keine STVA-Daten gefunden!");

                    StvaStamm.Insert(0, new Stva
                    {
                        Landkreis = "",
                        KreisBezeichnung = " - keine Auswahl - "
                    });
                });
        }

        public void LadeKennzeichenGroesse()
        {
            ClearError();

            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                tblKennzGroesse = new DataTable();

                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();

                command.CommandText = "SELECT dbo.KennzeichGroesse.ID, dbo.MaterialKennzGroesse.Matnr, dbo.MaterialKennzGroesse.Kennzart, dbo.KennzeichGroesse.Groesse" +
                        " FROM dbo.MaterialKennzGroesse INNER JOIN dbo.KennzeichGroesse ON dbo.MaterialKennzGroesse.Matnr = dbo.KennzeichGroesse.Matnr";

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                adapter.Fill(tblKennzGroesse);

                if (tblKennzGroesse.Rows.Count == 0)
                    RaiseError(9999, "Keine Daten gefunden!");
            }
            catch (Exception ex)
            {
                RaiseError(9999, "Fehler beim Laden der Eingabeliste: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void getFilialadresse()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_FILIAL_ADRESSE.Init(SAP, "I_VKORG, I_VKBUR", VKORG, VKBUR);

                    CallBapi();

                    AdresseFiliale = AppModelMappings.Z_ZLD_EXPORT_FILIAL_ADRESSE_ES_FIL_ADRS_To_Filialadresse.Copy(Z_ZLD_EXPORT_FILIAL_ADRESSE.ES_FIL_ADRS.GetExportList(SAP)).ToList().FirstOrDefault();
                });
        }

        public void GetGruppen_Touren(string GroupArt)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_GET_GRUPPE.Init(SAP, "I_VKBUR, I_GRUPART", VKBUR, GroupArt);

                    CallBapi();

                    var gruppenTouren = AppModelMappings.Z_ZLD_GET_GRUPPE_GT_GRUPPE_To_GruppeTour.Copy(Z_ZLD_GET_GRUPPE.GT_GRUPPE.GetExportList(SAP)).ToList();

                    var newGroupName = (gruppenTouren.Any() ? " - keine Auswahl - " : (GroupArt == "T" ? " - keine Touren gepflegt - " : " - keine Gruppen gepflegt - "));

                    gruppenTouren.Add(new GruppeTour
                    {
                        Gruppe = "0",
                        VkBur = VKBUR,
                        GruppenArt = GroupArt,
                        GruppenName = newGroupName
                    });

                    if (GroupArt == "T")
                        Touren = new List<GruppeTour>(gruppenTouren);
                    else
                        Kundengruppen = new List<GruppeTour>(gruppenTouren);
                });
        }

        public void GetKunden_TourenZuordnung()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_GET_GRUPPE_KDZU.Init(SAP, "I_GRUPPE", GroupOrTourID);

                    CallBapi();

                    KundenZurGruppe = AppModelMappings.Z_ZLD_GET_GRUPPE_KDZU_GT_KDZU_To_Kundenadresse.Copy(Z_ZLD_GET_GRUPPE_KDZU.GT_KDZU.GetExportList(SAP)).ToList();
                });
        }

        public void SetKunden_Touren(string GroupArt, string Action)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_SET_GRUPPE.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_GRUPPE", GroupOrTourID);
                    SAP.SetImportParameter("I_GRUPART", GroupArt);
                    SAP.SetImportParameter("I_BEZEI", Bezeichnung);
                    SAP.SetImportParameter("I_FUNC", Action);

                    CallBapi();
                });
        }

        public void SetKunden_TourenZuordnung(string Kunnr, string Action)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_SET_GRUPPE_KDZU.Init(SAP);

                    SAP.SetImportParameter("I_GRUPPE", GroupOrTourID);
                    SAP.SetImportParameter("I_KUNNR", Kunnr);
                    SAP.SetImportParameter("I_FUNC", Action);

                    CallBapi();
                });
        }

        public void ProofIBAN()
        {
            Bankname = "";
            Bankschluessel = "";
            SWIFT = "";
            Kontonr = "";

            ExecuteSapZugriff(() =>
                {
                    Z_FI_CONV_IBAN_2_BANK_ACCOUNT.Init(SAP, "I_IBAN", IBAN);

                    CallBapi();

                    Bankname = SAP.GetExportParameter("E_BANKA");
                    Bankschluessel = SAP.GetExportParameter("E_BANK_NUMBER");
                    SWIFT = SAP.GetExportParameter("E_SWIFT");
                    Kontonr = SAP.GetExportParameter("E_BANK_ACCOUNT");
                });
        }

        public static string toShortDateStr(string dat)
        {
            DateTime datum;

            try
            {
                datum = Convert.ToDateTime(dat.Substring(0, 2) + "." + dat.Substring(2, 2) + "." + DateTime.Now.Year.ToString().Substring(0, 2) + dat.Substring(4, 2));
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return datum.ToShortDateString();
        }

        public static string GetVkOrgFromUserReference(string userReferenz)
        {
            return userReferenz.NotNullOrEmpty().Substring(0, Math.Min(4, userReferenz.Length));
        }

        public static string GetVkBurFromUserReference(string userReferenz)
        {
            return (userReferenz.NotNullOrEmpty().Length > 4 ? userReferenz.Substring(4, Math.Min(4, (userReferenz.Length - 4))) : "");
        }

        public string SonderStvaStammToJsArray()
        {
            StringBuilder javaScript = new StringBuilder();

            for (int i = 0; i < SonderStvaStamm.Count; i++)
            {
                if (i == 0)
                    javaScript.Append("ArraySonderStva = \n[\n");

                var item = SonderStvaStamm[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + item.Landkreis + "'");
                javaScript.Append(",");
                javaScript.Append("'" + item.KfzKreiskennzeichen + "'");
                javaScript.Append(" ]");

                if ((i + 1) == SonderStvaStamm.Count)
                    javaScript.Append("\n];\n");
                else
                    javaScript.Append(",\n");
            }

            return javaScript.ToString();
        }

        public string MaterialStammToJsArray()
        {
            StringBuilder javaScript = new StringBuilder();

            for (int i = 0; i < MaterialStamm.Count; i++)
            {
                if (i == 0)
                    javaScript.Append("ArrayMengeERL = \n[\n");

                var item = MaterialStamm[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + item.MaterialNr + "'");
                javaScript.Append(",");
                javaScript.Append("'" + item.MengeErlaubt.BoolToX() + "'");
                javaScript.Append(" ]");

                if ((i + 1) == MaterialStamm.Count)
                    javaScript.Append("\n];\n");
                else
                    javaScript.Append(",\n");
            }

            return javaScript.ToString();
        }

        public string KundenStammToJsArray()
        {
            StringBuilder javaScript = new StringBuilder();

            for (int i = 0; i < KundenStamm.Count; i++)
            {
                if (i == 0)
                    javaScript.Append("ArrayBarkunde = \n[\n");

                var item = KundenStamm[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + item.KundenNr + "'");
                javaScript.Append(",");
                javaScript.Append("'" + item.Bar.BoolToX() + "'");
                javaScript.Append(",");
                javaScript.Append("'" + item.Pauschal.BoolToX() + "'");
                javaScript.Append(",");
                javaScript.Append("'" + item.Cpd.BoolToX() + "'");
                javaScript.Append(" ]");

                if ((i + 1) == KundenStamm.Count)
                    javaScript.Append("\n];\n");
                else
                    javaScript.Append(",\n");
            }

            return javaScript.ToString();
        }

        public static bool FilterData<T>(T item, string filterProperty, string filterValue, bool useStringLike)
        {
            var blnResult = false;
            var prop = typeof(T).GetProperty(filterProperty);

            if (prop.PropertyType == typeof(string))
            {
                var itemValue = (string)prop.GetValue(item, null);

                if (useStringLike)
                {
                    blnResult = (itemValue.NotNullOrEmpty().ToLower().Contains(filterValue.NotNullOrEmpty().ToLower()));
                }
                else
                {
                    blnResult = (String.Compare(itemValue, filterValue, true) == 0);
                }
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                var itemValue = (DateTime)prop.GetValue(item, null);

                DateTime tmpDate;

                if (DateTime.TryParseExact(filterValue, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDate))
                {
                    blnResult = (itemValue.Date == tmpDate.Date);
                }
            }
            else if (prop.PropertyType == typeof(DateTime?))
            {
                var itemValue = (DateTime?)prop.GetValue(item, null);

                DateTime tmpDate;

                if (itemValue.HasValue && DateTime.TryParseExact(filterValue, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDate))
                {
                    blnResult = (itemValue.Value.Date == tmpDate.Date);
                }
            }
            else if (prop.PropertyType == typeof(Decimal))
            {
                var itemValue = (decimal)prop.GetValue(item, null);

                decimal tmpDec;

                if (Decimal.TryParse(filterValue.NotNullOrEmpty().Replace(',', '.'), out tmpDec))
                {
                    blnResult = (itemValue == tmpDec);
                }
            }
            else if (prop.PropertyType == typeof(Decimal?))
            {
                var itemValue = (decimal?)prop.GetValue(item, null);

                decimal tmpDec;

                if (itemValue.HasValue && Decimal.TryParse(filterValue.NotNullOrEmpty().Replace(',', '.'), out tmpDec))
                {
                    blnResult = (itemValue == tmpDec);
                }
            }

            return blnResult;
        }

        public static void KennzeichenAufteilen(string kennzeichen, out string kennzTeil1, out string kennzTeil2)
        {
            kennzTeil1 = "";
            kennzTeil2 = "";

            var k = kennzeichen.NotNullOrEmpty();

            if (k.Contains('-'))
            {
                string[] tmpKennz = k.Split('-');

                if (tmpKennz.Length == 1)
                {
                    kennzTeil1 = tmpKennz[0];
                }
                else if (tmpKennz.Length == 2)
                {
                    kennzTeil1 = tmpKennz[0];
                    kennzTeil2 = tmpKennz[1];
                }
                else if (tmpKennz.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                {
                    kennzTeil1 = tmpKennz[0];
                    kennzTeil2 = tmpKennz[1] + "-" + tmpKennz[2];
                }
            }
            else
            {
                kennzTeil1 = k.Substring(0, Math.Min(3, k.Length));
                if (k.Length > 3)
                {
                    kennzTeil2 = k.Substring(3);
                }
            }
        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Kennzeichenpreis ausblenden 
        /// wenn es sich um einen Pauschalkunden handelt oder kein Kennzeichenmaterial zum
        /// Material hinterlegt ist.
        /// </summary>
        /// <param name="kundenNr"></param>
        /// <param name="Matnr">Materialnr.</param>
        /// <returns>Visibility von txtPreisKZ im Gridview</returns>
        public bool proofPauschMat(String kundenNr, String Matnr)
        {
            var kunde = KundenStamm.FirstOrDefault(k => k.KundenNr == kundenNr);
            var mat = MaterialStamm.FirstOrDefault(m => m.MaterialNr == Matnr);

            return ((kunde == null || !kunde.Pauschal) && mat != null && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr));
        }

        /// <summary>
        /// Gebührenpflichtig?
        /// </summary>
        /// <param name="Matnr"></param>
        /// <returns></returns>
        public bool proofGebMatPflicht(String Matnr)
        {
            var mat = MaterialStamm.FirstOrDefault(m => m.MaterialNr == Matnr);

            return (mat != null && mat.Gebuehrenpflichtig);
        }

        public string GetMaterialNameFromDienstleistungRow(DataRow dRow)
        {
            String[] sMaterial = dRow["Text"].ToString().Split('~');

            if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
            {
                return dRow["DLBezeichnung"].ToString().Trim();
            }

            if (sMaterial.Length == 2)
            {
                return sMaterial[0].Trim();
            }

            return "";
        }

        #endregion
    }
}