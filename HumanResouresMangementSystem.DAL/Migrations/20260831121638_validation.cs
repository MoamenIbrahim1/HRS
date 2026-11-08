using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class validation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkData_ContractDate",
                table: "WorkData");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkData_Salary",
                table: "WorkData");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Employee_Age_Min20",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkData_ContractDate",
                table: "WorkData",
                sql: "YEAR([ContractDate]) >= 2008");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkData_Salary",
                table: "WorkData",
                sql: "[Salary] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Employee_Age_Min20",
                table: "Employees",
                sql: "DATEDIFF(YEAR, [BirthDate], GETDATE()) >= 20");
        }
    }
}
