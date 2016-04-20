using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.WFM.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_WFM_READ_KONVERTER_01.GT_DATEN, WfmAuftragFeldname> Z_WFM_READ_KONVERTER_01_GT_DATEN_To_WfmAuftragFeldname
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_KONVERTER_01.GT_DATEN, WfmAuftragFeldname>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Feldname = s.FELDNAME_AUFTRAG;
                        d.Anzeigename = s.FELDANZEIGE;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_DATEN, WfmAuftrag> Z_WFM_READ_AUFTRAEGE_01_GT_DATEN_To_WfmAuftrag
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_DATEN, WfmAuftrag>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AbmeldeArtCode = s.ABMELDEART;
                        d.AbmeldeDatum = s.ABMELDEDATUM;
                        d.AbmeldeStatusCode = s.ABMELDESTATUS;
                        d.Anmerkung = s.ANMERKUNG;
                        d.Aufgabe = s.AUFGABE;
                        d.AuftragsNr = s.BELNR_ZABMK;
                        d.Carport = s.CARPORT;
                        d.Ende = s.ENDE;
                        d.EquiNrZb1 = s.EQUNR_ZB1;
                        d.EquiNrZb2 = s.EQUNR_ZB2;
                        d.Erfasst = s.ERFASST;
                        d.Erfassungsdatum = s.ERDAT_ZCARPK;
                        d.FahrgestellNr = s.FAHRG;
                        d.FarbFlag = s.Z_FARBE;
                        d.FolgetaskId = s.FOLGE_TASK_ID;
                        d.Istdatum = s.IST_DATUM;
                        d.Istzeit = s.IST_ZEIT.ToTimeString();
                        d.Kennzeichen = s.KENNZ;
                        d.KennzeichenHintenEntwertetDatum = s.KENNZH_ENTWERTET;
                        d.KennzeichenHintenGestohlenDatum = s.KENNZH_DIEBSTAHL;
                        d.KennzeichenHintenVorhandenDatum = s.KENNZ_HINTEN_VORH;
                        d.KennzeichenVornEntwertetDatum = s.KENNZV_ENTWERTET;
                        d.KennzeichenVornGestohlenDatum = s.KENNZV_DIEBSTAHL;
                        d.KennzeichenVornVorhandenDatum = s.KENNZ_VORN_VORH;
                        d.KundenAuftragsNr = s.KUNDENAUFTRAGSNR;
                        d.LaufendeNr = s.LFD_NR;
                        d.NeuanforderungEmpfaenger = s.EMPF_NEUANFORD;
                        d.NeuanforderungUser = s.NAUANF_USER;
                        d.NeuanforderungZlsDatum = s.NAUANF_ZLS;
                        d.NeueInfoDad = s.NEW_INFO_DAD.XToBool();
                        d.Referenz1 = s.REFERENZ1;
                        d.Referenz2 = s.REFERENZ2;
                        d.Referenz3 = s.REFERENZ3;
                        d.Selektion1 = s.SELEKTION1.XToBool();
                        d.Selektion2 = s.SELEKTION2.XToBool();
                        d.Selektion3 = s.SELEKTION3.XToBool();
                        d.Solldatum = s.SOLL_DATUM;
                        d.Sollzeit = s.SOLL_ZEIT.ToTimeString();
                        d.Startdatum = s.STARTDATUM;
                        d.Startzeit = s.STARTZEIT.ToTimeString();
                        d.Status = s.STATUS;
                        d.StornoDatum = s.STORNODATUM;
                        d.TaskId = s.TASK_ID;
                        d.ToDoWer = s.TODO_WER;
                        d.User = s.ZUSER;
                        d.Verkaufsbeleg = s.BELNR_ZCARPK;
                        d.VorgangsNrAbmeldeauftrag = s.VORG_NR_ABM_AUF;
                        d.VorgangsNrVorgaengerAuftrag = s.VORGAENGER_AUFTR;
                        d.WiedervorlageKundeDatum = s.WIEDVORLAGE_KUNDE;
                        d.WiedervorlageScDatum = s.WIEDVORLAGE_SC;
                        d.Zb1Nr = s.ZB1_NR;
                        d.Zb1VorhandenDatum = s.ZB1_VORHANDEN;
                        d.Zb2Nr = s.ZB2_NR;
                        d.Zb2VorhandenDatum = s.ZB2_VORHANDEN;
                        d.ZustimmungEmpfaenger = s.ZUSTIMM_EMPF;
                        d.ZustimmungUser = s.ZUSTIMM_USER;
                        d.ZustimmungZlsDatum = s.ZUSTIMMUNG_ZLS;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_INFO_01.GT_DATEN, WfmInfo> Z_WFM_READ_INFO_01_GT_DATEN_To_WfmInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_INFO_01.GT_DATEN, WfmInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VorgangsNrAbmeldeauftrag = s.VORG_NR_ABM_AUF;
                        d.Datum = s.DATUM;
                        d.LaufendeNr = s.LFD_NR;
                        d.NeueInfoDad = s.NEW_INFO_DAD.XToBool();
                        d.Text = s.TEXT;
                        d.ToDoWer = s.TODO_WER;
                        d.User = s.ZUSER;
                        d.Zeit = s.ZEIT.ToTimeString();
                    }));
            }
        }

        static public ModelMapping<Z_WFM_LIST_DOKU_01.GT_DOKUMENTE, WfmDokumentInfo> Z_WFM_LIST_DOKU_01_GT_DOKUMENTE_To_WfmDokumentInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_LIST_DOKU_01.GT_DOKUMENTE, WfmDokumentInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VorgangsNrAbmeldeauftrag = s.VORG_NR_ABM_AUF;
                        d.ArchivId = s.ARCHIV_ID;
                        d.Dateiname = s.DATEINAME;
                        d.Dokumentart = s.AR_OBJECT;
                        d.ObjectId = s.OBJECT_ID;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_TODO_01.GT_DATEN, WfmToDo> Z_WFM_READ_TODO_01_GT_DATEN_To_WfmToDo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_TODO_01.GT_DATEN, WfmToDo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VorgangsNrAbmeldeauftrag = s.VORG_NR_ABM_AUF;
                        d.Anmerkung = s.ANMERKUNG;
                        d.Aufgabe = s.AUFGABE;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.FolgetaskAufgabe = s.AUFGABE_FOLGE_TASK;
                        d.FolgetaskId = s.FOLGE_TASK_ID;
                        d.Istdatum = s.IST_DATUM;
                        d.Istzeit = s.IST_ZEIT.ToTimeString();
                        d.LaufendeNr = s.LFD_NR;
                        d.Solldatum = s.SOLL_DATUM;
                        d.Sollzeit = s.SOLL_ZEIT.ToTimeString();
                        d.Startdatum = s.STARTDATUM;
                        d.Startzeit = s.STARTZEIT.ToTimeString();
                        d.Status = s.STATUS;
                        d.TaskId = s.TASK_ID;
                        d.ToDoWer = s.TODO_WER;
                        d.User = s.ZUSER;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_DOKU_01.ES_DOKUMENT, WfmDokument> Z_WFM_READ_DOKU_01_ES_DOKUMENT_To_WfmDokument
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_DOKU_01.ES_DOKUMENT, WfmDokument>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FileName = s.DATEINAME;
                        d.DocumentType = s.AR_OBJECT;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_WRITE_DOKU_01.ES_EXPORT, WfmDokumentInfo> Z_WFM_WRITE_DOKU_01_ES_EXPORT_To_WfmDokumentInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_WRITE_DOKU_01.ES_EXPORT, WfmDokumentInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Dateiname = s.DATEINAME;
                        d.Dokumentart = s.AR_OBJECT;
                        d.ObjectId = s.OBJECT_ID;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_CALC_DURCHLAUFZEIT_01.ES_STATISTIK, WfmDurchlaufStatistik> Z_WFM_CALC_DURCHLAUFZEIT_01_ES_STATISTIK_To_WfmDurchlaufStatistik
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_CALC_DURCHLAUFZEIT_01.ES_STATISTIK, WfmDurchlaufStatistik>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.DurchschnittDauer = s.DURCHSCHNITT_DAUER.ToString();

                        d.AnzGes = s.ANZ_GES.ToString();

                        d.AnzStdLe03 = s.ANZ_STD_LE_03.ToString();
                        d.AnzStd0410 = s.ANZ_STD_04_10.ToString();
                        d.AnzStd1120 = s.ANZ_STD_11_20.ToString();
                        d.AnzStd2130 = s.ANZ_STD_21_30.ToString();
                        d.AnzStd3140 = s.ANZ_STD_31_40.ToString();
                        d.AnzStdGt40 = s.ANZ_STD_GT_40.ToString();

                        d.AnzKlaerLe03 = s.ANZ_KLAER_LE_03.ToString();
                        d.AnzKlaer0410 = s.ANZ_KLAER_04_10.ToString();
                        d.AnzKlaer1120 = s.ANZ_KLAER_11_20.ToString();
                        d.AnzKlaer2130 = s.ANZ_KLAER_21_30.ToString();
                        d.AnzKlaer3140 = s.ANZ_KLAER_31_40.ToString();
                        d.AnzKlaerGt40 = s.ANZ_KLAER_GT_40.ToString();

                        d.AnzAlleLe03 = s.ANZ_ALLE_LE_03.ToString();
                        d.AnzAlle0410 = s.ANZ_ALLE_04_10.ToString();
                        d.AnzAlle1120 = s.ANZ_ALLE_11_20.ToString();
                        d.AnzAlle2130 = s.ANZ_ALLE_21_30.ToString();
                        d.AnzAlle3140 = s.ANZ_ALLE_31_40.ToString();
                        d.AnzAlleGt40 = s.ANZ_ALLE_GT_40.ToString();
                    }));
            }
        }

        static public ModelMapping<Z_WFM_CALC_DURCHLAUFZEIT_01.ET_OUT, WfmDurchlaufSingle> Z_WFM_CALC_DURCHLAUFZEIT_01_ET_OUT_To_WfmDurchlaufSingle
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_CALC_DURCHLAUFZEIT_01.ET_OUT, WfmDurchlaufSingle>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.KundenNr = s.KUNNR;
                        d.VorgNrAbmAuf = s.VORG_NR_ABM_AUF;
                        d.ErledigtDatum = s.ERLEDIGT_DATUM;
                        d.AbmeldeArt = s.ABMELDEART;
                        d.Selektion1 = s.SELEKTION1;
                        d.Selektion2 = s.SELEKTION2;
                        d.Selektion3 = s.SELEKTION3;
                        d.Referenz1 = s.REFERENZ1;
                        d.Referenz2 = s.REFERENZ2;
                        d.Referenz3 = s.REFERENZ3;
                        d.FahrgestellNr = s.FAHRG;
                        d.Kennzeichen = s.KENNZ;
                        d.DurchlaufzeitStunden = s.DURCHLAUFZEIT_STUNDEN;
                        d.DurchlaufzeitTage = s.DURCHLAUFZEIT_TAGE;
                        d.AnlageDatum = s.ANLAGEDATUM;
                    }));
            }
        }

        #endregion

        #region ToSap

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMART, AbmeldeArtSelektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_ABMART_From_AbmeldeArtSelektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMART, AbmeldeArtSelektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.ABMELDEART = s.AbmeldeArt;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMSTAT, AbmeldeStatusSelektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_ABMSTAT_From_AbmeldeStatusSelektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMSTAT, AbmeldeStatusSelektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.ABMELDESTATUS = s.AbmeldeStatus;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_KDAUF, KundenAuftragsNrSelektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_KDAUF_From_KundenAuftragsNrSelektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_KDAUF, KundenAuftragsNrSelektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.KDAUF_VON = s.KundenAuftragsNrVon;
                        d.KDAUF_BIS = s.KundenAuftragsNrBis;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF1, Referenz1Selektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF1_From_Referenz1Selektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF1, Referenz1Selektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.REF1_VON = s.Referenz1Von;
                        d.REF1_BIS = s.Referenz1Bis;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF2, Referenz2Selektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF2_From_Referenz2Selektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF2, Referenz2Selektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.REF2_VON = s.Referenz2Von;
                        d.REF2_BIS = s.Referenz2Bis;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF3, Referenz3Selektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF3_From_Referenz3Selektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF3, Referenz3Selektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.REF3_VON = s.Referenz3Von;
                        d.REF3_BIS = s.Referenz3Bis;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_SOLLDAT, SolldatumSelektion> Z_WFM_READ_AUFTRAEGE_01_GT_SEL_SOLLDAT_From_SolldatumSelektion
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_SOLLDAT, SolldatumSelektion>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.SOLLDATUM_VON = s.SolldatumVonBis.StartDate;
                        d.SOLLDATUM_BIS = s.SolldatumVonBis.EndDate;
                    }));
            }
        }

        static public ModelMapping<Z_WFM_WRITE_INFO_01.GT_DATEN, WfmInfo> Z_WFM_WRITE_INFO_01_GT_DATEN_From_WfmInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_WRITE_INFO_01.GT_DATEN, WfmInfo>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.VORG_NR_ABM_AUF = s.VorgangsNrAbmeldeauftrag;
                        d.TEXT = s.Text;
                        d.NEW_INFO_KU = "X";
                    }));
            }
        }

        static public ModelMapping<Z_WFM_WRITE_DOKU_01.GS_DOKUMENT, WfmDokument> Z_WFM_WRITE_DOKU_01_GS_DOKUMENT_From_WfmDokument
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_WFM_WRITE_DOKU_01.GS_DOKUMENT, WfmDokument>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.AR_OBJECT = s.DocumentType;
                        d.DATEINAME = s.FileName;
                    }));
            }
        }

        #endregion
    }
}