using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.General.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Autohaus.Services
{
    public class EsdBeauftragungDataServiceSAP : CkgGeneralDataServiceSAP, IEsdBeauftragungDataService
    {
        public EsdBeauftragungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        private const string ValidCountryCodes = "AT,BE,CH,CZ,DE,DK,ES,FI,FR,GB,HU,IE,IT,LU,NL,PL,PT,RO,SE";

        public List<Land> LaenderAuswahlliste
        {
            get
            {
                return PropertyCacheGet(() => 
                            ValidCountryCodes.Split(',')
                            .Select(code => new Land { ID = code, Name = Localize.TranslateResourceKey("Country_" + code) } ))
                            .ToList();
            }
        }


        public string GetEmpfaengerEmailAdresse()
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP, "I_KENNUNG", "ESD-BEAUFTRAGUNG");

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var sapItem = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportList(SAP).FirstOrDefault();

                if (sapItem != null)
                    return sapItem.EMAIL;
            }

            return "";
        }
    }
}
