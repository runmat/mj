using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.Serialization;

namespace MvcTools.Data
{
    public class XsdSchemaCreator
    {
        static public void GenerateXsdSchema<T>(string fileName) where T : class, new()
        {
            var exporter = new XsdDataContractExporter();

            using (var writer = new XmlTextWriter(fileName, new UTF8Encoding()))
            {
                writer.Formatting = Formatting.Indented;
                if (exporter.CanExport(typeof(T)))
                {
                    exporter.Export(typeof(T));
                    var mySchemas = exporter.Schemas;

                    var xmlNameValue = exporter.GetRootElementName(typeof(T));
                    if (xmlNameValue == null)
                        return;
                    var employeeNameSpace = xmlNameValue.Namespace;

                    foreach (XmlSchema schema in mySchemas.Schemas(employeeNameSpace))
                    {
                        schema.Write(writer);
                    }
                }
            }
        }
    }
}
