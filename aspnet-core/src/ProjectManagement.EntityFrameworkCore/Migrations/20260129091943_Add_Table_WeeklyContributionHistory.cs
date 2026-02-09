using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_WeeklyContributionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyContributionHistories",
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
                    UserId = table.Column<long>(nullable: false),
                    ProjectUserBillId = table.Column<long>(nullable: false),
                    PMReportId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    Contribute = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyContributionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyContributionHistories_PMReports_PMReportId",
                        column: x => x.PMReportId,
                        principalTable: "PMReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyContributionHistories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyContributionHistories_ProjectUserBills_ProjectUserBillId",
                        column: x => x.ProjectUserBillId,
                        principalTable: "ProjectUserBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyContributionHistories_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyContributionHistories_PMReportId",
                table: "WeeklyContributionHistories",
                column: "PMReportId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyContributionHistories_ProjectId",
                table: "WeeklyContributionHistories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyContributionHistories_ProjectUserBillId",
                table: "WeeklyContributionHistories",
                column: "ProjectUserBillId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyContributionHistories_UserId",
                table: "WeeklyContributionHistories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyContributionHistories");
        }
    }
}
