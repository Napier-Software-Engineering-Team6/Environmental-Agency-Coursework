using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class removedDuplicatedRequireds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This method is intentionally left empty because the migration is irreversible.
            // Throwing a NotSupportedException to indicate that rolling back this migration is not supported.
            throw new NotSupportedException("This migration cannot be reversed.");
        }
    }
}
