using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class categoryPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "CategoryProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_Place",
                table: "CategoryProducts",
                column: "Place",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Place",
                table: "Categories",
                column: "Place",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_Place",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Place",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "Categories");
        }
    }
}
