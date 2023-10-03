using RegistrationModul.Models;
using RegistrationModule.DAO;
using System.IO;
using System.Threading.Tasks;

namespace RegistrationModul.Services
{
    public class AuthService
    {
        private readonly UsersDAO usersDAO;

        public AuthService()
        {
            usersDAO = new UsersDAO("users.json");
        }

        public async Task CreateUser(User user)
        {
            var users = usersDAO.GetAll();

            user.Id = Utils.GetUUID();
            if (users.Exists(u => u.Id == user.Id || u.Login == user.Login)) throw new InvalidDataException("User already exist!");
            usersDAO.Create(user);
        }

        public async Task<bool> CheckUserExist(string login, string password)
        {
            var users = usersDAO.GetAll();
            return users.Exists(u => u.Login == login && u.Password == password && u.Id == Utils.GetUUID());
        }
    }
}
