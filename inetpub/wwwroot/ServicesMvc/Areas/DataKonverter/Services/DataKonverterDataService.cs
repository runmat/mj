using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using Microsoft.VisualBasic.FileIO;
using SapORM.Contracts;
using ServicesMvc.Areas.DataKonverter.Models;
using LumenWorks.Framework.IO.Csv;
using FieldType = ServicesMvc.Areas.DataKonverter.Models.FieldType;

namespace CkgDomainLogic.DataKonverter.Services
{
    public class DataKonverterDataService : CkgGeneralDataServiceSAP, IDataKonverterDataService
    {
        public DataKonverterDataService(ISapDataService sap)
            : base(sap)
        {
        }

        /// <summary>
        /// Gibt SourceFile-Objekt zurück
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="firstRowIsCaption"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public SourceFile FillSourceFile(string filename, bool firstRowIsCaption, char delimiter = ';')
        {            
            var csvObj = CsvReaderFactory.GetCsvObj(filename, firstRowIsCaption, delimiter);

            var fieldCount = csvObj.FieldCount;
            var headers = csvObj.GetFieldHeaders();
            var fields = new List<Field>();

            // Falls keine Überschriften, Spaltennamen selbst erstellen..
            for (var i = 0; i < headers.Length; i++)
            {
                if (!firstRowIsCaption) 
                    headers[i] = "Spalte" + i + 1;

                var newField = new Field
                {
                    Caption = headers[i],
                    FieldType = FieldType.String,  // DefaultType
                    IsUsed = true,
                };

                fields.Add(newField);
            }

            // Daten jeder Column zuordnen...
            while (csvObj.ReadNextRecord())
            {
                for (var j = 0; j < fieldCount; j++)
                {
                    var value = csvObj[j];
                    fields[j].Records.Add(value);
                }
            }

            var sourceFile = new SourceFile
            {
                Filename = filename,
                FirstRowIsCaption = firstRowIsCaption,
                Fields = fields
            };
            
            // File.Delete(filename);
            return sourceFile;
        }

        protected DataItem.DataType GetDataType(IEnumerable<string> values)
        {
            var dataType = DataItem.DataType.String;

            return dataType;
        }
    }
}