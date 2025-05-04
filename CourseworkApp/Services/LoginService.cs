using System.Collections.Generic;
using System.Linq;
using CourseworkApp.Models;

namespace CourseworkApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly List<UserModel> _users = new()
        {
            new UserModel { Username = "admin", Password = "admin123", Role = UserRole.Admin },
            new UserModel { Username = "user", Password = "user123", Role = UserRole.User }
        };

        public UserModel? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);
        }

    }
}
