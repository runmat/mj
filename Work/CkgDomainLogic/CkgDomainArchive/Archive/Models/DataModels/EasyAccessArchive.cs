using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class EasyAccessArchive
    {

        public string Id { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public string Index { get; set; }

        public string IndexName { get; set; }

        public string TitleName { get; set; }

        public object DefaultQuery { get; set; }

        [LocalizedDisplay(LocalizeConstants.ArchiveType)]
        public string ArchiveType { get; set; }

        public bool Selected { get; set; }
    }
}
