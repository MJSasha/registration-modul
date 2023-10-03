using RegistrationModul.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RegistrationModule.DAO
{
    public class UsersDAO
    {
        private readonly string jsonFilePath;
        private readonly List<User> users;

        public UsersDAO(string jsonFilePath)
        {
            this.jsonFilePath = jsonFilePath;

            if (File.Exists(this.jsonFilePath))
            {
                string json = File.ReadAllText(this.jsonFilePath);
                users = string.IsNullOrWhiteSpace(json) ? new List<User>() : JsonSerializer.Deserialize<List<User>>(json);
            }
            else
            {
                users = new List<User>();
            }
        }

        public List<User> GetAll()
        {
            return users;
        }

        public void Create(User user)
        {
            users.Add(user);
            SaveChanges();
        }

        public void Update(User updatedUser)
        {
            int index = users.FindIndex(user => user.Id == updatedUser.Id);
            if (index != -1)
            {
                users[index] = updatedUser;
                SaveChanges();
            }
        }

        public void Delete(string id)
        {
            User userToRemove = users.Find(user => user.Id == id);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonFilePath, json);
        }
    }
}
