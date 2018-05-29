using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QimErp.Web.Migrations.TenantDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Core_Users",
                columns: table => new
                {
                    PId = table.Column<string>(maxLength: 36, nullable: false),
                    CellPhone = table.Column<string>(maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    LastModifyBy = table.Column<string>(maxLength: 50, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    LoginCount = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Salt = table.Column<string>(maxLength: 50, nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    UserAccount = table.Column<string>(maxLength: 20, nullable: false),
                    UserName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Users", x => x.PId);
                    //table.ForeignKey(
                    //    name: "FK_Core_Users_Core_Users_CreateBy",
                    //    column: x => x.CreateBy,
                    //    principalTable: "Core_Users",
                    //    principalColumn: "PId",
                    //    onDelete: ReferentialAction.Cascade);
                    //table.ForeignKey(
                    //    name: "FK_Core_Users_Core_Users_LastModifyBy",
                    //    column: x => x.LastModifyBy,
                    //    principalTable: "Core_Users",
                    //    principalColumn: "PId",
                    //    onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Core_Users_CreateBy",
                table: "Core_Users",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Users_LastModifyBy",
                table: "Core_Users",
                column: "LastModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Users_UserAccount",
                table: "Core_Users",
                column: "UserAccount",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_Users");
        }
    }
}
