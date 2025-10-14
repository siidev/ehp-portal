using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSOPortalX.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIconUrlFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon_url",
                table: "categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon_url",
                table: "categories",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
