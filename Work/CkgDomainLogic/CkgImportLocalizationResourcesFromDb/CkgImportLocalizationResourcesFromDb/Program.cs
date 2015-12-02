using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GeneralTools.Services;

namespace CkgImportLocalizationResourcesFromDb
{
    class Program
    {
        static readonly string SourceConnectionString = ConfigurationManager.AppSettings["DadTest"];

        static bool ConnectionStringsAreDifferent(string connString1, string connString2)
        {
            var conn1 = new SqlConnection(connString1);
            var conn2 = new SqlConnection(connString2);
            if (conn1.DataSource == null || conn2.DataSource == null)
                return true;

            return !(conn1.DataSource.ToLower() == conn2.DataSource.ToLower() && conn1.Database.ToLower() == conn2.Database.ToLower());
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                GenerateTranslationCsharpCode();
                return;
            }

            var connectionStringDict = GeneralConfiguration.GetConfigAllServersValues("ConnectionString", SourceConnectionString)
                .Where(ce => ConnectionStringsAreDifferent(ce.Value, SourceConnectionString));
            foreach (var destinationConnStringEntry in connectionStringDict)
            {
                var destinationConnectionString = destinationConnStringEntry.Value;

                Console.WriteLine(@"*** Connection '{0}' ***", destinationConnStringEntry.Key);

                CopyDataTable(SourceConnectionString, destinationConnectionString, "TranslatedResource");
                CopyDataTable(SourceConnectionString, destinationConnectionString, "TranslatedResourceCustom");
                CopyDataTable(SourceConnectionString, destinationConnectionString, "ConfigAllServers");
                SetTimeOfLastResourceUpdate(destinationConnectionString);

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void CopyDataTable(string sourceConnString, string destinationConnString, string tableName)
        {
            if (destinationConnString.ToLower() == sourceConnString.ToLower())
                return;

            var sourceDbContext = new DomainDbContext(sourceConnString);
            var destinationDbContext = new DomainDbContext(destinationConnString); 

            Console.WriteLine(@"Tabelle '{0}' wird kopiert ...", tableName);

            destinationDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            destinationDbContext.SaveChanges();

            if (tableName.ToLower() == "translatedresource")
                sourceDbContext.TranslatedResources.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResources.Add(ModelMapping.Copy(sourceResource)));
            else if (tableName.ToLower() == "translatedresourcecustom")
                sourceDbContext.TranslatedResourcesCustom.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResourcesCustom.Add(ModelMapping.Copy(sourceResource)));
            else if (tableName.ToLower() == "configallservers")
                sourceDbContext.ConfigsAllServers.ToList().ForEach(sourceResource => destinationDbContext.ConfigsAllServers.Add(ModelMapping.Copy(sourceResource)));

            destinationDbContext.SaveChanges();
        }

        private static void SetTimeOfLastResourceUpdate(string destinationConnString)
        {
            var destinationDbContext = new DomainDbContext(destinationConnString);

            Console.WriteLine(@"Setze 'TimeOfLastResourceUpdate' in Tabelle 'Config'");

            destinationDbContext.Database.ExecuteSqlCommand("update Config set [Value] = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where [Context] = 'Localization' and [Key] = 'TimeOfLastResourceUpdate'");
            destinationDbContext.SaveChanges();
        }

        private static void GenerateTranslationCsharpCode()
        {
            var domainDbContext = new DomainDbContext(SourceConnectionString);
            var q = from translatedresource in domainDbContext.TranslatedResources 
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
