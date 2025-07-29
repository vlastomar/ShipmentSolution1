using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Shipments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 1,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 2,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 3,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 4,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 5,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 6,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 7,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 8,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 9,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 10,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CreatedByUserId",
                table: "Shipments",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_AspNetUsers_CreatedByUserId",
                table: "Shipments",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_AspNetUsers_CreatedByUserId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_CreatedByUserId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Shipments");
        }
    }
}
