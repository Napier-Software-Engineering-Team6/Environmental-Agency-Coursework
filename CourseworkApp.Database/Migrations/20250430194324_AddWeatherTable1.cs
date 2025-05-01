using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 28, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(3937), new DateTime(2025, 4, 27, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(3934), new DateTime(2025, 4, 27, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(3878) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(4021), new DateTime(2025, 4, 28, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(4013), new DateTime(2025, 4, 28, 20, 43, 24, 169, DateTimeKind.Local).AddTicks(3941) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 28, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9884), new DateTime(2025, 4, 27, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9882), new DateTime(2025, 4, 27, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9817) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9893), new DateTime(2025, 4, 28, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9891), new DateTime(2025, 4, 28, 20, 41, 6, 715, DateTimeKind.Local).AddTicks(9887) });
        }
    }
}
