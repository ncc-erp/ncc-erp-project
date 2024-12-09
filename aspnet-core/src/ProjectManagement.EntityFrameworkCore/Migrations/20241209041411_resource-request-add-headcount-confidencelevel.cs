using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class resourcerequestaddheadcountconfidencelevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ConfidenceLevel",
                table: "ResourceRequests",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HeadCount",
                table: "ResourceRequests",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfidenceLevel",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "HeadCount",
                table: "ResourceRequests");
        }
    }
}
