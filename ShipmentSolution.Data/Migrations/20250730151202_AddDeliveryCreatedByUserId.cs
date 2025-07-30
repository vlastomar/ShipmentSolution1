using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Deliveries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 1,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 2,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 3,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 4,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 5,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 6,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 7,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 8,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 9,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Deliveries",
                keyColumn: "DeliveryId",
                keyValue: 10,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CreatedByUserId",
                table: "Deliveries",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_AspNetUsers_CreatedByUserId",
                table: "Deliveries",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_AspNetUsers_CreatedByUserId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_CreatedByUserId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Deliveries");
        }
    }
}
