using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Update_ResourceRequest_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecruitmentSend",
                table: "ResourceRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "Level",
                table: "ResourceRequests",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Priority",
                table: "ResourceRequests",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "RecruitmentUrl",
                table: "ResourceRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecruitmentSend",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "RecruitmentUrl",
                table: "ResourceRequests");
        }
    }
}
