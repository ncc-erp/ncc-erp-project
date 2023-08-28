using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class addNecessaryReviewintoPMReportProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NecessaryReview",
                table: "PMReportProjects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NecessaryReview",
                table: "PMReportProjects");
        }
    }
}
