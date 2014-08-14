using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class UploadFiles : UiModel
    {
        public string FahrtIndex { get; set; }
        
        public string[] UploadFileNameArray { get; set; }

        private string _uploadGroupNames;
        public string UploadGroupNames
        {
            get { return _uploadGroupNames; }
            set
            {
                _uploadGroupNames = value;

                if (UploadFileNameArray == null)
                    UploadFileNameArray = new string[UploadGroupNameArray.Length];
            }
        }
        public string[] UploadGroupNameArray { get { return (string.IsNullOrEmpty(UploadGroupNames) ? new string[0] : UploadGroupNames.Split(',')); } }

        private IEnumerable<string> UploadFilesValid { get { return UploadFileNameArray.Where(u => !string.IsNullOrEmpty(u)).ToList(); } }

        public override string ViewName
        {
            get { return "Partial/UploadFilesEdit"; }
        }

        public override bool IsEmpty
        {
            get { return UploadFilesValid.None(); }
        }

        public override GeneralEntity SummaryItem
        {
            get
            {
                var countUploadFiles = UploadFilesValid.Count();
                var uploadFilesSummaryBody = "";
                if (countUploadFiles > 0)
                    uploadFilesSummaryBody = string.Format("{0} Dokument{1}", countUploadFiles, (countUploadFiles > 1 ? "e" : ""));

                var parallelSummaryBodies = "";
                if (ParallelSummaryUiModels != null)
                    parallelSummaryBodies = string.Join(", ", ParallelSummaryUiModels.Select(parallel => parallel.SummaryItem.Body).ToArray());

                uploadFilesSummaryBody = string.Join(", ", new StringListNotEmpty(parallelSummaryBodies, uploadFilesSummaryBody));

                return new GeneralEntity
                {
                    Title = string.Join(",", HeaderShort.Split(',').Take(2).ToArray()),
                    Body = uploadFilesSummaryBody,
                };
            }
        }

        public static List<WebUploadProtokoll> WebUploadProtokolle { get; set; }

        public List<WebUploadProtokoll> UploadProtokolle
        {
            get
            {
                var list = new List<WebUploadProtokoll>();

                if (string.IsNullOrEmpty(UploadGroupNames) || WebUploadProtokolle == null)
                    return list;

                for (var i = 0; i < UploadGroupNameArray.Length; i++)
                {
                    if (UploadFileNameArray[i].IsNullOrEmpty())
                        continue;

                    var pArt = UploadGroupNameArray[i];
                    list.Add(new WebUploadProtokoll
                                 {
                                     Protokollart = pArt,
                                     Kategorie = WebUploadProtokolle.First(p => p.Protokollart == pArt).Kategorie,
                                     FahrtIndex = FahrtIndex,
                                 });
                }

                return list;
            }
        }
    }
}
