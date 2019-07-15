using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class placeUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(
            //    name: "IX_CategoryProducts_Place",
            //    table: "CategoryProducts",
            //    column: "Place",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Categories_Place",
            //    table: "Categories",
            //    column: "Place",
            //    unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_CategoryProducts_Place",
            //    table: "CategoryProducts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Categories_Place",
            //    table: "Categories");
        }
    }
}
