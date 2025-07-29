using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 4,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 5,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedByUserId",
                table: "Customers",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedByUserId",
                table: "Customers",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_CreatedByUserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedByUserId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Customers");
        }
    }
}
