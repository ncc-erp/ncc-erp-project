using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class UpdatetableMeetingReportCriteriaandtableProjectWeeklySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallSummary",
                table: "ProjectWeeklySummaries");

            migrationBuilder.AddColumn<string>(
                name: "SectionName",
                table: "ProjectWeeklySummaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionName",
                table: "ProjectWeeklySummaries");

            migrationBuilder.AddColumn<string>(
                name: "OverallSummary",
                table: "ProjectWeeklySummaries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
