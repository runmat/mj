﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace SapORM.Services
{
    public class FunctionReflector
    {
        public static ISapDataService DataService { get; set; }

        private static ISapConnection SapConnection { get { return DataService.SapConnection; } }

        private static IDynSapProxyFactory DynFactory { get { return DataService.DynSapProxyFactory; } }

        public static void WriteOrmForSapFunction(string sapFunction)
        {
            sapFunction = sapFunction.NotNullOrEmpty().ToUpper();

            var sapProxy = DynFactory.CreateProxyCache(sapFunction, SapConnection, DynFactory).GetProxy();

            var bapiStructure = sapProxy.GetBapiStructure();

            var exportPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\..\models\Models\SAP\AutoGenerated";
            var exportFileName = Path.Combine(exportPath, sapFunction + ".cs");
            var sw = FileHelper.CreateTextFile(exportFileName);
            if (sw == null) return;

            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.Data;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using System.Web.Script.Serialization;");
            sw.WriteLine("using GeneralTools.Contracts;");
            sw.WriteLine("using GeneralTools.Models;");
            sw.WriteLine("using SapORM.Contracts;");
            sw.WriteLine();

            sw.WriteLine("namespace SapORM.Models");
            sw.WriteLine("{");
            sw.WriteLine("\tpublic partial class {0}", sapFunction);
            sw.WriteLine("\t{");

            sw.WriteLine("\t\tpublic static void Init(ISapDataService sap)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tsap.Init(typeof({0}).Name);", sapFunction);
            sw.WriteLine("\t\t}");
            sw.WriteLine();

            sw.WriteLine("\t\tpublic static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tsap.Init(typeof({0}).Name, inputParameterKeys, inputParameterValues);", sapFunction);
            sw.WriteLine("\t\t}");
            sw.WriteLine();

            foreach (var item in bapiStructure.ImportParameters)
            {
                WriteOrmForParameter(sw, item, true, false);
            }

            foreach (var item in bapiStructure.ExportParameters)
            {
                WriteOrmForParameter(sw, item, false, true);
            }

            foreach (var item in bapiStructure.Parameters)
            {
                WriteOrmForParameter(sw, item, true, true);
            }

            foreach (var item in bapiStructure.ImportTables)
            {
                WriteOrmForTable(sw, sapFunction, item.Key, item.Value, true, false);
            }

            foreach (var item in bapiStructure.ExportTables)
            {
                WriteOrmForTable(sw, sapFunction, item.Key, item.Value, false, true);
            }

            foreach (var item in bapiStructure.Tables)
            {
                WriteOrmForTable(sw, sapFunction, item.Key, item.Value, true, true);
            }
            sw.WriteLine("\t}");

            sw.WriteLine();
            sw.WriteLine("\tpublic static partial class DataTableExtensions");
            sw.WriteLine("\t{");
            foreach (var item in bapiStructure.ImportTables.Keys)
            {
                WriteOrmForTableExtensionMethods(sw, sapFunction, item);
            }
            foreach (var item in bapiStructure.ExportTables.Keys)
            {
                WriteOrmForTableExtensionMethods(sw, sapFunction, item);
            }
            foreach (var item in bapiStructure.Tables.Keys)
            {
                WriteOrmForTableExtensionMethods(sw, sapFunction, item);
            }
            sw.WriteLine("\t}");

            sw.WriteLine("}");
            sw.Close();

            AddModelToProjectFile(sapFunction);

            ExportAllModelsToExcel(exportPath);
        }

        private static void WriteOrmForParameter(StreamWriter sw, BapiField item, bool isImport, bool isExport)
        {
            if (isImport)
            {
                sw.WriteLine();
                sw.WriteLine("\t\tpublic static void SetImportParameter_{0}(ISapDataService sap, {1} value)", item.Name, item.TypeAsString);
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tsap.SetImportParameter(\"{0}\", value);", item.Name);
                sw.WriteLine("\t\t}");
            }

            if (isExport)
            {
                sw.WriteLine();
                sw.WriteLine("\t\tpublic static {0} GetExportParameter_{1}(ISapDataService sap)", item.TypeAsString, item.Name);
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\treturn sap.GetExportParameter<{0}>(\"{1}\"){2};", item.TypeAsString, item.Name, (item.TypeAsString == "string" ? ".NotNullOrEmpty().Trim()" : ""));
                sw.WriteLine("\t\t}");
            }
        }

        private static void WriteOrmForTable(StreamWriter sw, string sapFunction, string tableName, List<BapiField> tableColumns, bool isImport, bool isExport)
        {
            sw.WriteLine();
            sw.WriteLine("\t\tpublic partial class {0} : IModelMappingApplied", tableName);
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\t[SapIgnore]");
            sw.WriteLine("\t\t\t[ScriptIgnore]");
            sw.WriteLine("\t\t\tpublic ISapConnection SAPConnection { get; set; }");
            sw.WriteLine();
            sw.WriteLine("\t\t\t[SapIgnore]");
            sw.WriteLine("\t\t\t[ScriptIgnore]");
            sw.WriteLine("\t\t\tpublic IDynSapProxyFactory DynSapProxyFactory { get; set; }");
            sw.WriteLine();

            foreach (var col in tableColumns)
            {
                sw.WriteLine("\t\t\tpublic {0} {1} {{ get; set; }}", col.TypeAsString, col.Name);
                sw.WriteLine();
            }

            sw.WriteLine("\t\t\tpublic static {0} Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)", tableName);
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tvar o = new {0}", tableName);
            sw.WriteLine("\t\t\t\t{");

            foreach (var col in tableColumns)
            {
                if (col.IsNullable)
                    sw.WriteLine("\t\t\t\t\t{0} = string.IsNullOrEmpty(row[\"{0}\"].ToString()) ? null : ({1})row[\"{0}\"],", col.Name, col.TypeAsString);
                else
                    sw.WriteLine("\t\t\t\t\t{0} = ({1})row[\"{0}\"],", col.Name, col.TypeAsString);
            }

            sw.WriteLine();
            sw.WriteLine("\t\t\t\t\tSAPConnection = sapConnection,");
            sw.WriteLine("\t\t\t\t\tDynSapProxyFactory = dynSapProxyFactory,");

            sw.WriteLine("\t\t\t\t};");
            sw.WriteLine("\t\t\t\to.OnInitFromSap();");
            sw.WriteLine("\t\t\t\treturn o;");
            sw.WriteLine("\t\t\t}");

            sw.WriteLine();
            sw.WriteLine("\t\t\tpartial void OnInitFromSap();");

            sw.WriteLine();
            sw.WriteLine("\t\t\tpartial void OnInitFromExtern();");

            sw.WriteLine();
            sw.WriteLine("\t\t\tpublic void OnModelMappingApplied()");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tOnInitFromExtern();");
            sw.WriteLine("\t\t\t}");

            sw.WriteLine();
            sw.WriteLine("\t\t\tpublic static IEnumerable<{0}> Select(DataTable dt, ISapConnection sapConnection = null)", tableName);
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\treturn dt.AsEnumerable().Select(r => Create(r, sapConnection));");
            sw.WriteLine("\t\t\t}");

            if (isExport)
            {
                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> ToList(DataTable dt, ISapConnection sapConnection = null)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn Select(dt, sapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");
            }

            sw.WriteLine();
            sw.WriteLine("\t\t\tpublic static IEnumerable<{0}> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)", tableName);
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tvar tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof({0}).Name.ToLower());", tableName);
            sw.WriteLine("\t\t\t\tif (tbl == null)");
            sw.WriteLine("\t\t\t\t\treturn null;");
            sw.WriteLine();
            sw.WriteLine("\t\t\t\treturn Select(tbl, sapConnection);");
            sw.WriteLine("\t\t\t}");

            if (isExport)
            {
                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");

                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> ToList(ISapDataService sapDataService)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);");
                sw.WriteLine("\t\t\t}");

                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\tif (sapDataService == null) ");
                sw.WriteLine("\t\t\t\t\treturn new List<{0}>();", tableName);
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\tvar dts = sapDataService.GetExportTablesWithInitExecute(\"" + sapFunction + "\", inputParameterKeys, inputParameterValues);");
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapDataService.SapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");

                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> GetExportListWithExecute(ISapDataService sapDataService)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\tif (sapDataService == null) ");
                sw.WriteLine("\t\t\t\t\treturn new List<{0}>();", tableName);
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\tvar dts = sapDataService.GetExportTablesWithExecute();");
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapDataService.SapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");

                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> GetExportList(ISapDataService sapDataService)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\tif (sapDataService == null) ");
                sw.WriteLine("\t\t\t\t\treturn new List<{0}>();", tableName);
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\tvar dts = sapDataService.GetExportTables();");
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapDataService.SapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");
            }

            if (isImport)
            {
                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\tif (sapDataService == null) ");
                sw.WriteLine("\t\t\t\t\treturn new List<{0}>();", tableName);
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\tvar dts = sapDataService.GetImportTablesWithInit(\"" + sapFunction + "\", inputParameterKeys, inputParameterValues);");
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapDataService.SapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");

                sw.WriteLine();
                sw.WriteLine("\t\t\tpublic static List<{0}> GetImportList(ISapDataService sapDataService)", tableName);
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\tif (sapDataService == null) ");
                sw.WriteLine("\t\t\t\t\treturn new List<{0}>();", tableName);
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\tvar dts = sapDataService.GetImportTables();");
                sw.WriteLine("\t\t\t\t ");
                sw.WriteLine("\t\t\t\treturn Select(dts, sapDataService.SapConnection).ToListOrEmptyList();");
                sw.WriteLine("\t\t\t}");
            }

            sw.WriteLine("\t\t}");
        }

        private static void WriteOrmForTableExtensionMethods(StreamWriter sw, string sapFunction, string tableName)
        {
            sw.WriteLine();
            sw.WriteLine("\t\tpublic static DataTable ToTable(this IEnumerable<{0}.{1}> list)", sapFunction, tableName);
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\treturn SapDataServiceExtensions.ToTable(list);");
            sw.WriteLine("\t\t}");
            sw.WriteLine();
        }

        private static void AddModelToProjectFile(string sapFunction)
        {
            var modelIncludeName = string.Format(@"Models\SAP\AutoGenerated\{0}.cs", sapFunction);

            var csprojPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\..\models\SapORM.Models.csproj";
            var csproj = XDocument.Load(csprojPath);
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var itemGroup = csproj.Descendants(msbuild + "ItemGroup").FirstOrDefault(x => x.Descendants(msbuild + "Compile").Any());
            if (itemGroup != null && itemGroup.Descendants().None(x => x.Attributes().Any(a => a.Name == "Include" && a.Value == modelIncludeName)))
            {
                var item = new XElement(msbuild + "Compile");
                item.SetAttributeValue("Include", modelIncludeName);
                itemGroup.Add(item);
                csproj.Save(csprojPath);
            }
        }

        private static void ExportAllModelsToExcel(string sourcePath)
        {
            var destinationPath = @"\\AHS018\Abteilung\IT\IT-Entwicklung\SAP";
            var destinationFile = Path.Combine(destinationPath, "SAP-Bapis.csv");
            if (!FileService.TryFileDelete(destinationFile))
                return;

            var sw = FileHelper.CreateTextFile(destinationFile);
            Directory.GetFiles(sourcePath, "*.cs").Select(s => Path.GetFileName(s).NotNullOrEmpty().Replace(".cs", "")).OrderBy(s => s).ToList().ForEach(sw.WriteLine);
            sw.Close();
        }
    }
}
