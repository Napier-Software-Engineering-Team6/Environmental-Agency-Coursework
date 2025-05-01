using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3109), new DateTime(2025, 4, 28, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3106), new DateTime(2025, 4, 28, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3042) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 30, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3120), new DateTime(2025, 4, 29, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3118), new DateTime(2025, 4, 29, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3112) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 21, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3129), new DateTime(2025, 4, 26, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3128), new DateTime(2025, 4, 28, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3123) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3136), new DateTime(2025, 4, 28, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3134), new DateTime(2025, 4, 28, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3131) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 30, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3142), new DateTime(2025, 4, 30, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3141), new DateTime(2025, 4, 24, 15, 22, 37, 465, DateTimeKind.Local).AddTicks(3138) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4018), new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4016), new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(3953) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4030), new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4028), new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4022) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 21, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4039), new DateTime(2025, 4, 26, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4038), new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4033) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 29, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4047), new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4045), new DateTime(2025, 4, 28, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4041) });

            migrationBuilder.UpdateData(
                table: "weather",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate", "Time" },
                values: new object[] { new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4053), new DateTime(2025, 4, 30, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4052), new DateTime(2025, 4, 24, 15, 15, 15, 588, DateTimeKind.Local).AddTicks(4049) });
        }
    }
}
