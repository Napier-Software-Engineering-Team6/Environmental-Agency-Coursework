using CourseworkApp.Models;

namespace CourseworkApp.Services
{
    /// <summary>
    /// Stores the currently logged-in user session.
    /// </summary>
    public static class SessionService
    {
        /// <summary>
        /// Gets or sets the current logged-in user.
        /// </summary>
        public static UserModel? LoggedInUser { get; set; }
    }
}
