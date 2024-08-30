using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Migrations
{
    /// <inheritdoc />
    public partial class orderss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_securities_SecurityID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "SecurityID",
                table: "Orders",
                newName: "SecurityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SecurityID",
                table: "Orders",
                newName: "IX_Orders_SecurityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_securities_SecurityId",
                table: "Orders",
                column: "SecurityId",
                principalTable: "securities",
                principalColumn: "SecurityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_securities_SecurityId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "SecurityId",
                table: "Orders",
                newName: "SecurityID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SecurityId",
                table: "Orders",
                newName: "IX_Orders_SecurityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_securities_SecurityID",
                table: "Orders",
                column: "SecurityID",
                principalTable: "securities",
                principalColumn: "SecurityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
