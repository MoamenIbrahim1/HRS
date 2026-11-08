using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addhoursdeduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "EmployeeDeductions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "EmployeeDeductions");
        }
    }
}
