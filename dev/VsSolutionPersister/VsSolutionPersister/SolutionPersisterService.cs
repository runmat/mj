using System.Collections.Generic;
using GeneralTools.Services;
// ReSharper disable RedundantUsingDirective
using System.IO;
using System.Xml;
using System;
using System.Linq;
using System.Text;
using GeneralTools.Models;

namespace VsSolutionPersister
{
    public class SolutionPersisterService
    {
        private static string AppDataRootPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); } }

        private static string AppDataPath
        {
            get
            {
                var path = Path.Combine(AppDataRootPath, "VsSolutionPersister");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                return path;
            }
        }
        private static string AppDataItemsFileName { get { return Path.Combine(AppDataPath, "items.xml"); } }

        public string SolutionPath { get { return System.Configuration.ConfigurationManager.AppSettings["SolutionPathToPersist"]; } }

        public string SolutionName { get { return SolutionPath.NotNullOrEmpty().Split('\\').Last(); } }

        public string GetCurrentGitBranchName()
        {
            var path = GetCurrentGitFolder();

            var gitHeadFileName = Path.Combine(path, ".git", "HEAD");
            if (!File.Exists(gitHeadFileName))
                return "";
            var head = File.ReadAllText(gitHeadFileName);
            var headParts = head.Split('/');
            if (headParts.None())
                return "";

            return headParts.Last().Trim();
        }

        string GetCurrentGitFolder()
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
            return path;
        }

        public string GetCurrentRootGitFolder()
        {
            return new DirectoryInfo(GetCurrentGitFolder()).FullName;
        }

        public string GetSolutionStartpageUrl()
        {
            XmlDocument xmlDoc;
            XmlNode startPageUrlNode;
            string solutionUserFilename;

            GetSolutionUserFileXml(out xmlDoc, out startPageUrlNode, out solutionUserFilename);
            if (startPageUrlNode == null)
                return "";

            return startPageUrlNode.InnerText;
        }

        public void SaveSolutionStartpageUrl(string url)
        {
            XmlDocument xmlDoc;
            XmlNode startPageUrlNode;
            string solutionUserFilename;

            GetSolutionUserFileXml(out xmlDoc, out startPageUrlNode, out solutionUserFilename);
            if (startPageUrlNode == null)
                return;

            startPageUrlNode.InnerText = url;

            var sr = new StringWriter();
            xmlDoc.Save(sr);
            if (FileService.TryFileDelete(solutionUserFilename))
                File.WriteAllText(solutionUserFilename, sr.ToString(), new UnicodeEncoding());
        }

        private void GetSolutionUserFileXml(out XmlDocument xmlDoc, out XmlNode startPageUrlNode, out string solutionUserFilename)
        {
            xmlDoc = new XmlDocument();
            startPageUrlNode = null;

            solutionUserFilename = Path.Combine(SolutionPath, SolutionName + ".csproj.user");
            if (!File.Exists(solutionUserFilename))
                return;

            xmlDoc.Load(solutionUserFilename);
            var mgr = new XmlNamespaceManager(xmlDoc.NameTable);
            mgr.AddNamespace("a", "http://schemas.microsoft.com/developer/msbuild/2003");
            var nodes = xmlDoc.SelectNodes("//a:StartPageUrl", mgr);
            if (nodes == null || nodes.Count == 0)
                return;

            startPageUrlNode = nodes[0];
        }

        public List<SolutionItem> LoadSolutionItems()
        {
            if (!File.Exists(AppDataItemsFileName))
                return new List<SolutionItem>();

            return XmlService.XmlDeserializeFromFile<List<SolutionItem>>(AppDataItemsFileName);
        }

        public void SaveSolutionXmlItems(List<SolutionItem> items)
        {
            XmlService.XmlSerializeToFile(items, AppDataItemsFileName);
        }

        static string GetSolutionItemFolder(SolutionItem item)
        {
            return Path.Combine(AppDataPath, item.Name);
        }

        readonly string[] _filesToCopy = { "{0}.csproj.user", "{0}.tss", "{0}.suo", "{0}.v12.suo" };

        public void LoadSolutionItemFiles(SolutionItem item)
        {
            var itemFolder = GetSolutionItemFolder(item);
            if (!Directory.Exists(itemFolder))
                return;

            for (var i = 0; i < _filesToCopy.Length; i++)
            {
                var fileToCopy = string.Format(_filesToCopy[i], SolutionName);
                FileService.TryFileCopy(Path.Combine(itemFolder, fileToCopy), Path.Combine(SolutionPath, fileToCopy));
            }
        }
        public void SaveSolutionItemFiles(SolutionItem item)
        {
            var itemFolder = GetSolutionItemFolder(item);
            if (!FileService.TryDirectoryDelete(itemFolder))
                return;

            if (!FileService.TryDirectoryCreate(itemFolder))
                return;

            for (var i = 0; i < _filesToCopy.Length; i++)
            {
                var fileToCopy = string.Format(_filesToCopy[i], SolutionName);
                FileService.TryFileCopy(Path.Combine(SolutionPath, fileToCopy), Path.Combine(itemFolder, fileToCopy));
            }
        }

        public void DeleteSolutionItemFiles(SolutionItem item)
        {
            FileService.TryDirectoryDelete(GetSolutionItemFolder(item));
        }
    }
}
