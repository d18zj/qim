using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Qim.EntitiFrameworkCore.Tests.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test_Blog",
                columns: table => new
                {
                    PId = table.Column<string>(maxLength: 36, nullable: false),
                    CreateBy = table.Column<string>(maxLength: 36, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    DeleteBy = table.Column<string>(maxLength: 36, nullable: true),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifyBy = table.Column<string>(maxLength: 36, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Blog", x => x.PId);
                });

            migrationBuilder.CreateTable(
                name: "Test_SeqGuid",
                columns: table => new
                {
                    PId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ArrKey = table.Column<byte[]>(maxLength: 16, nullable: false),
                    GuidKey = table.Column<Guid>(nullable: false),
                    StrKey = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_SeqGuid", x => x.PId);
                });

            migrationBuilder.CreateTable(
                name: "Test_Post",
                columns: table => new
                {
                    PId = table.Column<Guid>(nullable: false),
                    BlogId = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 36, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    LastModifyBy = table.Column<string>(maxLength: 36, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test_Post", x => x.PId);
                    table.ForeignKey(
                        name: "FK_Test_Post_Test_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Test_Blog",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Test_Post_BlogId",
                table: "Test_Post",
                column: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test_Post");

            migrationBuilder.DropTable(
                name: "Test_SeqGuid");

            migrationBuilder.DropTable(
                name: "Test_Blog");
        }
    }
}
