using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_ProjectUserOnboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUserOnboardings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProjectUserId = table.Column<long>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    ChecklistJson = table.Column<string>(nullable: true),
                    SentRequestTime = table.Column<DateTime>(nullable: true),
                    ConfirmedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserOnboardings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserOnboardings_ProjectUsers_ProjectUserId",
                        column: x => x.ProjectUserId,
                        principalTable: "ProjectUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings",
                column: "ProjectUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUserOnboardings");
        }
    }
}
