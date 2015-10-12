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

        private List<DomainUser> _domainUsers = new List<DomainUser>();
        public List<DomainUser> DomainUsers
        {
            get { return _domainUsers; }
            set { _domainUsers = value; }
        }

        private List<DomainLocation> _domainLocations = new List<DomainLocation>();
        public List<DomainLocation> DomainLocations
        {
            get { return _domainLocations; }
            set { _domainLocations = value; }
        }

        private List<Archive> _archives = new List<Archive>();
        public List<Archive> Archives
        {
            get { return _archives; }
            set { _archives = value; }
        }

        private List<Archive> _archivesForDocTypes;
        [XmlIgnore] 
        public List<Archive> ArchivesForDocTypes
        {
            get { return _archivesForDocTypes ?? (_archivesForDocTypes = Archives.Where(archive => !archive.IsInternal).ToList()); }
        }

        private SmtpSettings _smtpSettings = new SmtpSettings();
        public SmtpSettings SmtpSettings
        {
            get { return _smtpSettings; }
            set { _smtpSettings = value; }
        }

        private SapSettings _sapSettings = new SapSettings();
        public SapSettings SapSettings
        {
            get { return _sapSettings; }
            set { _sapSettings = value; }
        }

        private ScanSettings _scanSettings = new ScanSettings();
        public ScanSettings ScanSettings 
        { 
            get { return _scanSettings; }
            set { _scanSettings = value; }
        }

        [XmlIgnore]
        public Archive ZipArchive { get { return Archives.FirstOrDefault(archive => archive.ID == "ZIP"); } }


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
                {}
                 
                return "";
            }
        }
    }
}
