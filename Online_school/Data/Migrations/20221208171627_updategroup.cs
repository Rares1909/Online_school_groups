using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onlineschool.Data.Migrations
{
    /// <inheritdoc />
    public partial class updategroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGroup_AspNetUsers_UserId",
                table: "ApplicationUserGroup");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ApplicationUserGroup",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserGroup_UserId",
                table: "ApplicationUserGroup",
                newName: "IX_ApplicationUserGroup_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGroup_AspNetUsers_UsersId",
                table: "ApplicationUserGroup",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGroup_AspNetUsers_UsersId",
                table: "ApplicationUserGroup");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ApplicationUserGroup",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserGroup_UsersId",
                table: "ApplicationUserGroup",
                newName: "IX_ApplicationUserGroup_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGroup_AspNetUsers_UserId",
                table: "ApplicationUserGroup",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
