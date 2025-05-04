using CourseworkApp.Models;

namespace CourseworkApp.Services
{
    /// <summary>
    /// Basic hardcoded authentication service for Admin/User roles.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly List<UserModel> _users = new()
        {
            // Sample users for demonstration purposes
            // In a real application, these would be stored securely in a database (DB access was proven in operations task)
            new UserModel { Username = "admin", Password = "admin123", Role = UserRole.Admin },
            new UserModel { Username = "user", Password = "user123", Role = UserRole.User }
        };

        /// <inheritdoc/>
        public UserModel? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);
        }
    }
}
