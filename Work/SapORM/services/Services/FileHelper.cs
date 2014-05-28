using System.IO;

namespace SapORM.Services
{
    public class FileHelper
    {
        static public StreamWriter CreateTextFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                return File.CreateText(fileName);
            }
            catch
            {
                return null;
            }
        }
    }
}
