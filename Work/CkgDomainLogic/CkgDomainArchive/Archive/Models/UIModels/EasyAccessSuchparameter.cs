using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class EasyAccessSuchparameter
    {
        public List<string> ArchiveTypes { get; set; }

        [LocalizedDisplay(LocalizeConstants.ArchiveType)]
        public string Archivtyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.Archives)]
        public List<EasyAccessArchive> Archives { get; set; }

        [LocalizedDisplay(LocalizeConstants.Archives)]
        public List<EasyAccessArchive> ArchivesOfType { get; set; }

        public List<EasyAccessResultField> SearchFields { get; set; }

        public EasyAccessSuchparameter()
        {
            ArchiveTypes = new List<string>();
            Archives = new List<EasyAccessArchive>();
            ArchivesOfType = new List<EasyAccessArchive>();
            SearchFields = new List<EasyAccessResultField>();
        }
    }
}
