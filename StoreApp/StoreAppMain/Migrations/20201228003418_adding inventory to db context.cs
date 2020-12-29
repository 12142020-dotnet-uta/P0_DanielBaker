using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class addinginventorytodbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_locations_StoreLocationId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_products_ProductID",
                table: "Inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory");

            migrationBuilder.RenameTable(
                name: "Inventory",
                newName: "inventories");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_StoreLocationId",
                table: "inventories",
                newName: "IX_inventories_StoreLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_ProductID",
                table: "inventories",
                newName: "IX_inventories_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventories",
                table: "inventories",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventories_locations_StoreLocationId",
                table: "inventories",
                column: "StoreLocationId",
                principalTable: "locations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_inventories_products_ProductID",
                table: "inventories",
                column: "ProductID",
                principalTable: "products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventories_locations_StoreLocationId",
                table: "inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_inventories_products_ProductID",
                table: "inventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventories",
                table: "inventories");

            migrationBuilder.RenameTable(
                name: "inventories",
                newName: "Inventory");

            migrationBuilder.RenameIndex(
                name: "IX_inventories_StoreLocationId",
                table: "Inventory",
                newName: "IX_Inventory_StoreLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_inventories_ProductID",
                table: "Inventory",
                newName: "IX_Inventory_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_locations_StoreLocationId",
                table: "Inventory",
                column: "StoreLocationId",
                principalTable: "locations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_products_ProductID",
                table: "Inventory",
                column: "ProductID",
                principalTable: "products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
