using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_isExpose_HeadCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HeadCount",
                table: "ProjectUserBills",
                nullable: false,
                defaultValue: 1f);

            migrationBuilder.AddColumn<bool>(
                name: "isExpose",
                table: "ProjectUserBills",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadCount",
                table: "ProjectUserBills");

            migrationBuilder.DropColumn(
                name: "isExpose",
                table: "ProjectUserBills");
        }
    }
}
