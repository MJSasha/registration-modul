using RegistrationModul;
using RegistrationModule.Models;
using System.Collections.Generic;
using System.Linq;

namespace RegistrationModule.DAO
{
    public class CompanyDAO : JsonDAO<Company>
    {
        public CompanyDAO() : base("store.json")
        {
            if (!entities.Any()) Create(new Company { UUIDs = new List<string> { Utils.GetUUID() } });
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
