using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using GeneralTools.Services;
using GeneralTools.Models;

namespace DocumentTools.Services
{
    public abstract class AbstractDocumentFactory
    {
        protected AbstractDocumentFactory()
        {
            var tempPath = ConfigurationManager.AppSettings["TempPDFPath"];

            if (tempPath.IsNullOrEmpty())
                if (HttpContext.Current != null)
                    tempPath = HttpContext.Current.Server.MapPath("~/");
                else
                    tempPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "";

            ProvideTempFileForResource("DocumentTools.Aspose.Total.lic",
                                                        tmpFileName =>
                                                            {
                                                                new Aspose.Cells.License().SetLicense(tmpFileName);
                                                                new Aspose.Words.License().SetLicense(tmpFileName);
                                                                new Aspose.Pdf.License().SetLicense(tmpFileName);
                                                            },
                                                         tempPath);
        }

        private static void ProvideTempFileForResource(string resourceName, Action<string> tempFileAction, string tempFolder = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return;

            if (string.IsNullOrEmpty(tempFolder))
                tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var tmpFileName = Path.Combine(tempFolder, resourceName);

            if (File.Exists(tmpFileName))
                FileService.TryFileDelete(tmpFileName);

            try
            {
                var outFileStream = new FileStream(tmpFileName, FileMode.OpenOrCreate);

                FileService.CopyStream(stream, outFileStream);

                stream.Close();
                outFileStream.Close();
            }
            catch { return; }


            tempFileAction(tmpFileName);


            FileService.TryFileDelete(tmpFileName);
        }
    }
}
