using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SepiaSyncLib.Services;

namespace SepiaSync
{
    class Program
    {
        static void Main()
        {
            using (var repository = new SqlDbRepository())
            {
                repository.Action();
            }
        }
    }
}
