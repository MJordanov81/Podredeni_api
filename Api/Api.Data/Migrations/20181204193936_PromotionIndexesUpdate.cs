using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class PromotionIndexesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Promotions_PromoCode",
                table: "Promotions");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_PromoCode",
                table: "Promotions",
                column: "PromoCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Promotions_PromoCode",
                table: "Promotions");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Promotions_PromoCode",
                table: "Promotions",
                column: "PromoCode");
        }
    }
}
