using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace CkgAbbyy
{
    public class Helper
    {
        public static bool KeyIsValid(string xmlFileName, string key)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFileName);
                if (xmlDocument.DocumentElement == null)
                    return false;

                var keyObjects = xmlDocument.DocumentElement.ChildNodes[0].ChildNodes.OfType<XmlNode>().ToList();
                var keyObject = keyObjects.FirstOrDefault(e => e.Name == key);
                if (keyObject == null || keyObject.LastChild == null)
                    return false;

                if (keyObject.LastChild.Value == null)
                    return false;

                var keyValue = keyObject.LastChild.Value;
                var nakedFileName = Path.GetFileNameWithoutExtension(xmlFileName);

                keyValue = ReplaceNoideChars(keyValue).ToLower();
                nakedFileName = ReplaceNoideChars(nakedFileName).ToLower();

                return KeyMatchesFileNames(new[] { nakedFileName }, keyValue);
            }
            catch { return false; }
        }

        static readonly string[] Separators = { "/", "-", "." };

        static string ReplaceNoideChars(string text)
        {
            foreach (var t in Separators)
                text = text.Replace(t, "");

            text = text.Replace(" ", "");

            return text;
        }

        static bool KeyMatchesFileNames(string[] azFileNames, string text)
        {
            foreach (var azFileName in azFileNames)
            {
                var az = azFileName.ToUpper();
                az = az.Substring(az.IndexOf("_", StringComparison.Ordinal) + 1);
                az = az.Replace(".JPG", "");

                foreach (var t in Separators)
                {
                    var azProcessed = az.Replace("-", t).ToLower();
                    if (azProcessed == text)
                        return true;
                }
            }

            return false;
        }
    }
}
