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

            var destinationDbServer = (args[0].Length <= 4 ? "Dad" : "") +  args[0].ToUpper();
            CopyTranslationDataTable(destinationDbServer, "TranslatedResource");
            CopyTranslationDataTable(destinationDbServer, "TranslatedResourceCustom");
        }

        private static void CopyTranslationDataTable(string destinationDbServer, string tableName)
        {
            var sourceDbContext = new DomainDbContext(ConfigurationManager.AppSettings["DadTest"]);
            var destinationDbContext = new DomainDbContext(ConfigurationManager.AppSettings[destinationDbServer]); // new DomainDbContext(ConfigurationManager.AppSettings[destinationDbServer == "PROD" ? "ConnectionstringProdSystem" : "ConnectionstringDevSystem"]);

            Console.WriteLine();
            Console.WriteLine("  Tabelle '{0}' wird kopiert, bitte warten ...", tableName);
            Console.WriteLine();

            destinationDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            destinationDbContext.SaveChanges();
            if (tableName.ToLower() == "translatedresource")
                sourceDbContext.TranslatedResources.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResources.Add(ModelMapping.Copy(sourceResource)));
            else
                sourceDbContext.TranslatedResourcesCustom.ToList().ForEach(sourceResource => destinationDbContext.TranslatedResourcesCustom.Add(ModelMapping.Copy(sourceResource)));
            destinationDbContext.SaveChanges();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine();
            Console.WriteLine("  Erfolg!  Tabelle '{0}' kopiert von Server TEST => {1}", tableName, destinationDbServer);
            Console.WriteLine();
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine();
            Thread.Sleep(200);            
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

            File.WriteAllText(outputFileName, sbClass.ToString(), Encoding.UTF8);

            outputFileName = rootPath + @"..\..\..\..\..\..\tools\GeneralTools\Resources\AutoGenerated\LocalizeConstants.cs";
            if (File.Exists(outputFileName))
                File.SetAttributes(outputFileName, FileAttributes.Normal);

            File.WriteAllText(outputFileName, sbConstants.ToString(), Encoding.UTF8);
        }
    }
}
