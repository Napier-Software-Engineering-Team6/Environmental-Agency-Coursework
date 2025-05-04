using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSensorIdFromHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorConfigHistory_Sensors_SensorId",
                table: "SensorConfigHistory");

            migrationBuilder.DropIndex(
                name: "IX_SensorConfigHistory_SensorId",
                table: "SensorConfigHistory");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "SensorConfigHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "SensorConfigHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SensorConfigHistory_SensorId",
                table: "SensorConfigHistory",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorConfigHistory_Sensors_SensorId",
                table: "SensorConfigHistory",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "SensorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
