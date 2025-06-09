using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Rename_MezonId_To_MezonUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MezonId",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "MezonUserId",
                table: "AbpUsers",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MezonUserId",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "MezonId",
                table: "AbpUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }
    }
}
