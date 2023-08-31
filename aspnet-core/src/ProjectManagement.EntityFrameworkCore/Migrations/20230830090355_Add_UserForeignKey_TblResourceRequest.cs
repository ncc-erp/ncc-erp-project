using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_UserForeignKey_TblResourceRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequests_BillAccountId",
                table: "ResourceRequests",
                column: "BillAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceRequests_AbpUsers_BillAccountId",
                table: "ResourceRequests",
                column: "BillAccountId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceRequests_AbpUsers_BillAccountId",
                table: "ResourceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ResourceRequests_BillAccountId",
                table: "ResourceRequests");
        }
    }
}
