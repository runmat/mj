using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MvcTools.Imaging
{
    static public class DbImageFileCacheService
    {
        static public void SetImageBytesAndCacheToImageFile<T>(this T o, string propertyName, object value)
        {
            if (value != null && false == (value is byte[]))
                throw new NotSupportedException("SetImageBytesAndCacheToImageFile needs type byte[] as setter value");

            var type = o.GetType();

            var keyProperty = type.GetProperties().ToList().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());
            if (keyProperty == null)
                throw new NotSupportedException(string.Format("Type {0} needs at least one property with a [Key] attribute", type.Name));

            var propertyForTempFileName = type.GetProperty(string.Format("{0}TempFileName", propertyName));
            if (propertyForTempFileName == null || propertyForTempFileName.PropertyType != typeof(string))
                throw new NotSupportedException(string.Format("Type {0} needs one property '{1}TempFileName' of type string", type.Name, propertyName));

            var id = keyProperty.GetValue(o, null) + "__" + Guid.NewGuid();

            var defaultImage = Imaging.CreateEmptyHintImage("kein Bild", "606060", 200, 25); 
            var imageBytes = value as byte[] == null ? defaultImage : Imaging.ScaleImage((value as byte[]).ToArray(), 200, 200, null);

            var image = Imaging.ImageFromBytes(imageBytes);
            var imageFileExtension = Imaging.GetExtensionByImageFormat(image.RawFormat).ToLower();
            var imageFileName = string.Format("{0}.{1}", id, imageFileExtension);

            string applicationRootFolder;
            if (HttpContext.Current != null)
                applicationRootFolder = HttpContext.Current.Server.MapPath("~/");
            else
                applicationRootFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\..\..";

            var imageSubFolder = string.Format(@"Images\Temp\{0}", type.Name);
            var totalFileFolder = Path.Combine(applicationRootFolder, imageSubFolder);
            if (!Directory.Exists(totalFileFolder))
                Directory.CreateDirectory(totalFileFolder);
            var totalImageFileName = Path.Combine(totalFileFolder, imageFileName);
            var webImageFileName = string.Format("/{0}/{1}", imageSubFolder.Replace("\\", "/"), imageFileName);

            var outFile = totalImageFileName;
            var outFileStream = new FileStream(outFile, FileMode.OpenOrCreate);
            outFileStream.Write(imageBytes, 0, imageBytes.Length);
            outFileStream.Close();


            // invoke the setter of our property for the Image Bytes:
            type.GetProperty(propertyName).GetSetMethod(true).Invoke(o, new[] { value });

            // invoke the setter of our property for the Image Temp File Name:
            type.GetProperty(propertyForTempFileName.Name).GetSetMethod(true).Invoke(o, new[] { webImageFileName });
        }

        static public string GetImageTempFileName<T>(this T o, string propertyName)
        {
            var type = o.GetType();

            var propertyForTempFileName = type.GetProperty(string.Format("{0}TempFileName", propertyName));
            if (propertyForTempFileName == null || propertyForTempFileName.PropertyType != typeof(string))
                throw new NotSupportedException(string.Format("Type {0} needs one property '{1}TempFileName' of type string", type.Name, propertyName));

            // invoke the getter of our property for the Image Temp File Name:
            return (string)type.GetProperty(propertyForTempFileName.Name).GetGetMethod(true).Invoke(o, new object[] { });
        }
    }
}
