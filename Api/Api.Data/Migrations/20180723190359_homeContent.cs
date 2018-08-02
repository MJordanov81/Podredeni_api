using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Data.Migrations
{
    public partial class homeContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeContent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ArticleHeading = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SectionContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SectionHeading = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeContent", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeContent");
        }
    }
}
