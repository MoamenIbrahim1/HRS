using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResouresMangementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupPermissions_Groups_HrGroupId",
                table: "GroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Groups_HrGroupId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_HrGroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_HrGroupId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_GroupPermissions_HrGroupId",
                table: "GroupPermissions");

            migrationBuilder.DropColumn(
                name: "HrGroupId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "HrGroupId",
                table: "GroupPermissions");

            migrationBuilder.RenameColumn(
                name: "HrGroupId",
                table: "Users",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HrGroupId",
                table: "Users",
                newName: "IX_Users_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Groups_GroupId",
                table: "GroupPermissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupPermissions_Groups_GroupId",
                table: "GroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Users",
                newName: "HrGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                newName: "IX_Users_HrGroupId");

            migrationBuilder.AddColumn<int>(
                name: "HrGroupId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrGroupId",
                table: "GroupPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_HrGroupId",
                table: "Permissions",
                column: "HrGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_HrGroupId",
                table: "GroupPermissions",
                column: "HrGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Groups_HrGroupId",
                table: "GroupPermissions",
                column: "HrGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Groups_HrGroupId",
                table: "Permissions",
                column: "HrGroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_HrGroupId",
                table: "Users",
                column: "HrGroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
