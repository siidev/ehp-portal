using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSOPortalX.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesForSsoTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sso_portal_tokens_user_id",
                table: "sso_portal_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_sso_portal_tokens_token",
                table: "sso_portal_tokens",
                column: "token");

            migrationBuilder.CreateIndex(
                name: "IX_sso_portal_tokens_user_active_exp",
                table: "sso_portal_tokens",
                columns: new[] { "user_id", "is_active", "expires_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sso_portal_tokens_token",
                table: "sso_portal_tokens");

            migrationBuilder.DropIndex(
                name: "IX_sso_portal_tokens_user_active_exp",
                table: "sso_portal_tokens");

            migrationBuilder.CreateIndex(
                name: "IX_sso_portal_tokens_user_id",
                table: "sso_portal_tokens",
                column: "user_id");
        }
    }
}
