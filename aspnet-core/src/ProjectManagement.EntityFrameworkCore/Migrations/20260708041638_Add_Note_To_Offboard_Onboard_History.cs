using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Note_To_Offboard_Onboard_History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ProjectUserOnboardings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnboardDate",
                table: "ProjectUserOnboardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OffboardUsers",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ProjectUserOnboardings");

            migrationBuilder.DropColumn(
                name: "OnboardDate",
                table: "ProjectUserOnboardings");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OffboardUsers");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings",
                column: "ProjectUserId",
                unique: true);
        }
    }
}
