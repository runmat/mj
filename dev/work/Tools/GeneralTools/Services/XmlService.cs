using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GeneralTools.Services
{
    public class XmlService
    {
        static string GetXmlFileName(string xmlDirectoryName, string fileNameWithoutDirectory)
        {
            return Path.Combine(xmlDirectoryName, string.Format("{0}.xml", fileNameWithoutDirectory));
        }

        #region Xml Write

        public static void XmlTryDeleteFile(string xmlDirectoryName, string fileNameWithoutDirectory) 
        {
            var xmlFileName = GetXmlFileName(xmlDirectoryName, fileNameWithoutDirectory);

            FileService.TryFileDelete(xmlFileName);
        }

        public static void XmlTryDeleteFile<T>(string xmlDirectoryName) where T : class
        {
            XmlTryDeleteFile(xmlDirectoryName, typeof(T).Name);
        }

        public static void XmlSerializeToPath(object data, string xmlDirectoryName, string fileNameWithoutDirectory, bool acceptException = false) 
        {
            var xmlFileName = GetXmlFileName(xmlDirectoryName, fileNameWithoutDirectory);

            XmlSerializeToFile(data, xmlFileName, acceptException);
        }

        public static void XmlSerializeToPath(object data, string xmlDirectoryName, bool acceptException = false) 
        {
            XmlSerializeToPath(data, xmlDirectoryName, data.GetType().Name, acceptException);
        }

        public static void XmlSerializeToFile(object data, string xmlFileName, bool acceptException = false) 
        {
            try
            {
                FileService.TryFileDelete(xmlFileName);

                XmlSerialize(data, new StreamWriter(xmlFileName));
            }
            catch 
            {
                if (!acceptException)
                    throw;
            }
        }

        public static void XmlTrySerializeToPath(object data, string xmlDirectoryName, string fileNameWithoutDirectory) 
        {
            var xmlFileName = GetXmlFileName(xmlDirectoryName, fileNameWithoutDirectory);

            XmlSerializeToFile(data, xmlFileName, true);
        }

        public static void XmlTrySerializeToPath(object data, string xmlDirectoryName) 
        {
            XmlSerializeToPath(data, xmlDirectoryName, data.GetType().Name, true);
        }

        public static string XmlSerializeToString(object data) 
        {
            var ms = new MemoryStream();
            XmlSerialize(data, new StreamWriter(ms));
            ms.Close();

            return Encoding.UTF8.GetString(ms.GetBuffer()).Replace("\0","");
        }

        static void XmlSerialize(object data, TextWriter writer) 
        {
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(writer, data);
            writer.Close();
        }

        #region Raw

        public static string XmlSerializeRawBulkToString<T>(T data) where T : class
        {
            return XmlSerializeRawBulkToString(data, typeof(T));
        }

        public static string XmlSerializeRawBulkToString(object data, Type type) 
        {
            var ms = new MemoryStream();
            XmlSerializeRawBulk(data, new StreamWriter(ms), type);
            ms.Close();

            var s = Encoding.UTF8.GetString(ms.GetBuffer()).Replace("\0", "");
            s = s.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n", "");
            return s;
        }

        static void XmlSerializeRawBulk(object data, TextWriter writer, Type type) 
        {
            var serializer = new XmlSerializer(type);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, data, namespaces);
            writer.Close();
        }

        #endregion

        #endregion


        #region Xml Read

        public static T XmlDeserializeFromPath<T>(string xmlDirectoryName, string fileNameWithoutDirectory, bool acceptException = false) where T : class, new()
        {
            var xmlFileName = GetXmlFileName(xmlDirectoryName, fileNameWithoutDirectory);

            return XmlDeserializeFromFile<T>(xmlFileName, acceptException);
        }

        public static T XmlDeserializeFromPath<T>(string xmlDirectoryName, bool acceptException = false) where T : class, new()
        {
            return XmlDeserializeFromPath<T>(xmlDirectoryName, typeof(T).Name, acceptException);
        }

        public static T XmlDeserializeFromFile<T>(string xmlFileName, bool acceptException = false) where T : class, new()
        {
            if (!File.Exists(xmlFileName))
            {
                var ctorInfo = typeof (T).GetConstructor(new Type[] {});
                if (ctorInfo != null)
                    return (T) ctorInfo.Invoke(new object[] { });
                return null;
            }

            T data;

            try
            {
                data = (T) XmlDeserialize(new StreamReader(xmlFileName), typeof (T));
            }
            catch
            {
                data = null;
                if (!acceptException)
                    throw;
            }

            return data;
        }

        public static T XmlTryDeserializeFromFile<T>(string xmlFileName) 
        {
            if (!File.Exists(xmlFileName))
                return default(T);

            T data;

            try
            {
                data = (T)XmlDeserialize(new StreamReader(xmlFileName), typeof(T));
            }
            catch
            {
                return default(T);
            }

            return data;
        }      
  
        public static T XmlTryDeserializeFromPath<T>(string xmlDirectoryName, string fileNameWithoutDirectory) where T : class, new()
        {
            var xmlFileName = GetXmlFileName(xmlDirectoryName, fileNameWithoutDirectory);

            return XmlDeserializeFromFile<T>(xmlFileName, true);
        }

        public static T XmlTryDeserializeFromPath<T>(string xmlDirectoryName) where T : class, new()
        {
            return XmlDeserializeFromPath<T>(xmlDirectoryName, typeof(T).Name, true);
        }
        
        public static object XmlDeserializeFromString(string xmlString, Type type) 
        {
            if (string.IsNullOrEmpty(xmlString))
                return null;

            return XmlDeserialize(new StringReader(xmlString), type);
        }

        public static T XmlTryDeserializeFromString<T>(string xmlString) where T : class, new ()
        {
            T t;
            try { t = (T) XmlDeserializeFromString(xmlString, typeof (T)); }
            catch { t = new T();}

            return t;
        }

        public static T XmlDeserializeFromString<T>(string xmlString) where T : class
        {
            return (T)XmlDeserializeFromString(xmlString, typeof(T));
        }

        static object XmlDeserialize(TextReader reader, Type type) 
        {
            var serializer = new XmlSerializer(type);
            var t = serializer.Deserialize(reader);
            reader.Close();

            return t;
        }

        public static bool BulkReadXmlItems<T>(string bulkXmlFileName, Func<T, bool> processOneItem) 
            where T : class
        {
            var atLeastOneItemProcessedFailed = false;

            using (var fs = File.OpenRead(bulkXmlFileName))
            {
                var context = new XmlParserContext(new NameTable(), null, null, XmlSpace.Default);
                var reader = new XmlTextReader(fs, XmlNodeType.Element, context);

                try
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == typeof(T).Name)
                        {
                            var s = reader.ReadOuterXml();
                            var storedItem = XmlDeserializeFromString<T>(s);

                            if (!processOneItem(storedItem))
                                atLeastOneItemProcessedFailed = true;
                        }
                    }
                }
                catch (XmlException e)
                {
                    var eMessage = e.Message.ToLower();
                    if (!eMessage.StartsWith("unexpected end of file has occurred") && !eMessage.StartsWith("unerwartetes dateiende"))
                        throw;
                }
            }

            return atLeastOneItemProcessedFailed;
        }

        public static bool BulkWriteXmlItem<T>(string bulkXmlFileName, T item) where T : class
        {
            if (!File.Exists(bulkXmlFileName))
            {
                using (var fNew = new StreamWriter(new FileStream(bulkXmlFileName, FileMode.CreateNew)))
                {
                    fNew.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    fNew.WriteLine("<LogItems>");
                    fNew.Close();
                }
            }
            using (var fs = File.Open(bulkXmlFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                var writer = new XmlTextWriter(fs, Encoding.UTF8);

                var s = XmlSerializeRawBulkToString(item);
                writer.WriteRaw(s.ToCharArray(), 0, s.Length);
                writer.WriteWhitespace("\n");

                writer.Close();
            }

            return true;
        }

        #endregion 
    }
}
