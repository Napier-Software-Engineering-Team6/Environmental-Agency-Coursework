using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseworkApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorConfigurationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirmwareConfigurations",
                columns: table => new
                {
                    FirmwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmwareData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndofLifeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmwareConfigurations", x => x.FirmwareId);
                });

            migrationBuilder.CreateTable(
                name: "SensorConfigurations",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConfigData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorConfigurations", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentConfigId = table.Column<int>(type: "int", nullable: false),
                    CurrentFirmwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensors_FirmwareConfigurations_CurrentFirmwareId",
                        column: x => x.CurrentFirmwareId,
                        principalTable: "FirmwareConfigurations",
                        principalColumn: "FirmwareId");
                    table.ForeignKey(
                        name: "FK_Sensors_SensorConfigurations_CurrentConfigId",
                        column: x => x.CurrentConfigId,
                        principalTable: "SensorConfigurations",
                        principalColumn: "ConfigId");
                });

            migrationBuilder.CreateTable(
                name: "SensorConfigHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    ConfigId = table.Column<int>(type: "int", nullable: true),
                    FirmwareId = table.Column<int>(type: "int", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorConfigHistory", x => x.HistoryId);
                    table.CheckConstraint("CK_SensorConfigHistory_ConfigOrFirmware", "(ConfigId IS NULL AND FirmwareId IS NOT NULL) OR (ConfigId IS NOT NULL AND FirmwareId IS NULL)");
                    table.ForeignKey(
                        name: "FK_SensorConfigHistory_FirmwareConfigurations_FirmwareId",
                        column: x => x.FirmwareId,
                        principalTable: "FirmwareConfigurations",
                        principalColumn: "FirmwareId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SensorConfigHistory_SensorConfigurations_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "SensorConfigurations",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SensorConfigHistory_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorConfigHistory_ConfigId",
                table: "SensorConfigHistory",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorConfigHistory_FirmwareId",
                table: "SensorConfigHistory",
                column: "FirmwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorConfigHistory_SensorId",
                table: "SensorConfigHistory",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_CurrentConfigId",
                table: "Sensors",
                column: "CurrentConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_CurrentFirmwareId",
                table: "Sensors",
                column: "CurrentFirmwareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorConfigHistory");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "FirmwareConfigurations");

            migrationBuilder.DropTable(
                name: "SensorConfigurations");
        }
    }
}
