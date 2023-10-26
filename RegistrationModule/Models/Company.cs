using RegistrationModul.Models;
using RegistrationModule.Interfaces;
using System;
using System.Collections.Generic;

namespace RegistrationModule.Models
{
    public class Company : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> UUIDs { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<FileCredentials> FileCredentials { get; set; } = new();
    }
}
