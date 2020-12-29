using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class addinginventorytolocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_locations_StoreLocationId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_StoreLocationId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "StoreLocationId",
                table: "products");

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventory_locations_StoreLocationId",
                        column: x => x.StoreLocationId,
                        principalTable: "locations",
                        principalColumn: "StoreLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventory_products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductID",
                table: "Inventory",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreLocationId",
                table: "Inventory",
                column: "StoreLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreLocationId",
                table: "products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_StoreLocationId",
                table: "products",
                column: "StoreLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_locations_StoreLocationId",
                table: "products",
                column: "StoreLocationId",
                principalTable: "locations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
