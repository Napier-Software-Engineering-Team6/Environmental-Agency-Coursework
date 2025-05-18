using Microsoft.EntityFrameworkCore;
using CourseworkApp.Database.Models;

namespace CourseworkApp.Database.Data
{
    /// <summary>
    /// TestDbContext is a derived class from GenericDbContext.
    /// It specifies the connection name for the test environment.
    /// </summary>
    public class TestDbContext : GenericDbContext
    {
        internal override string connectionName { get; set; } = "TestConnection";

        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }
    }
}
