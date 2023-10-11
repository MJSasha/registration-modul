using RegistrationModul.Models;
using RegistrationModule.DAO;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.Services
{
    public class AuthService
    {
        private readonly JsonDAO<User> usersDAO;

        public AuthService()
        {
            usersDAO = new JsonDAO<User>("users.json");
        }

        public async Task CreateUser(User user)
        {
            var users = usersDAO.GetAll();

            user.Id = Utils.GetUUID();
            if (users.Exists(u => u.Id == user.Id || u.Login == user.Login)) throw new InvalidDataException("User with same login or UUID already exist!");
            usersDAO.Create(user);
        }

        public async Task<bool> CheckUserExist(string login, string password)
        {
            var user = usersDAO.GetAll().Find(u => u.Login == login && u.Id == Utils.GetUUID());
            return user != null && user.Credentials.Password == Utils.HashPassword(password, user.Credentials.Salt);
        }
    }
}
