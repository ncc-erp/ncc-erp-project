using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_tbl_PMReportProjectRisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PMReportProjectRisks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    PMReportProjectId = table.Column<long>(nullable: false),
                    Risk = table.Column<string>(maxLength: 10000, nullable: true),
                    Impact = table.Column<string>(maxLength: 10000, nullable: true),
                    Priority = table.Column<byte>(nullable: false),
                    Solution = table.Column<string>(maxLength: 10000, nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMReportProjectRisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMReportProjectRisks_PMReportProjects_PMReportProjectId",
                        column: x => x.PMReportProjectId,
                        principalTable: "PMReportProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PMReportProjectRisks_PMReportProjectId",
                table: "PMReportProjectRisks",
                column: "PMReportProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMReportProjectRisks");
        }
    }
}
