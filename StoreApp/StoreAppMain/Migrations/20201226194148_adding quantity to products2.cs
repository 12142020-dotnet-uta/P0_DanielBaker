using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class addingquantitytoproducts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    StoreLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreLocationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.StoreLocationId);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerFName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAge = table.Column<int>(type: "int", nullable: false),
                    PerferedStoreStoreLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_customers_locations_PerferedStoreStoreLocationId",
                        column: x => x.PerferedStoreStoreLocationId,
                        principalTable: "locations",
                        principalColumn: "StoreLocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPrice = table.Column<double>(type: "float", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    StoreLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAgeRestricted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_products_locations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "locations",
                        principalColumn: "StoreLocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_orders_customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_PerferedStoreStoreLocationId",
                table: "customers",
                column: "PerferedStoreStoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerID",
                table: "orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_products_StoreLocationId",
                table: "products",
                column: "StoreLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "locations");
        }
    }
}
