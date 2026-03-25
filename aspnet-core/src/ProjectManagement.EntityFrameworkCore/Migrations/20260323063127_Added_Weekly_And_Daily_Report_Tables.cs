using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Added_Weekly_And_Daily_Report_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectWeeklySummaries",
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
                    Summary = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    PMReportId = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectWeeklySummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectWeeklySummaries_PMReports_PMReportId",
                        column: x => x.PMReportId,
                        principalTable: "PMReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectWeeklySummaries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDailyReports",
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
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Content = table.Column<string>(nullable: true),
                    WeeklySummaryId = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDailyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDailyReports_ProjectWeeklySummaries_WeeklySummaryId",
                        column: x => x.WeeklySummaryId,
                        principalTable: "ProjectWeeklySummaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDailyReports_WeeklySummaryId",
                table: "ProjectDailyReports",
                column: "WeeklySummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectWeeklySummaries_PMReportId",
                table: "ProjectWeeklySummaries",
                column: "PMReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectWeeklySummaries_ProjectId",
                table: "ProjectWeeklySummaries",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDailyReports");

            migrationBuilder.DropTable(
                name: "ProjectWeeklySummaries");
        }
    }
}
