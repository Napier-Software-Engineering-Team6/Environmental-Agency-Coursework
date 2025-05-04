using Microsoft.EntityFrameworkCore;

namespace CourseworkApp.Database.Data
{
    public class CourseDbContext : GenericDbContext
    {
        internal override string connectionName { get; set; } = "DevelopmentConnection";

        public CourseDbContext()
        {
        }

        public CourseDbContext(DbContextOptions<CourseDbContext> options)
            : base(options)
        {
        }
    }
}
