namespace CourseworkApp.Models
{
    public enum UserRole
    {
        Admin,
        User
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; } // In real apps, this would be a hashed value
        public UserRole Role { get; set; }
    }
}
