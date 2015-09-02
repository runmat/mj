using System;
using System.IO;
using System.Reflection;

namespace GeneralTools.Services
{
    public class FileService
    {
        public static void ProvideTempFileForResource(string resourceName, Action<string> tempFileAction, string tempFolder = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return;

            if (string.IsNullOrEmpty(tempFolder))
                tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var tmpFileName = Path.Combine(tempFolder, resourceName);

            if (File.Exists(tmpFileName))
                TryFileDelete(tmpFileName);

            try
            {
                var outFileStream = new FileStream(tmpFileName, FileMode.OpenOrCreate);

                CopyStream(stream, outFileStream);

                stream.Close();
                outFileStream.Close();
            }
            catch { return; }


            tempFileAction(tmpFileName);


            TryFileDelete(tmpFileName); 
        }

        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, len);
        }

        public static bool TryFileCopyBytes(byte[] bytes, string destinationFileName)
        {
            var success = true;

            try
            {
                if (!TryFileDelete(destinationFileName))
                    return false;

                using (var outFileStream = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                {
                    outFileStream.Write(bytes, 0, bytes.Length);
                    outFileStream.Close();
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public static bool TryDirectoryCreate(string directoryPath)
        {
            var success = true;
            try
            {
                if (Directory.Exists(directoryPath))
                    return true;

                Directory.CreateDirectory(directoryPath);
            }
            catch { success = false; }

            return success;
        }

        public static string[] TryDirectoryGetFiles(string path, string searchPattern)
        {
            if (!Directory.Exists(path))
                return new string[] {};

            return Directory.GetFiles(path, searchPattern);
        }

        public static bool TryDirectoryDelete(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                return false;

            var success = true;
            try
            {
                if (Directory.Exists(directoryPath))
                    Directory.Delete(directoryPath, true);
            }
            catch { success = false; }

            return success;
        }

        public static bool TryCreateFileWithOverwrite(string filePath)
        {
            var success = true;
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                var sw = File.CreateText(filePath);
                sw.Close();
                File.Delete(filePath);
            }
            catch { success = false; }

            return success;
        }

        public static bool TryFileDelete(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var success = true;
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch { success = false; }

            return success;
        }

        public static bool TryFileCopy(string fileSourcePath, string fileDestinationPath)
        {
            if (string.IsNullOrEmpty(fileSourcePath))
                return false;
            if (string.IsNullOrEmpty(fileDestinationPath))
                return false;

            var success = true;
            try
            {
                File.Copy(fileSourcePath, fileDestinationPath, true);
            }
            catch { success = false; }

            return success;
        }

        public static string PathGetFileName(string fileName)
        {
            if (fileName == null)
                return "";

            return Path.GetFileName(fileName);
        }

        public static bool PathExistsAndWriteEnabled(string path, Action<string> alertAction = null, string pathFriendlyName = null)
        {
            if (!Directory.Exists(path))
                TryDirectoryCreate(path);
            
            if (!Directory.Exists(path))
            {
                if (alertAction != null)
                    alertAction(string.Format("Der von Ihnen angegebene Pfad '{0}'{1}existiert nicht!\r\n\r\nBitte geben Sie einen gültigen Pfad bzw. Netzwerkfreigabe mit Schreibrechten für alle Benutzer an.", path, pathFriendlyName));
                return false;
            }
            if (!TryCreateFileWithOverwrite(Path.Combine(path, string.Format("TmpFileService_{0}.txt", Guid.NewGuid()))))
            {
                if (alertAction != null)
                    alertAction(string.Format("Der aktuelle Windows-User hat keine Schreibberechtigung auf den Pfad '{0}'{1}!\r\n\r\nBitte geben Sie einen Pfad mit Schreibrechten für alle Benutzer an!", path, pathFriendlyName));
                return false;
            }

            return true;
        }
    
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            var fs = File.OpenRead(fullFilePath);
            try
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }
        }
        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(sourceDirectory))
                return;

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyDirectoryAll(diSource, diTarget);
        }

        public static void CopyDirectoryAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists; if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectoryAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
