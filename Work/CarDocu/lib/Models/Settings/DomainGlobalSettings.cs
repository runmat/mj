using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarDocu.Models.Settings;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class DomainGlobalSettings
    {
        public string ScanDataUrl { get; set; }

        public string DomainDataUrl { get; set; }

        public List<DomainUser> DomainUsers { get; set; } = new List<DomainUser>();

        public List<DomainLocation> DomainLocations { get; set; } = new List<DomainLocation>();

        public List<Archive> Archives { get; set; } = new List<Archive>();

        private List<Archive> _archivesForDocTypes;
        [XmlIgnore] 
        public List<Archive> ArchivesForDocTypes
        {
            get { return _archivesForDocTypes ?? (_archivesForDocTypes = Archives.Where(archive => !archive.IsInternal).ToList()); }
        }

        public SmtpSettings SmtpSettings { get; set; } = new SmtpSettings();

        public SapSettings SapSettings { get; set; } = new SapSettings();

        public ScanSettings ScanSettings { get; set; } = new ScanSettings();

        [XmlIgnore]
        public Archive ZipArchive { get { return Archives.FirstOrDefault(archive => archive.ID == "ZIP"); } }

        [XmlIgnore]
        public Archive BackupArchive { get { return Archives.FirstOrDefault(archive => archive.ID == "BACKUP"); } }


        public bool MergeAvailableAndFixedArchives()
        {
            var difference = Archives.Count < Archive.FixedAvailableArchives.Count; 

            if (difference)
                Archive.FixedAvailableArchives.ForEach(fixedArchive =>
                    {
                        if (Archives.None(archive => archive.ID == fixedArchive.ID))
                            Archives.Add(fixedArchive);
                    });

            return difference;
        }

        public void PatchArchives()
        {
            var zipArchiv = Archives.FirstOrDefault(a => a.ID == "ZIP");
            if (zipArchiv != null)
            {
                zipArchiv.IsInternal = false;
                zipArchiv.BackgroundDeliveryDisabled = true;
            }

            var backupArchiv = Archives.FirstOrDefault(a => a.ID == "BACKUP");
            if (backupArchiv != null)
                backupArchiv.BackgroundDeliveryDisabled = true;
        }

        [XmlIgnore]
        public string TempPath {
            get
            {
                var tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CarDocu\\";
                if (new DirectoryInfo(tempPath).Exists)
                {
                    return tempPath;
                }
               
                try
                {
                    new DirectoryInfo(tempPath).Create();
                    return tempPath;
                }
                catch (Exception)
                {
                    // ignored
                }

                return "";
            }
        }
    }
}
