using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onlineschool.Data.Migrations
{
    /// <inheritdoc />
    public partial class Categ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "moderator",
                table: "Groups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "moderator",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
