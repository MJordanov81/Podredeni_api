using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class carouselItemCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "CarouselItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CarouselItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CarouselItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "CarouselItems",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
