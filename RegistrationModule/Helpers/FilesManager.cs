using Avalonia.Platform.Storage;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace RegistrationModule.Helpers
{
    public static class FilesManager
    {
        public static string EncryptAndWriteToFile(IStorageFile file, string password, string content)
        {
            var filePath = file.Path.AbsolutePath;
            var directory = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);

            using var fs = new FileStream(Path.Combine(directory, fileName + ".zip"), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using var zipStream = new ZipOutputStream(fs);

            zipStream.Password = password;
            var zipEntry = new ZipEntry(fileName);
            zipStream.PutNextEntry(zipEntry);

            using var writer = new StreamWriter(zipStream);
            writer.Write(content);
            File.Delete(filePath);

            return filePath + ".zip";
        }

        public static string ReadAndDecryptFile(IStorageFile file, string password = "")
        {
            var filePath = file.Path.AbsolutePath;

            if (!IsArchive(filePath))
            {
                return File.ReadAllText(filePath);
            }

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var zipStream = new ZipInputStream(fs);
            zipStream.Password = password;

            ZipEntry entry = zipStream.GetNextEntry();

            if (entry == null)
            {
                throw new Exception("Archive is empty or password is incorrect.");
            }

            using var reader = new StreamReader(zipStream);
            return reader.ReadToEnd();
        }

        private static bool IsArchive(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase);
        }
    }
}
