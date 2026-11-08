using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addfieldstogrouppermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanAdd",
                table: "GroupPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "GroupPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEdit",
                table: "GroupPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanView",
                table: "GroupPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAdd",
                table: "GroupPermissions");

            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "GroupPermissions");

            migrationBuilder.DropColumn(
                name: "CanEdit",
                table: "GroupPermissions");

            migrationBuilder.DropColumn(
                name: "CanView",
                table: "GroupPermissions");
        }
    }
}
