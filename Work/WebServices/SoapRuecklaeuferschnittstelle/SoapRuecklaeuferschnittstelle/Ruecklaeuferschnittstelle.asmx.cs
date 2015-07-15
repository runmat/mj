using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using SapORM.Services;

namespace SoapRuecklaeuferschnittstelle
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für Post
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Post : WebService
    {
        private static ISapDataService SapDataService
        {
            get
            {
                if (ConfigurationManager.AppSettings["SapProdSytem"].NotNullOrEmpty().ToLower() == "true")
                    return new SapDataServiceLiveSystemNoCacheFactory().Create();

                return new SapDataServiceTestSystemNoCacheFactory().Create();
            }
        }

        [WebMethod]
        // ReSharper disable InconsistentNaming Sonst generiert WDSL Klasse ruecklaeuferschnittstelle auf dem Client
        public Ruecklaeuferschnittstelle Put(string anmeldename, string passwort, Ruecklaeuferschnittstelle Ruecklaeuferschnittstelle)
        // ReSharper restore InconsistentNaming
        {
            try
            {
                // Authentifizierung
                if (!RuecklaeuferschnittstelleUtils.Authenticate(anmeldename, passwort))
                {
                    Ruecklaeuferschnittstelle.fehlercode = RuecklaeuferschnittstelleUtils.ErrorAuthentification;
                    return Ruecklaeuferschnittstelle;
                }

                // Fehlen die Stammdaten?
                if (Ruecklaeuferschnittstelle.Stammdaten == null)
                {
                    Ruecklaeuferschnittstelle.fehlercode = RuecklaeuferschnittstelleUtils.StammdatenNichtAngegeben;
                    return Ruecklaeuferschnittstelle;
                }

                var sap = SapDataService;

                Z_DPM_IMP_DAT_RUECKL_01.Init(sap);

                sap.SetImportParameter("I_KUNNR_AG", "10048516"); // Wert fixiert

                var gtdatList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetImportList(sap);
                var transpList = Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP.GetImportList(sap);
                var repListe = Z_DPM_IMP_DAT_RUECKL_01.GT_REP.GetImportList(sap);

                #region Aufbereitung des SAP Aufrufs

                #region Stammdaten

                var gtDat = new Z_DPM_IMP_DAT_RUECKL_01.GT_DAT
                    {
                        VORGANGS_ID = Ruecklaeuferschnittstelle.vorgangsid.ToString(),
                        STD_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat),
                        RUECKGAB_OPTION = Ruecklaeuferschnittstelle.Stammdaten.Rueckgabeoption,
                        KUNNR_HLA = Ruecklaeuferschnittstelle.Stammdaten.Kundennummer,
                        KUNNAME_HLA = Ruecklaeuferschnittstelle.Stammdaten.Kundenname,
                        NUTZER = Ruecklaeuferschnittstelle.Stammdaten.Nutzer,
                        VERTRAGSNR_HLA = Ruecklaeuferschnittstelle.Stammdaten.Vertragsnummer,
                        LICENSE_NUM = Ruecklaeuferschnittstelle.Stammdaten.Kennzeichen,
                        CHASSIS_NUM = Ruecklaeuferschnittstelle.Stammdaten.Fahrgestellnummer,
                        ERST_ZUL_DAT = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(Ruecklaeuferschnittstelle.Stammdaten.Erstzulassung),
                        HERST = Ruecklaeuferschnittstelle.Stammdaten.Hersteller,
                        SERIE = Ruecklaeuferschnittstelle.Stammdaten.Serie,
                        KRAFTSTOFF = Ruecklaeuferschnittstelle.Stammdaten.Treibstoffart,
                        AUFBAU = Ruecklaeuferschnittstelle.Stammdaten.Aufbauart,
                        LEISTUNG = Ruecklaeuferschnittstelle.Stammdaten.Leistung,
                        WINTERREIFEN = Ruecklaeuferschnittstelle.Stammdaten.Winterreifen,
                    };

                #endregion

                #region Abholauftrag

                if (Ruecklaeuferschnittstelle.Abholung != null && Ruecklaeuferschnittstelle.Abholung.Abholauftrag != null)
                {
                    gtDat.TRANSPORTART = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Transportart;

                    // wenn Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Datum_von 
                    if (Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Datum_von.Date < DateTime.Now.Date)
                    {
                        gtDat.WLIEFDAT_VON = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(DateTime.Now.Date.AddDays(2));
                        gtDat.WLIEFDAT_BIS = gtDat.WLIEFDAT_VON;
                    }
                    else
                    {
                        gtDat.WLIEFDAT_VON = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Datum_von);
                        gtDat.WLIEFDAT_BIS = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Datum_bis);
                    }


                    gtDat.BEM = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Bemerkung;
                    gtDat.ABH_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat);
                    gtDat.EIG_ANL = RuecklaeuferschnittstelleUtils.BoolToTrueFalse(Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Eigenanlieferung);

                    if (Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Gutachten_erstellen.HasValue)
                    {
                        gtDat.GUTA_ERSTELLEN = RuecklaeuferschnittstelleUtils.BoolToTrueFalse(Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Gutachten_erstellen.Value);
                    }
                    else
                    {
                        // Default Wert ist true
                        gtDat.GUTA_ERSTELLEN = RuecklaeuferschnittstelleUtils.BoolToTrueFalse(true);
                    }

                    if (Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen != null && Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort != null)
                    {
                        gtDat.NAME_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Firma;
                        gtDat.NAME_2_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Ansprechpartner;
                        gtDat.STREET_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Strasse;
                        gtDat.POSTL_CODE_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Postleitzahl;
                        gtDat.CITY_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Ort;
                        gtDat.NAME_2_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Ansprechpartner;
                        gtDat.TELEPHONE_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Telefon;
                        gtDat.SMTP_ADDR_ABH = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Abholort.Email;
                    }

                    if (Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen != null && Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort != null)
                    {
                        gtDat.NAME_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Firma;
                        gtDat.NAME_2_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Ansprechpartner;
                        gtDat.STREET_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Strasse;
                        gtDat.POSTL_CODE_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Postleitzahl;
                        gtDat.CITY_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Ort;
                        gtDat.NAME_2_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Ansprechpartner;
                        gtDat.TELEPHONE_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Telefon;
                        gtDat.SMTP_ADDR_ZI = Ruecklaeuferschnittstelle.Abholung.Abholauftrag.Adressen.Zielort.Email;
                    }

                    if (Ruecklaeuferschnittstelle.Abholung.Abholtermin_bestaetigt != null)
                    {
                        gtDat.ABHT_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.Abholung.Abholtermin_bestaetigt.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat);
                        gtDat.BEST_ABH_TERMIN = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(Ruecklaeuferschnittstelle.Abholung.Abholtermin_bestaetigt.Bestaetigter_Abholtermin);
                    }
                }

                #endregion

                #region Aufbereitung und Reparaturen

                if (Ruecklaeuferschnittstelle.Aufbereitung != null)
                {
                    if (Ruecklaeuferschnittstelle.Aufbereitung.Aufbereitungsauftrag != null)
                    {
                        gtDat.AUFB_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.Aufbereitung.Aufbereitungsauftrag.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat);
                        gtDat.EING_AUFBER_AUFTR = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(Ruecklaeuferschnittstelle.Aufbereitung.Aufbereitungsauftrag.Datum);
                        gtDat.BEM_AUFB = Ruecklaeuferschnittstelle.Aufbereitung.Aufbereitungsauftrag.Bemerkung;

                        foreach (var reparatur in Ruecklaeuferschnittstelle.Aufbereitung.Aufbereitungsauftrag.Reparaturen)
                        {
                            var sapRep = new Z_DPM_IMP_DAT_RUECKL_01.GT_REP
                                {
                                    BEZEICHNUNG = reparatur.Bezeichnung,
                                    CODE = reparatur.Code,
                                    MASSNAHME = reparatur.Massnahme,
                                    REP_KOSTEN = reparatur.Reparaturkosten.ToString(),
                                    TYP = reparatur.Typ,
                                    VORGANGS_ID = Ruecklaeuferschnittstelle.vorgangsid.ToString()
                                };
                            repListe.Add(sapRep);
                        }
                    }
                }

                #endregion

                #region Verwertung

                if (Ruecklaeuferschnittstelle.Verwertung != null)
                {
                    if (Ruecklaeuferschnittstelle.Verwertung.Verwertungsentscheidung != null)
                    {
                        gtDat.VERW_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.Verwertung.Verwertungsentscheidung.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat);
                        gtDat.ENTSCHEIDUNG = Ruecklaeuferschnittstelle.Verwertung.Verwertungsentscheidung.Entscheidung;
                        gtDat.VERKAUFSKANAL = Ruecklaeuferschnittstelle.Verwertung.Verwertungsentscheidung.Verkaufskanal;
                    }
                }

                #endregion

                #region Transport

                if (Ruecklaeuferschnittstelle.Transport != null)
                {
                    foreach (var transport in Ruecklaeuferschnittstelle.Transport)
                    {
                        var sapTrans = new Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP
                        {
                            ZULETZT_AKTUALISIERT = transport.Transportauftrag.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat),
                            TRANSPORTART = transport.Transportauftrag.Transportart,
                            WLIEFDAT_VON = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(transport.Transportauftrag.Datum_von),
                            WLIEFDAT_BIS = RuecklaeuferschnittstelleUtils.DatumDateTimeToSap(transport.Transportauftrag.Datum_bis),
                            BEM = transport.Transportauftrag.Bemerkung,
                            NUMMER = transport.Transportauftrag.Nummer.ToString(),
                            NAME_ABH = transport.Transportauftrag.Adressen.Abholort.Firma,
                            STREET_ABH = transport.Transportauftrag.Adressen.Abholort.Strasse,
                            POSTL_CODE_ABH = transport.Transportauftrag.Adressen.Abholort.Postleitzahl,
                            CITY_ABH = transport.Transportauftrag.Adressen.Abholort.Ort,
                            NAME_2_ABH = transport.Transportauftrag.Adressen.Abholort.Ansprechpartner,
                            TELEPHONE_ABH = transport.Transportauftrag.Adressen.Abholort.Telefon,
                            SMTP_ADDR_ABH = transport.Transportauftrag.Adressen.Abholort.Email,
                            NAME_ZI = transport.Transportauftrag.Adressen.Zielort.Firma,
                            STREET_ZI = transport.Transportauftrag.Adressen.Zielort.Strasse,
                            POSTL_CODE_ZI = transport.Transportauftrag.Adressen.Zielort.Postleitzahl,
                            CITY_ZI = transport.Transportauftrag.Adressen.Zielort.Ort,
                            NAME_2_ZI = transport.Transportauftrag.Adressen.Zielort.Ansprechpartner,
                            TELEPHONE_ZI = transport.Transportauftrag.Adressen.Zielort.Telefon,
                            SMTP_ADDR_ZI = transport.Transportauftrag.Adressen.Zielort.Email,
                            VORGANGS_ID = Ruecklaeuferschnittstelle.vorgangsid.ToString()
                        };
                        transpList.Add(sapTrans);
                    }
                }

                #endregion

                #region ProzessEnde

                if (Ruecklaeuferschnittstelle.Prozessende != null)
                {
                    gtDat.VORG_ABGESCHL = Ruecklaeuferschnittstelle.Prozessende.vorgang_abgeschlossen;
                    gtDat.VABG_ZULETZT_AKTUALISIERT = Ruecklaeuferschnittstelle.Prozessende.zuletzt_aktualisiert.ToString(RuecklaeuferschnittstelleUtils.TimestampDateFormat);
                }

                #endregion

                #endregion

                #region SAP Aufuruf

                LogService logService = new LogService(String.Empty, String.Empty);

                var logInput = String.Format("GT_DAT: {0}, GT_REP: {1}, GT_TRANSP: {2}", gtDat.GetObjectAsString(), repListe.GetListAsString(), transpList.GetListAsString());
                logService.LogWebServiceTraffic("SAP-Input", logInput, ConfigurationManager.AppSettings["LogTableName"]);

                gtdatList.Add(gtDat);

                sap.ApplyImport(gtdatList);
                sap.ApplyImport(transpList);
                sap.ApplyImport(repListe);

                sap.Execute();

                var gtdatExportList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetExportList(sap);
                var gtReps = Z_DPM_IMP_DAT_RUECKL_01.GT_REP.GetExportList(sap);
                var gtTransps = Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP.GetExportList(sap);

                var logOutput = String.Format("GT_DAT: {0}, GT_REP: {1}, GT_TRANSP: {2}", gtdatExportList.GetListAsString(), gtReps.GetListAsString(), gtTransps.GetListAsString());
                logService.LogWebServiceTraffic("SAP-Input", logOutput, ConfigurationManager.AppSettings["LogTableName"]);

                var toReturn = RuecklaeuferschnittstelleUtils.RuecklaeuferschnittstelleFromSap(gtdatExportList, gtReps, gtTransps);
                return toReturn;
            }
            catch (Exception ex)
            {
                var logService = new LogService(String.Empty, String.Empty);
                logService.LogElmahError(ex, null);
                throw;
            }

            #endregion
        }

        [WebMethod]
        public RuecklaeuferschnittstelleList List(string anmeldename, string passwort, long[] ids)
        {
            try
            {
                if (!RuecklaeuferschnittstelleUtils.Authenticate(anmeldename, passwort))
                {
                    return new RuecklaeuferschnittstelleList
                    {
                        fehlercode = RuecklaeuferschnittstelleUtils.ErrorAuthentification,
                        Ruecklaeuferschnittstellen = new Ruecklaeuferschnittstelle[0]
                    };
                }

                if (ids == null || ids.Length == 0)
                {
                    return new RuecklaeuferschnittstelleList
                    {
                        fehlercode = 0.ToString(),
                        Ruecklaeuferschnittstellen = new Ruecklaeuferschnittstelle[0]
                    };
                }

                IEnumerable<long> enumerable = ids.ToArray();

                LogService logService = new LogService(String.Empty, String.Empty);

                var list = new List<Ruecklaeuferschnittstelle>();

                foreach (var id in enumerable)
                {
                    logService.LogWebServiceTraffic("SAP-Input", "VORGANGS_ID: " + id, ConfigurationManager.AppSettings["LogTableName"]);

                    var sap = SapDataService;

                    Z_DPM_IMP_DAT_RUECKL_01.Init(sap);

                    sap.SetImportParameter("I_KUNNR_AG", "10048516"); // Wert fixiert
                    var gtdatList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetImportList(sap);
                    var gtDat = new Z_DPM_IMP_DAT_RUECKL_01.GT_DAT
                    {
                        VORGANGS_ID = id.ToString()
                    };

                    gtdatList.Add(gtDat);
                    sap.ApplyImport(gtdatList);
                    sap.Execute();

                    var gtDatExportList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetExportList(sap);
                    var gtRepExportList = Z_DPM_IMP_DAT_RUECKL_01.GT_REP.GetExportList(sap);
                    var gtTranspExportList = Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP.GetExportList(sap);

                    var logOutput = String.Format("GT_DAT: {0}, GT_REP: {1}, GT_TRANSP: {2}", gtDatExportList.GetListAsString(), gtRepExportList.GetListAsString(), gtTranspExportList.GetListAsString());
                    logService.LogWebServiceTraffic("SAP-Input", logOutput, ConfigurationManager.AppSettings["LogTableName"]);

                    // Wenn auf SAP Seite kein Satz ermittelt wurde, einen "leeren" Satz mit Fehlercode zurückgeben
                    if (gtDatExportList.Any() == false)
                    {
                        list.Add(new Ruecklaeuferschnittstelle
                        {
                            vorgangsid = id,
                            fehlercode = RuecklaeuferschnittstelleUtils.StammdatenNichtAngegeben
                        });
                        continue;
                    }

                    // Wenn auf SAP Seite ein Satz ermittelt wurde ohne dass eine zuletzt_aktualisiert einen Wert hat, einen "leeren" Satz mit Fehlercode zurückgeben
                    if (string.IsNullOrEmpty(gtDatExportList.First().STD_ZULETZT_AKTUALISIERT))
                    {
                        list.Add(new Ruecklaeuferschnittstelle
                        {
                            vorgangsid = id,
                            fehlercode = RuecklaeuferschnittstelleUtils.StammdatenNichtAngegeben
                        });
                        continue;
                    }

                    // Satz kann übernommen werden
                    try
                    {
                        list.Add(RuecklaeuferschnittstelleUtils.RuecklaeuferschnittstelleFromSap(gtDatExportList,
                                                                                                 gtRepExportList,
                                                                                                 gtTranspExportList));
                    }
                    catch (Exception)
                    {
                        // Im Falle eines Fehlers bei dem einlesen der Werte einen leeren Satz zurückgeben
                        list.Add(new Ruecklaeuferschnittstelle
                            {
                                vorgangsid = id,
                                fehlercode = RuecklaeuferschnittstelleUtils.StammdatenNichtAngegeben
                            });                        
                    }
                }

                return new RuecklaeuferschnittstelleList
                {
                    fehlercode = string.Empty,
                    Ruecklaeuferschnittstellen = list.ToArray()
                };
            }
            catch (Exception ex)
            {
                var logService = new LogService(String.Empty, String.Empty);
                logService.LogElmahError(ex, null);
                throw;
            }
        }
    }
}
