using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
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
                                                         d.EingangGesamt = s.FZG_EING_GES.ToInt();
                                                         d.AusVorjahr = s.FZG_AUS_VJ.ToInt();
                                                         d.ZulassungVormonat = s.ZUL_VM.ToInt();
                                                         d.ZulassungLfdMonat = s.ZUL_LFD_M.ToInt();
                                                         d.ZulassungGesamtMonat = s.ZUL_GES_M.ToInt();
                                                         d.ZulassungProzLfdMonat = s.ZUL_PZ_LFD_M.ToInt();
                                                         d.ZulassungProzFolgeMonat = s.ZUL_PZ_FM.ToInt();
                                                         d.Bestand = s.FZG_BEST.ToInt();
                                                         d.Ausgerüstet = s.FZG_AUSGER.ToInt();
                                                         d.MitBrief = s.FZG_M_BRIEF.ToInt();
                                                         d.Zulassungsbereit = s.FZG_ZUL_BER.ToInt();
                                                         d.OhneUnitnummer = s.FZG_OHNE_UNIT.ToInt();                                                        
                                                         d.Sipp = s.ZSIPP_CODE;
                                                         d.Gesperrt = s.FZG_GESP.ToInt();
                                                     }));
            }
        }

        #endregion


    }
}