using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class AddLinkCv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkCV",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkCV",
                table: "ProjectUserBills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkCV",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "LinkCV",
                table: "ProjectUserBills");
        }
    }
}
