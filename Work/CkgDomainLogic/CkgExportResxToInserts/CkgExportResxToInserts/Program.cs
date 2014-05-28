using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace CkgExportResxToInserts
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            const string insert = "INSERT INTO [dbo].[TranslatedResource] ([Resource], [Format], [en], [de], [de_de], [de_at], [de_ch]) VALUES (N'{0}', NULL, N'{1}', N'{2}', N'{3}', N'{4}', N'{5}');";

            StringBuilder sb = new StringBuilder();
            var invariantResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);
            var deutschResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo("de-de"), true, true);
            foreach (DictionaryEntry invariantEntry in invariantResourceSet)
            {
                var s = deutschResourceSet.GetString(invariantEntry.Key.ToString()); // de un de-de Wert
                if (string.IsNullOrEmpty(s))
                {
                    sb.AppendLine(string.Format(insert, invariantEntry.Key, invariantEntry.Value.ToString().Replace("'", "''"), "", "", "", ""));
                    continue;
                }

                sb.AppendLine(string.Format(insert, invariantEntry.Key, invariantEntry.Value.ToString().Replace("'", "''"), s.Replace("'", "''"), "", "", ""));
            }

            var outputFileName = rootPath + @"\CkgExportResxToInserts.sql";
            if (File.Exists(outputFileName))
                File.SetAttributes(outputFileName, FileAttributes.Normal);

            File.WriteAllText(outputFileName, sb.ToString(), Encoding.UTF8);

        }
    }
}
