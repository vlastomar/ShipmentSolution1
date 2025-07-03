using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShipmentSolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PreferredShippingMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingCostThreshold = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "MailCarriers",
                columns: table => new
                {
                    MailCarrierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailCarriers", x => x.MailCarrierId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Dimensions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingCost = table.Column<float>(type: "real", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK_Shipments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MailCarrierId = table.Column<int>(type: "int", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Stops = table.Column<int>(type: "int", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                    table.ForeignKey(
                        name: "FK_Routes_MailCarriers_MailCarrierId",
                        column: x => x.MailCarrierId,
                        principalTable: "MailCarriers",
                        principalColumn: "MailCarrierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    MailCarrierId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    DateDelivered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_Deliveries_MailCarriers_MailCarrierId",
                        column: x => x.MailCarrierId,
                        principalTable: "MailCarriers",
                        principalColumn: "MailCarrierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "City", "Email", "FirstName", "LastName", "PhoneNumber", "PreferredShippingMethod", "ShippingCostThreshold", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, "New York City", "john.doe@example.com", "John", "Doe", "1234567890", "Express", 50f, "NY", "10001" },
                    { 2, "Los Angeles", "jane.smith@example.com", "Jane", "Smith", "9876543210", "Ground", 30f, "CA", "90001" },
                    { 3, "Chicago", "michael.johnson@example.com", "Michael", "Johnson", "5555555555", "Express", 75f, "IL", "60601" },
                    { 4, "Houston", "emily.brown@example.com", "Emily", "Brown", "1111111111", "Ground", 40f, "TX", "77001" },
                    { 5, "Miami", "william.taylor@example.com", "William", "Taylor", "9999999999", "Express", 60f, "FL", "33101" }
                });

            migrationBuilder.InsertData(
                table: "MailCarriers",
                columns: new[] { "MailCarrierId", "CurrentLocation", "Email", "EndDate", "FirstName", "LastName", "PhoneNumber", "RouteId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, "New York City", "david.wilson@example.com", null, "David", "Wilson", "1112223333", 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Available" },
                    { 2, "Los Angeles", "sarah.anderson@example.com", null, "Sarah", "Anderson", "4445556666", 2, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On Break" },
                    { 3, "Chicago", "robert.miller@example.com", null, "Robert", "Miller", "7778889999", 3, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On a Delivery" },
                    { 4, "Houston", "jennifer.thomas@example.com", null, "Jennifer", "Thomas", "1231231234", 4, new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Available" },
                    { 5, "Miami", "daniel.wilson@example.com", null, "Daniel", "Wilson", "9998887777", 5, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On Break" }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "Distance", "EndLocation", "MailCarrierId", "Priority", "StartLocation", "Stops" },
                values: new object[,]
                {
                    { 1, 150.5f, "Albany", 1, 1, "New York City", 0 },
                    { 2, 400.2f, "San Francisco", 2, 1, "Los Angeles", 0 },
                    { 3, 250.8f, "Detroit", 3, 1, "Chicago", 0 },
                    { 4, 50f, "Los Angeles", 3, 2, "Detroit", 1 },
                    { 5, 150.7f, "Orlando", 5, 1, "Miami", 0 },
                    { 6, 0f, "Albany", 1, 2, "Albany", 0 },
                    { 7, 400.2f, "Seattle", 2, 2, "San Francisco", 1 },
                    { 8, 250.8f, "boston", 4, 1, "Denver", 0 },
                    { 9, 50f, "Dallas", 4, 2, "boston", 1 },
                    { 10, 80f, "San Francisco", 5, 2, "Orlando", 1 }
                });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "ShipmentId", "CustomerId", "DeliveryDate", "Dimensions", "IsDeleted", "ShippingCost", "ShippingMethod", "Weight" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "12x10x8", false, 15f, "Ground", 10.5f },
                    { 2, 2, new DateTime(2023, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "10x8x6", false, 25f, "Express", 7.2f },
                    { 3, 3, new DateTime(2023, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "16x12x10", false, 18.5f, "Ground", 15.3f },
                    { 4, 4, new DateTime(2023, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "8x6x4", false, 30f, "Express", 5.9f },
                    { 5, 5, new DateTime(2023, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "14x10x8", false, 20f, "Ground", 12.8f },
                    { 6, 1, new DateTime(2023, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "10x8x6", false, 14.5f, "Ground", 9.7f },
                    { 7, 4, new DateTime(2023, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "8x6x4", false, 28f, "Express", 6.5f },
                    { 8, 5, new DateTime(2023, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "12x10x8", false, 17f, "Ground", 11.2f },
                    { 9, 2, new DateTime(2023, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "14x10x8", false, 32.5f, "Express", 8.9f },
                    { 10, 3, new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "16x12x10", false, 19.5f, "Ground", 13.7f }
                });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "DeliveryId", "DateDelivered", "MailCarrierId", "RouteId", "ShipmentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, new DateTime(2023, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 2 },
                    { 3, new DateTime(2023, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 3 },
                    { 4, new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, 4 },
                    { 5, new DateTime(2023, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 5, 5 },
                    { 6, new DateTime(2023, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6, 6 },
                    { 7, new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7, 7 },
                    { 8, new DateTime(2023, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 8, 8 },
                    { 9, new DateTime(2023, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 9, 9 },
                    { 10, new DateTime(2023, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 10, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_MailCarrierId",
                table: "Deliveries",
                column: "MailCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_RouteId",
                table: "Deliveries",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ShipmentId",
                table: "Deliveries",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_MailCarrierId",
                table: "Routes",
                column: "MailCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CustomerId",
                table: "Shipments",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "MailCarriers");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
