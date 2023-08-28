using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_PM_CheckPointUserResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserResults_PMId",
                table: "CheckPointUserResults",
                column: "PMId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckPointUserResults_AbpUsers_PMId",
                table: "CheckPointUserResults",
                column: "PMId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckPointUserResults_AbpUsers_PMId",
                table: "CheckPointUserResults");

            migrationBuilder.DropIndex(
                name: "IX_CheckPointUserResults_PMId",
                table: "CheckPointUserResults");
        }
    }
}
