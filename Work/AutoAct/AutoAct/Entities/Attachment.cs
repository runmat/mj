using System;
using System.IO;
using AutoAct.Enums;

namespace AutoAct.Entities
{
    /// <summary>
    /// Nur PDF Dokumente dürfen hochgeladen werden
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Wird von AutoAct generiert
        /// </summary>
        public Int64 Id { get; set; }

        public string FriendlyName { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Url des Dokuments in 
        /// </summary>
        public string AccessUrl { get; set; }

        /// <summary>
        /// Pfad UND Dateiname als string
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Nur application/pdf ist erlaubt
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///  Pro Attachemnt Type ist nur ein Dokument zulässig
        /// </summary>
        public AttachmentType AttachmentType { get; set; }

        /// <summary>
        ///  Wird von AutoAct gesetzt
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
