using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace CkgDomainLogic.Logs.Models
{
    [Table("SapBapi")]
    public class SapLogItemDetailed
    {
        [Key]
        [GridHidden]
        public int Id { get; set; }

        public string Bapi { get; set; }

        public string ImportTables { get; set; }


        //[GridRawHtml, NotMapped]
        //[LocalizedDisplay(LocalizeConstants.StackContext)]
        //public string StackContextHtml { get { return StackContextItemTemplate == null ? "" : StackContextItemTemplate(this.StackContext); } }


        #region Grid Hidden

        //[GridHidden]
        //public string LogonContext { get; set; }

        //[GridHidden]
        //public string DataContext { get; set; }

        //[GridHidden, NotMapped]
        //public StackContext StackContext
        //{
        //    get
        //    {
        //        if (StackContextItemTemplate == null || DataContext.IsNullOrEmpty())
        //            return new StackContext();

        //        return XmlService.XmlTryDeserializeFromString<StackContext>(DataContext);
        //    }
        //}

        //[GridHidden, NotMapped]
        //public static Func<StackContext, string> StackContextItemTemplate { get; set; }

        #endregion
    }
}
