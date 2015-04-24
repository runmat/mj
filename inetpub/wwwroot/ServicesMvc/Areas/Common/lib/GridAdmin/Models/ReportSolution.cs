using System;

namespace ServicesMvc.DomainCommon.Models
{
    public class ReportSolution
    {
        public bool AdminIsAuthorized { get; set; }

        public string AdminUserName { get; set; }

        public int AppID { get; set; }

        public DateTime CallingDateTime { get; set; }
    }
}