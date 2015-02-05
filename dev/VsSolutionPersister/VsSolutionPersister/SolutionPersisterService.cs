// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;

namespace VsSolutionPersister
{
    public class SolutionPersisterService
    {
        public string SolutionPath { get { return System.Configuration.ConfigurationManager.AppSettings["SolutionPathToPersist"]; } }

        public string SolutionName { get { return SolutionPath.NotNullOrEmpty().Split('\\').Last(); } }

        public string GetCurrentGitBranchName()
        {
            return "";
        }

        public string GetCurrentSolutionStartupPath()
        {
            return "";
        }
    }
}
