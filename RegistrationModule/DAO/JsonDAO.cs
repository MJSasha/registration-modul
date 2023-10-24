using RegistrationModule.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RegistrationModule.DAO
{
    public class JsonDAO<T> where T : IEntity
    {
        protected readonly List<T> entities;
        private readonly string jsonFilePath;

        public JsonDAO(string jsonFilePath)
        {
            this.jsonFilePath = jsonFilePath;

            if (File.Exists(this.jsonFilePath))
            {
                string json = File.ReadAllText(this.jsonFilePath);
                entities = string.IsNullOrWhiteSpace(json) ? new List<T>() : JsonSerializer.Deserialize<List<T>>(json);
            }
            else
            {
                entities = new List<T>();
            }
        }

        public List<T> GetAll()
        {
            return entities;
        }

        public void Create(T user)
        {
            entities.Add(user);
            SaveChanges();
        }

        public void Update(T entity)
        {
            int index = entities.FindIndex(user => user.Id == entity.Id);
            if (index != -1)
            {
                entities[index] = entity;
                SaveChanges();
            }
        }

        public void Delete(string id)
        {
            T entity = entities.Find(user => user.Id == id);
            if (entity != null)
            {
                entities.Remove(entity);
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            string json = JsonSerializer.Serialize(entities);
            File.WriteAllText(jsonFilePath, json);
        }
    }
}
