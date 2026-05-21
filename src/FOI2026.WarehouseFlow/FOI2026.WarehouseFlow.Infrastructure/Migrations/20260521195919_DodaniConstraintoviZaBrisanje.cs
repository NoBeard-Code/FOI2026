using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOI2026.WarehouseFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DodaniConstraintoviZaBrisanje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNoteItem_Item_ItemId",
                table: "DeliveryNoteItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Item_ItemId",
                table: "OrderItem");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNoteItem_Item_ItemId",
                table: "DeliveryNoteItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Item_ItemId",
                table: "OrderItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryNoteItem_Item_ItemId",
                table: "DeliveryNoteItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Item_ItemId",
                table: "OrderItem");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryNoteItem_Item_ItemId",
                table: "DeliveryNoteItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Item_ItemId",
                table: "OrderItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
