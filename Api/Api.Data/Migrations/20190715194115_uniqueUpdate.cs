using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class uniqueUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_CategoryProducts_Place",
            //    table: "CategoryProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(
            //    name: "IX_CategoryProducts_Place",
            //    table: "CategoryProducts",
            //    column: "Place",
            //    unique: true);
        }
    }
}
