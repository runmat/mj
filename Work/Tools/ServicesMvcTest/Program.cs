using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CkgDomainLogic.Services;

namespace ServicesMvcTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DashboardService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index");
        }
    }
}
