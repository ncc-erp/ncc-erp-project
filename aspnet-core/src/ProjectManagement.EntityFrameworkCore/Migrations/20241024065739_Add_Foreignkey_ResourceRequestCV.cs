using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Foreignkey_ResourceRequestCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequestCVs_UserId",
                table: "ResourceRequestCVs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceRequestCVs_AbpUsers_UserId",
                table: "ResourceRequestCVs",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceRequestCVs_AbpUsers_UserId",
                table: "ResourceRequestCVs");

            migrationBuilder.DropIndex(
                name: "IX_ResourceRequestCVs_UserId",
                table: "ResourceRequestCVs");
        }
    }
}
