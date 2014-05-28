using System.Collections;

namespace BaseService
{

    public class Zulassungen : CollectionBase, IList
    {
        public void Add(Zulassung dZulassung)
        {
            List.Add(dZulassung);
           
        }



        public Zulassung this[int index]
        {
            get
            {
                return ((Zulassung)List[index]);
            }
        }

        //public readonly virtual Zulassung Item(int index)
        //{
        //    get { return (Zulassung)this.List(index); }
        //}

    }
}