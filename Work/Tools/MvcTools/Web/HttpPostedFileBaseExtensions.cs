using System.IO;
using System.Linq;
using System.Web;
using GeneralTools.Services;

namespace MvcTools.Web
{
    public static class HttpPostedFileBaseExtensions
    {
        public static bool SavePostedFile(this HttpPostedFileBase postedFile, string outputFileName)
        {
            try
            {
                if (postedFile == null || postedFile.ContentLength == 0)
                    return false;

                using (var ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    var array = ms.GetBuffer();
                    var bytes = array.ToArray();
                    if (!FileService.TryFileCopyBytes(bytes, outputFileName))
                        return false;
                }
            }
            catch { return false; }

            return true;
        }

        /// <summary>
        /// Speichert ein Dokument. Falls ein Dokument mit dem gleichen Namen bereits vorhanden ist, wird ein fortlaufender numericher Suffix erzeugt.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static string SavePostedFile(this HttpPostedFileBase postedFile, string folder, string filename, string extention)
        {
            // Sollte eine datei mit dem gleichen Namen vorhanden sein mit einem neuen Namen erstellen
            if (File.Exists(string.Concat(folder, filename, extention)))
            {
                bool vorhanden = true;
                int suffix = 0;
                while ((vorhanden))
                {
                    suffix += 1;
                    if (!File.Exists(string.Concat(folder, filename, "_", suffix.ToString(), extention)))
                    {
                        vorhanden = false;
                    }
                }

                filename = string.Concat(filename, "_", suffix.ToString());
            }

            var filenamewithExtention = string.Concat(filename, extention);

            postedFile.SaveAs(string.Concat(folder, filenamewithExtention));
            return filename;
        }

        public static void DeleteExistingAndSavePostedFile(this HttpPostedFileBase postedFile, string folder)
        {
            var fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);
            var fileFullNameWithPath = string.Concat(folder, @"\", fileName);
            if (File.Exists(fileFullNameWithPath))
            {
                File.Delete(fileFullNameWithPath);
            }

            postedFile.SaveAs(fileFullNameWithPath);
        }
    }
}
