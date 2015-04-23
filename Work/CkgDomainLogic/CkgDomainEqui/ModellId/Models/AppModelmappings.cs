// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModellId> Z_DPM_READ_MODELID_TAB__GT_OUT_To_ModellId
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModellId>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                     {
                                                         d.ID = s.ZZMODELL;
                                                         d.Bezeichnung = s.ZZBEZEI;
                                                         
                                                         d.HerstellerCode = s.HERST;
                                                         d.HerstellerName = s.HERST_T;

                                                         d.SippCode = s.SIPP1.NotNullOr(" ") + s.SIPP2.NotNullOr(" ") +
                                                                      s.SIPP3.NotNullOr(" ") + s.SIPP4.NotNullOr(" ");
                                                         
                                                         d.LaufzeitTage = s.ZLAUFZEIT.ToInt(0);
                                                         d.Antrieb = s.ANTR;

                                                         d.LaufzeitBindung = s.ZLZBINDUNG.XToBool();
                                                         d.Lkw = s.LKW.XToBool();
                                                         d.Winterreifen = s.WINTERREIFEN.XToBool();
                                                         d.AnhaengerKupplung = s.AHK.XToBool();
                                                         d.NaviVorhanden = s.NAVI_VORH.XToBool();
                                                         d.SecurityFleet = s.SECU_FLEET.XToBool();
                                                         d.KennzeichenLeasingFahrzeug = s.LEASING.XToBool();
                                                         d.Bluetooth = s.BLUETOOTH.XToBool();
                                                     }));
            }
        }

        #endregion


        #region Save to Repository

        static public void Z_DPM_CHANGE_MODELID_From_ModellId(ISapDataService sap, ILogonContext logonContext, ModellId s)
        {
            sap.SetImportParameter("I_VERKZ", (s.InsertModeTmp ? "N" : "U"));

            sap.SetImportParameter("I_KUNNR", logonContext.KundenNr.ToSapKunnr());
            sap.SetImportParameter("I_UNAME", logonContext.UserName.SubstringTry(0, 12));

            sap.SetImportParameter("I_MODELID", s.ID);
            sap.SetImportParameter("I_HERST", s.HerstellerCode);
            sap.SetImportParameter("I_ZZBEZEI", s.Bezeichnung);

            sap.SetImportParameter("I_ZSIPP_CODE", s.SippCode);
            sap.SetImportParameter("I_ZLAUFZEIT", s.LaufzeitTage.ToString());
            sap.SetImportParameter("I_ANTR", s.Antrieb);
            
            sap.SetImportParameter("I_ZLZBINDUNG", s.LaufzeitBindung.BoolToX());
            sap.SetImportParameter("I_LKW", s.Lkw.BoolToX());
            sap.SetImportParameter("I_WINTERREIFEN", s.Winterreifen.BoolToX());
            sap.SetImportParameter("I_AHK", s.AnhaengerKupplung.BoolToX());
            sap.SetImportParameter("I_NAVI_VORH", s.NaviVorhanden.BoolToX());
            sap.SetImportParameter("I_SECU_FLEET", s.SecurityFleet.BoolToX());
            sap.SetImportParameter("I_LEASING", s.KennzeichenLeasingFahrzeug.BoolToX());
            sap.SetImportParameter("I_BLUETOOTH", s.Bluetooth.BoolToX());
        }

        #endregion
    }
}