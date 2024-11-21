using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Migrations
{
    /// <inheritdoc />
    public partial class AddColumInAccountRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Accounts",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Accounts",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "NumberAccount",
                table: "Accounts",
                newName: "number_account");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Accounts",
                newName: "id_account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                newName: "IX_Accounts_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_user_id",
                table: "Accounts",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_user_id",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "Accounts",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Accounts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "number_account",
                table: "Accounts",
                newName: "NumberAccount");

            migrationBuilder.RenameColumn(
                name: "id_account",
                table: "Accounts",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_user_id",
                table: "Accounts",
                newName: "IX_Accounts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
