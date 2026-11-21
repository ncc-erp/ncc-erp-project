using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class AddProjectKnowledgeBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectKnowledgeBases",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    SourceFileIds = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedByUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectKnowledgeBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectKnowledgeBases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectKnowledgeBaseHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    KnowledgeBaseId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdateByUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectKnowledgeBaseHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectKnowledgeBaseHistories_ProjectKnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "ProjectKnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectKnowledgeBases_ProjectId",
                table: "ProjectKnowledgeBases",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectKnowledgeBaseHistories_KnowledgeBaseId_CreateAt_Id",
                table: "ProjectKnowledgeBaseHistories",
                columns: new[] { "KnowledgeBaseId", "CreateAt", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ProjectKnowledgeBaseHistories");
            migrationBuilder.DropTable(name: "ProjectKnowledgeBases");
        }
    }
}
