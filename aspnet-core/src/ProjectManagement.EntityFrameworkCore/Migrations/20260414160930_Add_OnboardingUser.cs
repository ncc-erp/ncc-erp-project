using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_OnboardingUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnboardingChecklists",
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
                    Label = table.Column<string>(nullable: false),
                    DetailsJson = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingChecklists", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ProjectUserOnboardingDetails",
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
                    ProjectUserOnboardingId = table.Column<long>(nullable: false),
                    OnboardingChecklistId = table.Column<long>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserOnboardingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserOnboardingDetails_OnboardingChecklists_OnboardingChecklistId",
                        column: x => x.OnboardingChecklistId,
                        principalTable: "OnboardingChecklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserOnboardingDetails_ProjectUserOnboardings_ProjectUserOnboardingId",
                        column: x => x.ProjectUserOnboardingId,
                        principalTable: "ProjectUserOnboardings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardingDetails_OnboardingChecklistId",
                table: "ProjectUserOnboardingDetails",
                column: "OnboardingChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardingDetails_ProjectUserOnboardingId",
                table: "ProjectUserOnboardingDetails",
                column: "ProjectUserOnboardingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserOnboardings_ProjectUserId",
                table: "ProjectUserOnboardings",
                column: "ProjectUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUserOnboardingDetails");

            migrationBuilder.DropTable(
                name: "OnboardingChecklists");

            migrationBuilder.DropTable(
                name: "ProjectUserOnboardings");
        }
    }
}
