using CourseworkApp.Models;

namespace CourseworkApp.Services
{
    public interface ILoginService
    {
        UserModel? Authenticate(string username, string password);
    }

}
