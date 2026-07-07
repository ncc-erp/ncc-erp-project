using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class UpdateProjectUserOnboardingUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings",
                column: "ProjectUserId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings",
                column: "ProjectUserId",
                unique: true);
        }
    }
}
