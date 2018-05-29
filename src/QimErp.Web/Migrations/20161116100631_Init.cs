using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QimErp.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Core_DatabaseInfo",
                columns: table => new
                {
                    PId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    DbName = table.Column<string>(maxLength: 20, nullable: false),
                    DbNo = table.Column<string>(maxLength: 20, nullable: false),
                    DbSize = table.Column<int>(nullable: false),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: false),
                    LastModifyBy = table.Column<string>(maxLength: 50, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_DatabaseInfo", x => x.PId);
                });

            migrationBuilder.CreateTable(
                name: "Core_Tenant",
                columns: table => new
                {
                    PId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    DatabaseId = table.Column<int>(nullable: false),
                    EndTime = table.Column<DateTime>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastModifyBy = table.Column<string>(maxLength: 50, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    StartTime = table.Column<DateTime>(type: "date", nullable: false),
                    TenantName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Tenant", x => x.PId);
                    table.ForeignKey(
                        name: "FK_Core_Tenant_Core_DatabaseInfo_DatabaseId",
                        column: x => x.DatabaseId,
                        principalTable: "Core_DatabaseInfo",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Core_TenantAccount",
                columns: table => new
                {
                    PId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CellPhone = table.Column<string>(maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    LastModifyBy = table.Column<string>(maxLength: 50, nullable: true),
                    LastModifyOn = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_TenantAccount", x => x.PId);
                    table.ForeignKey(
                        name: "FK_Core_TenantAccount_Core_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Core_Tenant",
                        principalColumn: "PId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Core_DatabaseInfo_DbNo",
                table: "Core_DatabaseInfo",
                column: "DbNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Core_Tenant_DatabaseId",
                table: "Core_Tenant",
                column: "DatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_TenantAccount_CellPhone",
                table: "Core_TenantAccount",
                column: "CellPhone");

            migrationBuilder.CreateIndex(
                name: "IX_Core_TenantAccount_Email",
                table: "Core_TenantAccount",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Core_TenantAccount_TenantId",
                table: "Core_TenantAccount",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_TenantAccount_UserId",
                table: "Core_TenantAccount",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_TenantAccount");

            migrationBuilder.DropTable(
                name: "Core_Tenant");

            migrationBuilder.DropTable(
                name: "Core_DatabaseInfo");
        }
    }
}
