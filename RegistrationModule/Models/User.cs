using RegistrationModule.Definitions;
using RegistrationModule.Interfaces;
using RegistrationModule.Models;
using System;

namespace RegistrationModul.Models
{
    public class User : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Login { get; set; }
        public Credentials Credentials { get; set; }
        public UserRole Role { get; set; } = UserRole.Viewer;
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
    }
}
