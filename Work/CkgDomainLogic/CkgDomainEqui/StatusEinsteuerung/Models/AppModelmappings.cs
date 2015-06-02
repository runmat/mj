using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class AppStatusEinsteuerungModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB, StatusEinsteuerung> Z_M_EC_AVM_STATUS_EINSTEUERUNG_GT_WEB_To_StatusEinsteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB, StatusEinsteuerung>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                     {
                                                         d.PDINummer = s.ZZCARPORT;
                                                         d.PDIBezeichnung = s.ZNAME1;
                                                         d.Fahrzeuggruppe = s.ZFZG_GROUP;
                                                         d.HerstellerCode = s.ZZHERST;
                                                         d.Hersteller = s.ZKLTXT;
                                                         d.ModellCode = s.ZZMODELL;
                                                         d.Modellbezeichnung = s.ZZBEZEI;
                                                         d.EingangGesamt = s.FZG_EING_GES.ToInt(0);
                                                         d.AusVorjahr = s.FZG_AUS_VJ.ToInt(0);
                                                         d.ZulassungVormonat = s.ZUL_VM.ToInt(0);
                                                         d.ZulassungLfdMonat = s.ZUL_LFD_M.ToInt(0);
                                                         d.ZulassungGesamtMonat = s.ZUL_GES_M.ToInt(0);
                                                         d.ZulassungProzLfdMonat = s.ZUL_PZ_LFD_M.ToInt(0);
                                                         d.ZulassungProzFolgeMonat = s.ZUL_PZ_FM.ToInt(0);
                                                         d.Bestand = s.FZG_BEST.ToInt(0);
                                                         d.Ausgerüstet = s.FZG_AUSGER.ToInt(0);
                                                         d.MitBrief = s.FZG_M_BRIEF.ToInt(0);
                                                         d.Zulassungsbereit = s.FZG_ZUL_BER.ToInt(0);
                                                         d.OhneUnitnummer = s.FZG_OHNE_UNIT.ToInt(0);                                                        
                                                         d.Sipp = s.ZSIPP_CODE;
                                                         d.Gesperrt = s.FZG_GESP.ToInt(0);
                                                     }));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_STATUS_BESTAND.GT_WEB, StatusEinsteuerung> Z_M_EC_AVM_STATUS_BESTAND_GT_WEB_To_StatusEinsteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_STATUS_BESTAND.GT_WEB, StatusEinsteuerung>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                 {
                                                     d.PDINummer = s.ZZCARPORT;
                                                     d.PDIBezeichnung = s.ZNAME1;
                                                     d.Fahrzeuggruppe = s.ZFZG_GROUP;
                                                     d.HerstellerCode = s.ZZHERST;
                                                     d.Hersteller = s.ZKLTXT;
                                                     d.ModellCode = s.ZZMODELL;
                                                     d.Modellbezeichnung = s.ZZBEZEI;
                                                     d.Bestand = s.FZG_BEST.ToInt(0);
                                                     d.Ausgerüstet = s.FZG_AUSGER.ToInt(0);
                                                     d.MitBrief = s.FZG_M_BRIEF.ToInt(0);
                                                     d.Zulassungsbereit = s.FZG_ZUL_BER.ToInt(0);
                                                     d.OhneUnitnummer = s.FZG_OHNE_UNIT.ToInt(0);
                                                     d.Sipp = s.ZSIPP_CODE;
                                                     d.Gesperrt = s.FZG_GESP.ToInt(0);
                                                 }));
            }
        }

        #endregion

    }
}