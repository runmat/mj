﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugvoravisierungDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugvoravisierungDataService
    {
        public FahrzeugvoravisierungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<FahrzeugvoravisierungUploadModel> UploadItems { get; set; }

        public void ValidateFahrzeugeinsteuerungCsvUpload()
        {
          //  UploadItems.ForEach(ValidateSingleUploadItem);
        }

        public string SaveCsvUpload()
        {
            Z_DPM_IMP_MODELL_ID_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
            
            //var vgList = AppModelMappings.Z_DPM_UPLOAD_GRUDAT_TIP_01_GT_IN_From_FahrzeugeinsteuerungUploadModel.CopyBack(UploadItems).ToList();
            //SAP.ApplyImport(vgList);

            //SAP.Execute();

            //if (SAP.ResultCode != 0)
            //    return SAP.ResultMessage;

            //var outList = Z_DPM_UPLOAD_GRUDAT_TIP_01.GT_OUT.GetExportList(SAP);

            //foreach (var item in UploadItems)
            //{
            //    item.SaveStatus = outList.Find(o => o.CHASSIS_NUM.ToUpper() == item.Fahrgestellnummer.ToUpper()).BEM;
            //}

            return "";
        }

        void ValidateSingleUploadItem(FahrzeugvoravisierungUploadModel item)
        {
            
        }

        public bool SaveUploadItems(List<FahrzeugvoravisierungUploadModel> uploadItems)
        {
            throw new NotImplementedException();
        }
    }
}
