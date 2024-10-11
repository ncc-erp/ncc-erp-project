using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_CvStatusId_ResoureRequestCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CvStatusId",
                table: "ResourceRequestCVs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequestCVs_CvStatusId",
                table: "ResourceRequestCVs",
                column: "CvStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceRequestCVs_CvStatus_CvStatusId",
                table: "ResourceRequestCVs",
                column: "CvStatusId",
                principalTable: "CvStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceRequestCVs_CvStatus_CvStatusId",
                table: "ResourceRequestCVs");

            migrationBuilder.DropIndex(
                name: "IX_ResourceRequestCVs_CvStatusId",
                table: "ResourceRequestCVs");

            migrationBuilder.DropColumn(
                name: "CvStatusId",
                table: "ResourceRequestCVs");
        }
    }
}
