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
            //var directory = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);

            if (!IsArchive(filePath))
            {
                filePath += ".zip";
                File.Delete(file.Path.AbsolutePath);
            }

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using var zipStream = new ZipOutputStream(fs);

            zipStream.Password = password;
            var zipEntry = new ZipEntry(fileName);
            zipStream.PutNextEntry(zipEntry);

            using var writer = new StreamWriter(zipStream);
            writer.Write(content);

            return filePath + ".zip";
        }

        public static string ReadAndDecryptFile(IStorageFile file, string password = "")
        {
            var filePath = file.Path.AbsolutePath;
            var content = "";

            if (!IsArchive(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var reader = new StreamReader(fileStream);
                content = reader.ReadToEnd();
            }
            else
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var zipStream = new ZipInputStream(fs);
                zipStream.Password = password;

                ZipEntry entry = zipStream.GetNextEntry();

                if (entry == null)
                {
                    throw new Exception("Archive is empty or password is incorrect.");
                }

                using var reader = new StreamReader(zipStream);
                content = reader.ReadToEnd();
            }

            return content;
        }

        private static bool IsArchive(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase);
        }
    }
}
