using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class PersistableObjectContainer : IPersistableObjectContainer
    {
        [Key]
        public int ID { get; set; }

        [NotMapped, XmlIgnore]
        public string ObjectKey { get { return ID.ToString(); }  set {} }

        public string ObjectName { get; set; }

        [NotMapped, XmlIgnore]
        public object Object { get; set; }

        public string ObjectType { get; set; }

        public string ObjectData { get; set; }


        public string OwnerKey { get; set; }

        public string GroupKey { get; set; }


        public DateTime? EditDate { get; set; }

        public string EditUser { get; set; }
    }
}
