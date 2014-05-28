using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcTools.Data;

namespace MvcTools.Models
{
    public class ContentEntity
    {
        [Key]
        public string ID { get; set; }
        
        public string Text { get; set; }

        public void DbLoad()
        {
            var ce = MvcRepository.ContentEntities.FirstOrDefault(h => h.ID == ID);
            if (ce == null)
            {
                ce = new ContentEntity { ID = this.ID, Text = "[bitte Inhalt setzen]" };
                MvcRepository.AddContentEntity(ce);
            }
                
            Text = ce.Text;
        }

        public void DbSave()
        {
            var ce = MvcRepository.ContentEntities.FirstOrDefault(h => h.ID == ID);
            if (ce != null)
            {
                ce.Text = Text;
                MvcRepository.SubmitChanges();
            }
        }
    }
}
