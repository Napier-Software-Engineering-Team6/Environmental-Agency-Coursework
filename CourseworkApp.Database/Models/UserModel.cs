namespace CourseworkApp.Models
{
    /// <summary>
    /// Represents a user in the system with login credentials and role.
    /// </summary>
    public enum UserRole
    {
        Admin,
        User
    }

    /// <summary>
    /// User model storing username, password, and role.
    /// </summary>
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
