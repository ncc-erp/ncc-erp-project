using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_ProjectProcessCriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectProcessCriterias",
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
                    ProcessCriteriaId = table.Column<long>(nullable: false),
                    IsApply = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProcessCriterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProcessCriterias_ProcessCriterias_ProcessCriteriaId",
                        column: x => x.ProcessCriteriaId,
                        principalTable: "ProcessCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProcessCriterias_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProcessCriterias_ProcessCriteriaId",
                table: "ProjectProcessCriterias",
                column: "ProcessCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProcessCriterias_ProjectId",
                table: "ProjectProcessCriterias",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProcessCriterias");
        }
    }
}
