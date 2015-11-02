using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.General.Services;
using Microsoft.VisualBasic.FileIO;
using SapORM.Contracts;
using ServicesMvc.Areas.DataKonverter.Models;

namespace CkgDomainLogic.DataKonverter.Services
{
    public class DataKonverterDataService : CkgGeneralDataServiceSAP, IDataKonverterDataService
    {
        public DataKonverterDataService(ISapDataService sap)
            : base(sap)
        {
        }

        public SourceFile GetSourceFile(string filename, bool firstRowIsCaption, string delimiter = ";")
        {
            var sourceFile = new SourceFile
            {
                Filename = filename,
                FirstRowIsCaption = firstRowIsCaption
            };

            var encoding = GetFileEncoding(filename, Encoding.UTF8);

            //var sb = new StringBuilder();
            //using (var sr = new StreamReader(filename, encoding, true))
            //{
            //    string line;              
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        sb.AppendLine(line);
            //    }
            //}

            using (var parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                while (!parser.EndOfData)
                {
                    //Processing row
                    var columnTitles = parser.ReadFields();

                    // Falls in erster Zeile keine Überschrift, dann Überschriften selbst benennen...
                    if (!firstRowIsCaption)
                    {
                        var defaultName = "Spalte";
                        var i = 0;
                        foreach (var column in columnTitles)
                        {
                            columnTitles[i] = string.Format("{0}{1}", defaultName, i);
                            i++;
                        }
                    }

                    // Datatype pro Spalte ermitteln...
                    

                    // Daten einlesen...
                    foreach (var field in columnTitles)
                    {
                        //TODO: Process field
                        var test = field;
                    }

                }
            }


            return null;
        }

        protected Encoding GetFileEncoding(string csvFileName, Encoding defaultEncodingIfNoBom)
        {
            using (var reader = new StreamReader(csvFileName, defaultEncodingIfNoBom, true))
            {
                reader.Peek();
                var encoding = reader.CurrentEncoding;
                return encoding;
            }
        }

    }
}