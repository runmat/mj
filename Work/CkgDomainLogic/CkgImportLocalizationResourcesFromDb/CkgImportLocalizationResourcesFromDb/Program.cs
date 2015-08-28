using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CkgImportLocalizationResourcesFromDb
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                GenerateTranslationCsharpCode();
                return;
            }

            var sourceDbServer = "DadTest";
            foreach (var destinationDbServer in ConfigurationManager.AppSettings.AllKeys)
            {
                Console.WriteLine("*** Connection '{0}' ***", destinationDbServer);

                SetTimeOfLastResourceUpdate(destinationDbServer);
                CopyTranslationDataTable(sourceDbServer, destinationDbServer, "TranslatedResource");
                CopyTranslationDataTable(sourceDbServer, destinationDbServer, "TranslatedResourceCustom");

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void CopyTranslationDataTable(string sourceDbServer, string destinationDbServer, string tableName)
        {
            if (destinationDbServer.ToLower() == sourceDbServer.ToLower())
                return;

            var sourceDbContext = new DomainDbContext(ConfigurationManager.AppSettings[sourceDbServer]);
            var destinationDbContext = new DomainDbContext(ConfigurationManager.AppSettings[destinationDbServer]); // new DomainDbContext(ConfigurationManager.AppSettings[destinationDbServer == "PROD" ? "ConnectionstringProdSystem" : "ConnectionstringDevSystem"]);

            Console.WriteLine("Tabelle '{0}' wird kopiert ...", tableName);

            destinationDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            destinationDbContext.SaveChanges();
            if (tableName.ToLower() == "translatedresource")
                sourceDbContext.TranslatedResources.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResources.Add(ModelMapping.Copy(sourceResource)));
            else
                sourceDbContext.TranslatedResourcesCustom.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResourcesCustom.Add(ModelMapping.Copy(sourceResource)));
            destinationDbContext.SaveChanges();
        }

        private static void SetTimeOfLastResourceUpdate(string destinationDbServer)
        {
            var destinationDbContext = new DomainDbContext(ConfigurationManager.AppSettings[destinationDbServer]);

            Console.WriteLine("Setze 'TimeOfLastResourceUpdate' in Tabelle 'Config'");

            destinationDbContext.Database.ExecuteSqlCommand("update Config set [Value] = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where [Context] = 'Localization' and [Key] = 'TimeOfLastResourceUpdate'");
            destinationDbContext.SaveChanges();
        }

        private static void GenerateTranslationCsharpCode()
        {
            var domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["DadTest"]);
            var q = from translatedresource in domainDbContext.TranslatedResources //domainDbContext.GetResourceSchluessel()
                    select translatedresource;
            const string namespaceDeclaration = "namespace CkgDomainLogic.General.Services {";
            const string classDeclaration = "\tpublic partial class Localize { ";
            const string accessor =
                "\t\t/// <summary>\r\n" +
                "\t\t/// {2}\r\n" +
                "\t\t/// </summary>\r\n" +
                "\t\tpublic static string {0} { get { return TranslateResourceKey(\"{1}\"); }}\r\n";

            const string namespaceDeclarationForConstantsInGeneralTools = "namespace GeneralTools.Resources {";
            const string constantClassDeclaration = "\tpublic class LocalizeConstants { ";
            const string constantMemberDeclaration =
                "\t\t/// <summary>\r\n" +
                "\t\t/// {1}\r\n" +
                "\t\t/// </summary>\r\n" +
                "\t\tpublic const string {0} = \"{0}\";\r\n";

            const string lastline = "\t}\r\n}";

            var sbClass = new StringBuilder();
            var sbConstants = new StringBuilder();

            sbClass.AppendLine(namespaceDeclaration);
            sbClass.AppendLine(classDeclaration);

            sbConstants.AppendLine(namespaceDeclarationForConstantsInGeneralTools);
            sbConstants.AppendLine(constantClassDeclaration);

            q.ToList().ForEach(resourceItem =>
            {
                var suc = accessor.Replace("{0}", resourceItem.Resource);
                suc = suc.Replace("{1}", resourceItem.Resource);
                suc = suc.Replace("{2}", resourceItem.de);
                sbClass.AppendLine(suc);
                var constant = constantMemberDeclaration.Replace("{0}", resourceItem.Resource).Replace("{1}", resourceItem.de);
                sbConstants.AppendLine(constant);
            });

            sbClass.AppendLine(lastline);
            sbConstants.AppendLine(lastline);

            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var outputFileName = rootPath + @"..\..\..\..\..\lib\general\Services\Localization\AutoGenerated\Localize.cs";
            if (File.Exists(outputFileName))
                File.SetAttributes(outputFileName, FileAttributes.Normal);

            EnsureDirectoryExists(outputFileName);

            File.WriteAllText(outputFileName, sbClass.ToString(), Encoding.UTF8);

            outputFileName = rootPath + @"..\..\..\..\..\..\tools\GeneralTools\Resources\AutoGenerated\LocalizeConstants.cs";
            if (File.Exists(outputFileName))
                File.SetAttributes(outputFileName, FileAttributes.Normal);

            EnsureDirectoryExists(outputFileName);

            File.WriteAllText(outputFileName, sbConstants.ToString(), Encoding.UTF8);
        }

        private static void EnsureDirectoryExists(string outputFileName)
        {
            var dirName = Path.GetDirectoryName(outputFileName);
            if (dirName != null && !Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
        }
    }
}
