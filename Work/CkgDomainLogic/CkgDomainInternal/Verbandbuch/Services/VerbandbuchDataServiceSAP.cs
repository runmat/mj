using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainInternal.Verbandbuch.Contracts;
using CkgDomainInternal.Verbandbuch.Models;
using CkgDomainLogic.General.Models;
using SapORM.Contracts;
using SapORM.Models;
using CkgDomainLogic.General.Services;
using AppModelMappings = CkgDomainInternal.Verbandbuch.Models.AppModelMappings;


namespace CkgDomainInternal.Verbandbuch.Services
{
    public class VerbandbuchDataServiceSAP: CkgGeneralDataServiceSAP, IVerbandbuchDataService
    {
        public VerbandbuchDataServiceSAP(ISapDataService sap) : base(sap)
        {
        }

        public List<VerbandbuchModel> GetVerbandbuchEntries(string vkbur)
        {
            Z_VB_EXPORT_FAELLE.Init(SAP, "I_VKBUR", vkbur);
            SAP.Execute();

            var sapresult = Z_VB_EXPORT_FAELLE.GT_VERBANDBUCH.GetExportList(SAP);
            var webResult = AppModelMappings.Z_VB_EXPORT_FAELLE_TO_WEB.Copy(sapresult);
           
            return webResult.ToList();
        }

        public string SaveVorfallSAP(VerbandbuchModel vbModel)
        {

            Z_VB_IMPORT_FALL.Init(SAP);

            var sapList = new List<Z_VB_IMPORT_FALL.GT_VERBANDBUCH>
            {
                new Z_VB_IMPORT_FALL.GT_VERBANDBUCH
                {
                    
                    ART_HILF = vbModel.FirstAidMeasures,
                    ART_VERL = vbModel.Injury,
                    DATUM_HILF = vbModel.DateOfFirstAid.Value.Date,
                    DATUM_UNF = vbModel.DateOfAccident.Value.Date,
                    HERGANG = vbModel.CourseOfAccident,
                    NAME_HELFER = vbModel.FirstResponder,
                    NAME_VERL = vbModel.InjuredPerson,
                    NAME_ZEUG = vbModel.WitnessName,
                    ORT = vbModel.PlaceOfAccident,
                    ZEIT_HILF = vbModel.TimeOfFirstAid, 
                    ZEIT_UNF = vbModel.TimeOfAccident, 
                    VKBUR = vbModel.Vkbur
                    
                }
            };

            
            SAP.ApplyImport(sapList);
            Z_VB_IMPORT_FALL.GT_VERBANDBUCH.GetExportListWithExecute(SAP);

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            return String.Empty;
        }


    }
}
