using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Link_ResourceRequest_ResourceRequestCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequestCVs_ResourceRequestId",
                table: "ResourceRequestCVs",
                column: "ResourceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceRequestCVs_ResourceRequests_ResourceRequestId",
                table: "ResourceRequestCVs",
                column: "ResourceRequestId",
                principalTable: "ResourceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceRequestCVs_ResourceRequests_ResourceRequestId",
                table: "ResourceRequestCVs");

            migrationBuilder.DropIndex(
                name: "IX_ResourceRequestCVs_ResourceRequestId",
                table: "ResourceRequestCVs");
        }
    }
}
