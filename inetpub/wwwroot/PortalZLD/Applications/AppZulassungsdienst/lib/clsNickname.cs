using System;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class clsNickname : SapOrmBusinessBase
    {
        public string KundenName { get; private set; }
        public string KundenNickname { get; set; }

        public clsNickname(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void GetKundeNickname(string SearchKunnr)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_GET_NICKNAME.Init(SAP, "I_VKBUR, I_KUNNR", VKBUR, SearchKunnr.ToSapKunnr());

                    CallBapi();

                    var Name1 = SAP.GetExportParameter("E_NAME1");
                    var Name2 = SAP.GetExportParameter("E_NAME1");
                    var Extension = SAP.GetExportParameter("E_EXTENSION1");
                    KundenNickname = SAP.GetExportParameter("E_NICK_NAME");

                    KundenName = String.Format("{0} ~ {1}", Name1, Name2);
                    if (!String.IsNullOrEmpty(Extension))
                        KundenName += " / " + Extension;
                });
        }

        public void SetKundeNickname(string SearchKunnr, bool Delete)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_SET_NICKNAME.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_KUNNR", SearchKunnr.ToSapKunnr());
                    SAP.SetImportParameter("I_NICK_NAME", KundenNickname);
                    SAP.SetImportParameter("I_DELETE", Delete.BoolToX());

                    CallBapi();
                });
        }
    }
}
