using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class promoDiscountUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoDiscounts_Products_ProductId",
                table: "PromoDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_PromoDiscounts_ProductId",
                table: "PromoDiscounts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PromoDiscounts");

            migrationBuilder.CreateTable(
                name: "ProductPromoDiscounts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PromoDiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromoDiscounts", x => new { x.ProductId, x.PromoDiscountId });
                    table.ForeignKey(
                        name: "FK_ProductPromoDiscounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPromoDiscounts_PromoDiscounts_PromoDiscountId",
                        column: x => x.PromoDiscountId,
                        principalTable: "PromoDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPromoDiscounts_PromoDiscountId",
                table: "ProductPromoDiscounts",
                column: "PromoDiscountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPromoDiscounts");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "PromoDiscounts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoDiscounts_ProductId",
                table: "PromoDiscounts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoDiscounts_Products_ProductId",
                table: "PromoDiscounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
