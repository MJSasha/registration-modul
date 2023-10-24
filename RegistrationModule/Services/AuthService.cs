using RegistrationModul.Models;
using RegistrationModule.DAO;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.Services
{
    public class AuthService
    {
        public static User CurrentUser { get; private set; }

        private readonly CompanyDAO companyDAO;

        public AuthService()
        {
            companyDAO = new CompanyDAO();
        }

        public async Task CreateUser(User user)
        {
            var company = companyDAO.GetCurrentCompany();
            var users = company.Users;

            user.Id = Utils.GetUUID();
            if (users.Exists(u => u.Id == user.Id || u.Login == user.Login)) throw new InvalidDataException("User with same login or UUID already exist!");
            company.Users.Add(user);
            companyDAO.Update(company);
        }

        public async Task<bool> CheckUserExist(string login, string password)
        {
            var user = companyDAO.GetCurrentCompany().Users.Find(u => u.Login == login);
            var isExist = user != null && user.Credentials.Password == Utils.HashPassword(password, user.Credentials.Salt);
            if (isExist) CurrentUser = user;
            return isExist;
        }
    }
}
