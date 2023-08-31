using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Code_TableResourceRequest_RequestIdIsTemp_TableProjectUserBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemp",
                table: "ProjectUserBills",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ResourceRequestId",
                table: "ProjectUserBills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserBills_ResourceRequestId",
                table: "ProjectUserBills",
                column: "ResourceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUserBills_ResourceRequests_ResourceRequestId",
                table: "ProjectUserBills",
                column: "ResourceRequestId",
                principalTable: "ResourceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUserBills_ResourceRequests_ResourceRequestId",
                table: "ProjectUserBills");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUserBills_ResourceRequestId",
                table: "ProjectUserBills");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "IsTemp",
                table: "ProjectUserBills");

            migrationBuilder.DropColumn(
                name: "ResourceRequestId",
                table: "ProjectUserBills");
        }
    }
}
