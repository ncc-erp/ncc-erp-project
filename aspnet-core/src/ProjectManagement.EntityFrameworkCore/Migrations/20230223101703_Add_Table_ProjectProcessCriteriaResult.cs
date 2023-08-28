using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_ProjectProcessCriteriaResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectProcessCriteriaResults",
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
                    ProjectProcessResultId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    ProcessCriteriaId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProcessCriteriaResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProcessCriteriaResults_ProcessCriterias_ProcessCriteriaId",
                        column: x => x.ProcessCriteriaId,
                        principalTable: "ProcessCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProcessCriteriaResults_ProjectProcessResults_ProjectProcessResultId",
                        column: x => x.ProjectProcessResultId,
                        principalTable: "ProjectProcessResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProcessCriteriaResults_ProcessCriteriaId",
                table: "ProjectProcessCriteriaResults",
                column: "ProcessCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProcessCriteriaResults_ProjectProcessResultId",
                table: "ProjectProcessCriteriaResults",
                column: "ProjectProcessResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProcessCriteriaResults");
        }
    }
}
