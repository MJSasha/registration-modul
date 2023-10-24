using RegistrationModul;
using RegistrationModul.Models;
using RegistrationModule.Models;
using System.Collections.Generic;
using System.Linq;

namespace RegistrationModule.DAO
{
    public class CompanyDAO : JsonDAO<Company>
    {
        public CompanyDAO() : base("store.json")
        {
            if (!entities.Any())
            {
                var salt = Utils.GenerateSalt();
                Create(new Company
                {
                    UUIDs = new List<string> { Utils.GetUUID() },
                    Users = new List<User> { new User { Name = "admin@a", Login = "admin@a", Role = Definitions.UserRole.Editor, Credentials = new Credentials { Salt = salt, Password = Utils.HashPassword("ad1234", salt) } }
                }});
            }
        }

        public Company GetByUUID(string uuid)
        {
            return entities.Find(c => c.UUIDs.Exists(id => id == uuid));
        }

        public Company GetCurrentCompany()
        {
            return GetByUUID(Utils.GetUUID());
        }
    }
}
