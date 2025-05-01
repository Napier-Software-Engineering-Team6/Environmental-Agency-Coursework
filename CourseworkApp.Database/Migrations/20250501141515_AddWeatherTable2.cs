using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature_2m = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Relative_Humidity_2m = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Wind_Speed_10m = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Wind_Direction_10m = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "weather",
                columns: new[] { "Id", "EndDate", "Name", "Relative_Humidity_2m", "StartDate", "Temperature_2m", "Time", "Wind_Direction_10m", "Wind_Speed_10m" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4018), "Default Sensor", 65.0m, new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4016), 18.5m, new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(3953), 45.0m, 12.3m },
                    { 2, new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4030), "Backup Sensor", 70.0m, new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4028), 20.1m, new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4022), 90.0m, 15.0m },
                    { 3, new DateTime(2025, 4, 21, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4039), "Default Sensor", 64.0m, new DateTime(2025, 4, 26, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4038), 11.5m, new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4033), 44.0m, 11.1m },
                    { 4, new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4047), "Default Sensor", 65.0m, new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4045), 15.5m, new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4041), 47.0m, 10.3m },
                    { 5, new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4053), "Default Sensor", 60.0m, new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4052), 17.5m, new DateTime(2025, 4, 24, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4049), 45.0m, 10.3m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather");
        }
    }
}
