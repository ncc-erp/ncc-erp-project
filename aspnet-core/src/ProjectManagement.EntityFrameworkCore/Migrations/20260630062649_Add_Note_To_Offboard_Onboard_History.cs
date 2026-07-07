using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Note_To_Offboard_Onboard_History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ProjectUserOnboardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OffboardUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "ProjectUserOnboardings");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OffboardUsers");
        }
    }
}
