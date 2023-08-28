using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_fields_in_Project_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BriefDescription",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewKnowledge",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherProblems",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalProblems",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnologyUsed",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BriefDescription",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "NewKnowledge",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OtherProblems",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnicalProblems",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnologyUsed",
                table: "Projects");
        }
    }
}
