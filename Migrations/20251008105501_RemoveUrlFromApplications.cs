using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSOPortalX.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUrlFromApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url",
                table: "apps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "apps",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
