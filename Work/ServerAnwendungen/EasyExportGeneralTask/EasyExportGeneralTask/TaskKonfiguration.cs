using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EasyExportGeneralTask
{
    public class TaskKonfiguration
    {
        public string Name { get; set; }
        public string Kundennummer { get; set; }
        public AblaufTyp Ablauf { get; set; }
        public bool AbfrageNachDatum { get; set; }
        public DateTime Abfragedatum { get; set; }
        public bool LogfilesMitTitelStattKennzeichen { get; set; }
        public bool VerzeichnisseLeeren { get; set; }
        public bool DatumInSapSetzen { get; set; }
        public bool MailsSenden { get; set; }
        public string MailEmpfaenger { get; set; }

        public bool NoDataMailsSenden { get; set; }
        public string NoDataMailEmpfaenger { get; set; }

        public string easyLocation { get; set; }
        public string easyBlobPathLocal { get; set; }

        public List<ArchivDefinition> Archive { get; set; }

        public string exportPathZBII { get; set; }
        public string exportPathSteuerB { get; set; }
        public string exportPathZip { get; set; }

        [XmlIgnore]
        public string easyArchiveNameStandard { 
            get
            {
                var arc = Archive.Find(a => a.Typ == ArchivTyp.Standard);
                if (arc != null)
                {
                    var arcName = arc.Name;
                    if (arc.IstJahresarchiv)
                    {
                        var heute = DateTime.Today;
                        arcName += heute.ToString((arc.IstJahrVierstellig ? "yyyy" : "yy"));
                    }
                    return arcName;
                }
                return "";
            } 
        }
        [XmlIgnore]
        public string easyArchiveNameDokumente { 
            get
            {
                var arc = Archive.Find(a => a.Typ == ArchivTyp.Dokumente);
                if (arc != null)
                {
                    var arcName = arc.Name;
                    if (arc.IstJahresarchiv)
                    {
                        var heute = DateTime.Today;
                        arcName += heute.ToString((arc.IstJahrVierstellig ? "yyyy" : "yy"));
                    }
                    return arcName;
                }
                return "";
            } 
        }
        [XmlIgnore]
        public string easyArchiveNameSteuern
        {
            get
            {
                var arc = Archive.Find(a => a.Typ == ArchivTyp.Steuerbescheide);
                if (arc != null)
                {
                    var arcName = arc.Name;
                    if (arc.IstJahresarchiv)
                    {
                        var heute = DateTime.Today;
                        arcName += heute.ToString((arc.IstJahrVierstellig ? "yyyy" : "yy"));
                    }
                    return arcName;
                }
                return "";
            }
        }
        [XmlIgnore]
        public string easyArchiveNameFirst
        {
            get
            {
                if (Archive.Count > 0)
                {
                    var arc = Archive[0];
                    var arcName = arc.Name;
                    if (arc.IstJahresarchiv)
                    {
                        var heute = DateTime.Today;
                        arcName += heute.ToString((arc.IstJahrVierstellig ? "yyyy" : "yy"));
                    }
                    return arcName;
                }
                return "";
            }
        }
    }

    public enum ArchivTyp
    {
        Standard,
        Dokumente,
        Steuerbescheide
    }

    public enum AblaufTyp
    {
        Athlon,
        Alphabet,
        LeasePlan,
        CharterWay_All,
        CharterWay_Single,
        EuropaService,
        StarCar,
        XLeasing,
        XLCheck,
        DCBank,
        DaimlerFleet,
        SixtMobility,
        Autoinvest,
        Europcar,
        WKDA,
        WKDA_Selbstabmelder,
        StarCar2,
        CarDocu_Strafzettel,
        CarDocu_Abmeldung,

        WKDA_AT
    }

    public class ArchivDefinition
    {
        public string Name { get; set; }
        public ArchivTyp Typ { get; set; }
        public bool IstJahresarchiv { get; set; }
        public bool IstJahrVierstellig { get; set; }
    }
}
