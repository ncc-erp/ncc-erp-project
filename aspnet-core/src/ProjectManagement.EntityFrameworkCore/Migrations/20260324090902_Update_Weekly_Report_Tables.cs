using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Update_Weekly_Report_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "ProjectWeeklySummaries");

            migrationBuilder.AddColumn<string>(
                name: "OverallSummary",
                table: "ProjectWeeklySummaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallSummary",
                table: "ProjectWeeklySummaries");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "ProjectWeeklySummaries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
