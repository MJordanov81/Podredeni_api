using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Api.Data.Migrations
{
    public partial class PromotionFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountedProductsCount = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncludePriceDiscounts = table.Column<bool>(type: "bit", nullable: false),
                    IsAccumulative = table.Column<bool>(type: "bit", nullable: false),
                    IsInclusive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductsCount = table.Column<int>(type: "int", nullable: false),
                    PromoCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedQuota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountedProductsPromotions",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PromotionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountedProductsPromotions", x => new { x.ProductId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_DiscountedProductsPromotions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountedProductsPromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsPromotions",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PromotionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsPromotions", x => new { x.ProductId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_ProductsPromotions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsPromotions_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountedProductsPromotions_PromotionId",
                table: "DiscountedProductsPromotions",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsPromotions_PromotionId",
                table: "ProductsPromotions",
                column: "PromotionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountedProductsPromotions");

            migrationBuilder.DropTable(
                name: "ProductsPromotions");

            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
