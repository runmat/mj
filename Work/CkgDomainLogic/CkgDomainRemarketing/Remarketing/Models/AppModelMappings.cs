using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Remarketing.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_DPM_READ_AUFTR6_001.GT_WEB, Vermieter> Z_DPM_READ_AUFTR6_001_GT_WEB_To_Vermieter
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR6_001.GT_WEB, Vermieter>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VermieterId = s.POS_KURZTEXT;
                        d.VermieterName = s.POS_TEXT;
                    }
                ));
            }
        }

        static public ModelMapping<Z_DPM_READ_REM_002.GT_WEB, FehlendeDaten> Z_DPM_READ_REM_002_GT_WEB_To_FehlendeDaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_REM_002.GT_WEB, FehlendeDaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.VermieterId = s.AVNR;
                        d.VermieterName = s.TEXT_AVNR;
                        d.FahrgestellNr = s.FAHRGNR;
                        d.Kennzeichen = s.KENNZ;
                        d.Zb2Eingang = s.EGZB2DAT;
                        d.CarportEingang = s.HCEINGDAT;
                        d.EquiNr = s.EQUNR;
                        d.Rechnungsuebermittlung = s.UEBERM_RE;
                        d.Eingangsdatum = s.INDATUM;
                        d.Stilllegungsdatum = s.STILLDAT;
                        d.DatumHcUebergabeTuevSued = s.DAT_UEB_HC_TUEVSUED;
                        d.DatumErstellungBelastungsanzeige = s.ERDAT_BELAS;
                    }
                ));
            }
        }

        #endregion


        #region ToSap

        

        #endregion
    }
}