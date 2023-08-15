using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class update_Table_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Branch",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCode",
                table: "AbpUsers",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "UserLevel",
                table: "AbpUsers",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserLevel",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AbpUsers");
        }
    }
}
