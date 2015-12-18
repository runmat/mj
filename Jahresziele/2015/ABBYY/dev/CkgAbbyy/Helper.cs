using System;
using System.IO;
using System.Linq;
using System.Xml;
using GeneralTools.Models;

namespace CkgAbbyy
{
    public class Helper
    {
        public static bool KeyIsValid(string xmlFileName, string key, Func<string, string, bool> additionalCompareFunc = null)
        {
            try
            {
                string[] keys = { key };
                if (key.Contains(";"))
                    keys = key.Split(';');

                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFileName);
                if (xmlDocument.DocumentElement == null)
                    return false;

                var keyValue = "";
                foreach (var k in keys)
                {
                    var keyObjects = xmlDocument.DocumentElement.ChildNodes[0].ChildNodes.OfType<XmlNode>().ToList();
                    var keyObject = keyObjects.FirstOrDefault(e => e.Name == k);
                    if (keyObject == null || keyObject.LastChild == null)
                        continue;
                    if (keyObject.LastChild.Value == null)
                        continue;

                    keyValue = keyObject.LastChild.Value;

                    if (keyValue.IsNotNullOrEmpty())
                        break;
                }
                if (keyValue.IsNullOrEmpty())
                    return false;

                var nakedFileName = Path.GetFileNameWithoutExtension(xmlFileName);

                keyValue = ReplaceNoideChars(keyValue).ToLower();
                nakedFileName = ReplaceNoideChars(nakedFileName).ToLower();

                return KeyValueMatchesFileNames(new[] { nakedFileName }, keyValue, additionalCompareFunc);
            }
            catch { return false; }
        }

        static readonly string[] Separators = { "/", "-", "." };

        public static string ReplaceNoideChars(string text)
        {
            foreach (var t in Separators)
                text = text.Replace(t, "");

            text = text.Replace(" ", "");

            return text;
        }

        public static bool KeyValueMatchesFileNames(string[] azFileNames, string keyValue, Func<string, string, bool>additionalCompareFunc = null)
        {
            foreach (var azFileName in azFileNames)
            {
                var az = azFileName.ToUpper();
                az = az.Substring(az.IndexOf("_", StringComparison.Ordinal) + 1);
                az = az.Replace(".JPG", "");

                foreach (var t in Separators)
                {
                    var azProcessed = az.Replace("-", t).ToLower();
                    if (azProcessed == keyValue)
                        return true;
                    if (additionalCompareFunc != null && additionalCompareFunc(azProcessed, keyValue))
                        return true;
                }
            }

            return false;
        }
    }
}
