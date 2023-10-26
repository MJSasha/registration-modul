using Avalonia.Platform.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationModul
{
    public static class Utils
    {
        public static string GetUUID()
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c " + "wmic csproduct get UUID")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var proc = new Process() { StartInfo = procStartInfo };
            proc.Start();

            return proc.StandardOutput.ReadToEnd().Replace("UUID", string.Empty).Trim().ToUpper();
        }

        public static string GenerateSalt(int length = 32)
        {
            byte[] salt = new byte[length];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
            byte[] hashBytes = SHA256.HashData(combinedBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static string ChangeFileExtension(string filePath)
        {
            var currentExtension = Path.GetExtension(filePath);

            if (currentExtension == ".secretextension") return filePath;

            string newFiliPath = Path.ChangeExtension(filePath, ".secretextension");
            File.Move(filePath, newFiliPath);
            return newFiliPath;
        }

        public static async Task<string> HashFile(IStorageFile file)
        {
            using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            var bytes = Encoding.UTF8.GetBytes(await reader.ReadToEndAsync());
            byte[] hashBytes = SHA256.HashData(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
