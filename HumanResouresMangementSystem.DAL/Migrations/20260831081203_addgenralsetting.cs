using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addgenralsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeductionSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficialHolidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialHolidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RateType = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DeductionSettingId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductions_DeductionSettings_DeductionSettingId",
                        column: x => x.DeductionSettingId,
                        principalTable: "DeductionSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOvertimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OvertimeSettingId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HourRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOvertimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeOvertimes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeOvertimes_OvertimeSettings_OvertimeSettingId",
                        column: x => x.OvertimeSettingId,
                        principalTable: "OvertimeSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductions_DeductionSettingId",
                table: "EmployeeDeductions",
                column: "DeductionSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductions_EmployeeId",
                table: "EmployeeDeductions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOvertimes_EmployeeId",
                table: "EmployeeOvertimes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOvertimes_OvertimeSettingId",
                table: "EmployeeOvertimes",
                column: "OvertimeSettingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDeductions");

            migrationBuilder.DropTable(
                name: "EmployeeOvertimes");

            migrationBuilder.DropTable(
                name: "OfficialHolidays");

            migrationBuilder.DropTable(
                name: "DeductionSettings");

            migrationBuilder.DropTable(
                name: "OvertimeSettings");
        }
    }
}
