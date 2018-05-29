using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QimErp.Web.Migrations.TenantDb
{
    public partial class ChangeUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_Users",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_Users",
                maxLength: 36,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Core_Users_TenantId",
                table: "Core_Users",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Core_Users_TenantId",
                table: "Core_Users");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_Users",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_Users",
                maxLength: 50,
                nullable: false);
        }
    }
}
