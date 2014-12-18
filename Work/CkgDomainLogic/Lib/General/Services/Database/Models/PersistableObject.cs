using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class PersistableObject : IPersistableObjectContainer
    {
        [Key]
        public int ID { get; set; }

        [NotMapped, XmlIgnore]
        public string ObjectKey { get { return ID.ToString(); }  }


        public string OwnerKey { get; set; }

        public string GroupKey { get; set; }

        public string ObjectType { get; set; }

        public string ObjectData { get; set; }


        public DateTime? InsertDatum { get; set; }
        public string InsertUser { get; set; }

        public DateTime? UpdDatum { get; set; }
        public string UpdUser { get; set; }
    }
}
