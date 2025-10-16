using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSOPortalX.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisableToApps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_disable",
                table: "apps",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_disable",
                table: "apps");
        }
    }
}
