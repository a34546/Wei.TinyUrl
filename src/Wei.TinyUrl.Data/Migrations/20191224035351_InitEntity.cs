using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wei.TinyUrl.Data.Migrations
{
    public partial class InitEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Url = table.Column<string>(maxLength: 2000, nullable: true),
                    ExpiryTime = table.Column<DateTime>(nullable: true),
                    Source = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMapping", x => x.Id);
                });
            migrationBuilder.Sql("INSERT INTO `urlmapping` (`CreateTime`, `IsDelete`,  `Code`, `Url`, `Source`) VALUES (now(), 0, 'WPysCS', 'http://www.baidu.com', 'test');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlMapping");
        }
    }
}
