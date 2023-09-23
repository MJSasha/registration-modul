using RegistrationModul.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RegistrationModul.Services
{
    public class StorageService
    {
        private readonly string usersFile = "users.json";

        public StorageService()
        {
            if (!File.Exists(usersFile))
            {
                using var file = File.Create(usersFile);
            }
        }

        public async Task CreateUser(User user)
        {
            var usersJson = await File.ReadAllTextAsync(usersFile);
            var users = string.IsNullOrWhiteSpace(usersJson) ? new List<User>() : JsonSerializer.Deserialize<List<User>>(usersJson);

            user.Id = GetUUID();
            if (users.Exists(u => u.Id == user.Id || u.Login == user.Login)) throw new InvalidDataException("User already exist!");

            users.Add(user);
            await File.WriteAllTextAsync(usersFile, JsonSerializer.Serialize(users));
        }

        public async Task<bool> CheckUserExist(string login, string password)
        {
            var usersJson = await File.ReadAllTextAsync(usersFile);
            var users = string.IsNullOrWhiteSpace(usersJson) ? new List<User>() : JsonSerializer.Deserialize<List<User>>(usersJson);

            return users.Exists(u => u.Login == login && u.Password == password && u.Id == Utils.GetUUID());
        }

        private string GetUUID()
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
    }
}
