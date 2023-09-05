using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_tbls_for_ProjectCriteria_Feature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "PMReportProjectIssues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProjectCriterias",
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
                    Guideline = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCriterias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCriteriaResults",
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
                    Note = table.Column<string>(nullable: true),
                    PMReportId = table.Column<long>(nullable: false),
                    ProjectCriteriaId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCriteriaResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCriteriaResults_PMReports_PMReportId",
                        column: x => x.PMReportId,
                        principalTable: "PMReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCriteriaResults_ProjectCriterias_ProjectCriteriaId",
                        column: x => x.ProjectCriteriaId,
                        principalTable: "ProjectCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCriteriaResults_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCriteriaResults_PMReportId",
                table: "ProjectCriteriaResults",
                column: "PMReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCriteriaResults_ProjectCriteriaId",
                table: "ProjectCriteriaResults",
                column: "ProjectCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCriteriaResults_ProjectId",
                table: "ProjectCriteriaResults",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCriteriaResults");

            migrationBuilder.DropTable(
                name: "ProjectCriterias");

            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "PMReportProjectIssues");
        }
    }
}
