using System.Collections.Generic;
using GeneralTools.Services;

namespace SapORM
{
    public class TestViewModel : Store 
    {
        public List<string> MyNames 
        { 
            get 
            { 
                return PropertyCacheGet(() =>
                    {
                        var x = "Walter";
                        return new List<string> {"Susi", "Matz", x};
                    }); 
            } 
        }

        public void Reset()
        {
            PropertyCacheClear<TestViewModel>(m => m.MyNames);
        }

        public void Set(List<string> myNames)
        {
            PropertyCacheSet<TestViewModel>(m => m.MyNames, myNames);

            PropertyCacheSet(this, m => m.MyNames, myNames);
        }
    }
}
