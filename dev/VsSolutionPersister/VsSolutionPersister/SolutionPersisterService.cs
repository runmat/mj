using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
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
            var path = SolutionPath;
            var gitFolder = "";
            while (gitFolder.IsNullOrEmpty())
            {
                gitFolder = Directory.GetDirectories(path, ".git", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (gitFolder.IsNullOrEmpty())
                {
                    path += @"..\";
                    gitFolder = Directory.GetDirectories(path, ".git", SearchOption.TopDirectoryOnly).FirstOrDefault();
                }
            }

            var gitHeadFileName = Path.Combine(path, ".git", "HEAD");
            if (!File.Exists(gitHeadFileName))
                return "";
            var head = File.ReadAllText(gitHeadFileName);
            var headParts = head.Split('/');
            if (headParts.None())
                return "";

            return headParts.Last().Trim();
        }

        public string GetCurrentSolutionStartpageUrl()
        {
            var solutionUserFilename = Path.Combine(SolutionPath, SolutionName + ".csproj.user");
            if (!File.Exists(solutionUserFilename))
                return "";

            var doc = new XmlDocument();
            doc.Load(solutionUserFilename);
            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("a", "http://schemas.microsoft.com/developer/msbuild/2003");
            var nodes = doc.SelectNodes("//a:StartPageUrl", mgr);
            if (nodes == null || nodes.Count == 0)
                return "";

            return nodes[0].InnerText;
        }
    }
}
