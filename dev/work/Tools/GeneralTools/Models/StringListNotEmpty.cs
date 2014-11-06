using System.Collections.Generic;
using System.Linq;

namespace GeneralTools.Models
{
    public class StringListNotEmpty : List<string>
    {
        public StringListNotEmpty(params string[] strings)
        {
            CreateValidItems(strings);
        }

        public StringListNotEmpty(IEnumerable<string> strings)
        {
            CreateValidItems(strings);
        }

        private void CreateValidItems(IEnumerable<string> strings)
        {
            strings.ToList().ForEach(s =>
            {
                if (s.IsNotNullOrEmpty())
                    Add(s);
            });
        }
    }

    public class ListNotEmpty<T> : List<T> where T : class
    {
        public ListNotEmpty(params T[] objects)
        {
            CreateValidItems(objects);
        }

        public ListNotEmpty(IEnumerable<T> objects)
        {
            CreateValidItems(objects);
        }

        private void CreateValidItems(IEnumerable<T> objects)
        {
            objects.ToList().ForEach(o =>
            {
                if (o != null)
                    Add(o);
            });
        }
    }
}
