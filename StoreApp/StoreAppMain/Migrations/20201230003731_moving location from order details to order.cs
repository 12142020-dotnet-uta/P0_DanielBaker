using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class movinglocationfromorderdetailstoorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreLocationId",
                table: "orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_StoreLocationId",
                table: "orders",
                column: "StoreLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_locations_StoreLocationId",
                table: "orders",
                column: "StoreLocationId",
                principalTable: "locations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_locations_StoreLocationId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_StoreLocationId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "StoreLocationId",
                table: "orders");
        }
    }
}
