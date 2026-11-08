using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_genral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtraHoursRate = table.Column<double>(type: "float", nullable: false),
                    DeductionHoursRate = table.Column<double>(type: "float", nullable: false),
                    FirstWeekendDay = table.Column<int>(type: "int", nullable: false),
                    SecondWeekendDay = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralSettings");
        }
    }
}
