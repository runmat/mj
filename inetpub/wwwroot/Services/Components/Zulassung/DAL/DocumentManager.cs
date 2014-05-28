using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ICSharpCode.SharpZipLib.Zip;

namespace CKG.Components.Zulassung.DAL
{
    public sealed class DocumentProperties
    {
        public string FileName { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Identifier { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public sealed class DocumentManager
    {
        private static readonly System.Text.RegularExpressions.Regex InfoExtractor = new System.Text.RegularExpressions.Regex(@"^(?<country>\w+)_(?<type>\w+)_(?<identifier>\w+)_(?<date>\d{8})$", System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
        private readonly string managedDirectory;
        private readonly IEnumerable<DocumentProperties> files;

        public DocumentManager(string managedDirectory)
        {
            this.managedDirectory = managedDirectory;

            this.files = (from f in Directory.GetFiles(this.managedDirectory)
                          let name = Path.GetFileNameWithoutExtension(f)
                          let res = DocumentManager.InfoExtractor.Match(name)
                          where res.Success
                          select new DocumentProperties()
                          {
                              FileName = Path.GetFileName(f),
                              Country = res.Groups["country"].Value,
                              Type = res.Groups["type"].Value,
                              Identifier = res.Groups["identifier"].Value,
                              ValidUntil = DateTime.ParseExact(res.Groups["date"].Value, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture)
                          }).ToArray();
        }

        public IEnumerable<IGrouping<string, DocumentProperties>> GetDocumentsForCountry(string country)
        {
            return from f in this.files
                   where f.Country.Equals(country, StringComparison.OrdinalIgnoreCase)
                   group f by f.Type;
        }

        private IEnumerable<string> ValidateFiles(IEnumerable<string> fileNames)
        {
            return fileNames.Intersect(this.files.Select(f => f.FileName));
        }

        public void ValidateAndDownload(IEnumerable<string> fileNames, string zipName, System.Web.HttpResponseBase response)
        {
            var buffer = new byte[4096];
            fileNames = from f in this.ValidateFiles(fileNames)
                        select Path.Combine(this.managedDirectory, f);

            response.Clear();
            response.ContentType = "application/zip";
            response.AppendHeader("content-disposition", String.Format("attachment; filename=\"{0}\"", zipName));
            response.CacheControl = "Private";
            response.Cache.SetExpires(DateTime.Now);

            using (var zipOutputStream = new ZipOutputStream(response.OutputStream))
            {
                zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

                foreach (var fileName in fileNames)
                {
                    using (var fs = File.OpenRead(fileName))
                    {
                        var entry = new ZipEntry(ZipEntry.CleanName(fileName));
                        entry.Size = fs.Length;
                        // Setting the Size provides WinXP built-in extractor compatibility,
                        //  but if not available, you can set zipOutputStream.UseZip64 = UseZip64.Off instead.

                        zipOutputStream.PutNextEntry(entry);

                        var count = fs.Read(buffer, 0, buffer.Length);

                        while (count > 0)
                        {
                            zipOutputStream.Write(buffer, 0, count);
                            count = fs.Read(buffer, 0, buffer.Length);

                            if (!response.IsClientConnected)
                            {
                                break;
                            }

                            response.Flush();
                        }
                    }
                }
            }

            response.Flush();
            response.End();
        }
    }
}