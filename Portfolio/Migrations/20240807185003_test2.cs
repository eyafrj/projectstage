using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityShares",
                table: "SecurityShares");

            migrationBuilder.AddColumn<int>(
                name: "SecurityshareId",
                table: "SecurityShares",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityShares",
                table: "SecurityShares",
                column: "SecurityshareId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityShares_AccountId",
                table: "SecurityShares",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityShares",
                table: "SecurityShares");

            migrationBuilder.DropIndex(
                name: "IX_SecurityShares_AccountId",
                table: "SecurityShares");

            migrationBuilder.DropColumn(
                name: "SecurityshareId",
                table: "SecurityShares");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityShares",
                table: "SecurityShares",
                columns: new[] { "AccountId", "SecurityId" });
        }
    }
}
