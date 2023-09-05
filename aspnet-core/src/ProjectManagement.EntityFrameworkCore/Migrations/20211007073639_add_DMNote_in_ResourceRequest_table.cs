using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_DMNote_in_ResourceRequest_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "ResourceRequests");

            migrationBuilder.AddColumn<string>(
                name: "DMNote",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PMNote",
                table: "ResourceRequests",
                maxLength: 10000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DMNote",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "PMNote",
                table: "ResourceRequests");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ResourceRequests",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);
        }
    }
}
