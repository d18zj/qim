using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QimErp.Web.Migrations
{
    public partial class ChangeUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Core_TenantAccount",
                maxLength: 36,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_TenantAccount",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_TenantAccount",
                maxLength: 36,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_Tenant",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_Tenant",
                maxLength: 36,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_DatabaseInfo",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_DatabaseInfo",
                maxLength: 36,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_Tenant",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_Tenant",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Core_TenantAccount",
                maxLength: 20,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_TenantAccount",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_TenantAccount",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifyBy",
                table: "Core_DatabaseInfo",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Core_DatabaseInfo",
                maxLength: 50,
                nullable: false);
        }
    }
}
