using CourseworkApp.Models;
using CourseworkApp.Services;
using Xunit;

namespace CourseworkApp.Tests.Services
{
    public class LoginServiceTests
    {
        private readonly LoginService _loginService;

        public LoginServiceTests()
        {
            _loginService = new LoginService();
        }

        [Fact]
        public void Authenticate_WithValidAdminCredentials_ReturnsAdminUser()
        {
            var result = _loginService.Authenticate("admin", "admin123");

            Assert.NotNull(result);
            Assert.Equal(UserRole.Admin, result!.Role);
        }

        [Fact]
        public void Authenticate_WithValidUserCredentials_ReturnsUser()
        {
            var result = _loginService.Authenticate("user", "user123");

            Assert.NotNull(result);
            Assert.Equal(UserRole.User, result!.Role);
        }

        [Fact]
        public void Authenticate_WithInvalidCredentials_ReturnsNull()
        {
            var result = _loginService.Authenticate("unknown", "wrong");

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_IsCaseInsensitiveForUsername()
        {
            var result = _loginService.Authenticate("AdMiN", "admin123");

            Assert.NotNull(result);
            Assert.Equal(UserRole.Admin, result!.Role);
        }
    }
}
