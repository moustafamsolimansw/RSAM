using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSAM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsUsedTOOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                schema: "public",
                table: "UserOTPs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserOTPs_UserId",
                schema: "public",
                table: "UserOTPs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOTPs_Users_UserId",
                schema: "public",
                table: "UserOTPs",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOTPs_Users_UserId",
                schema: "public",
                table: "UserOTPs");

            migrationBuilder.DropIndex(
                name: "IX_UserOTPs_UserId",
                schema: "public",
                table: "UserOTPs");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                schema: "public",
                table: "UserOTPs");
        }
    }
}
