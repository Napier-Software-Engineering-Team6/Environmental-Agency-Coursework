using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Database.Data
{
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
