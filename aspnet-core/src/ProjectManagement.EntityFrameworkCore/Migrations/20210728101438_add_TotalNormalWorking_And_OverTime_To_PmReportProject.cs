using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_TotalNormalWorking_And_OverTime_To_PmReportProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalNormalWorkingTime",
                table: "PMReportProjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOverTime",
                table: "PMReportProjects",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalNormalWorkingTime",
                table: "PMReportProjects");

            migrationBuilder.DropColumn(
                name: "TotalOverTime",
                table: "PMReportProjects");
        }
    }
}
