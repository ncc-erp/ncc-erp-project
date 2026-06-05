using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_OffboardUser_And_ProjectAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OffboardUsers",
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
                    UserId = table.Column<long>(maxLength: 255, nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    ProjectRole = table.Column<int>(nullable: false),
                    CheckOffboardStatus = table.Column<int>(nullable: false),
                    OffboardChecklistJson = table.Column<string>(nullable: true),
                    HistoryAsset = table.Column<string>(nullable: true),
                    OffboardStatus = table.Column<int>(nullable: false),
                    OffboardDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffboardUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffboardUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OffboardUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAssets",
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
                    ProjectId = table.Column<long>(nullable: false),
                    AssetName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAssets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserAssets",
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
                    UserId = table.Column<long>(nullable: false),
                    ProjectAssetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserAssets_ProjectAssets_ProjectAssetId",
                        column: x => x.ProjectAssetId,
                        principalTable: "ProjectAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserAssets_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OffboardUsers_ProjectId",
                table: "OffboardUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OffboardUsers_UserId",
                table: "OffboardUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssets_ProjectId",
                table: "ProjectAssets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserAssets_ProjectAssetId",
                table: "ProjectUserAssets",
                column: "ProjectAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserAssets_UserId",
                table: "ProjectUserAssets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OffboardUsers");

            migrationBuilder.DropTable(
                name: "ProjectUserAssets");

            migrationBuilder.DropTable(
                name: "ProjectAssets");
        }
    }
}
