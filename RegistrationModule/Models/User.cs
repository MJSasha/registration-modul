using RegistrationModule.Interfaces;
using RegistrationModule.Models;

namespace RegistrationModul.Models
{
    public class User : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public Credentials Credentials { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
