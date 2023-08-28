using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Create_table_ResourceRequestSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceRequestSkills",
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
                    ResourceRequestId = table.Column<long>(nullable: false),
                    Quantity = table.Column<long>(nullable: false),
                    SkillId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRequestSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceRequestSkills_ResourceRequests_ResourceRequestId",
                        column: x => x.ResourceRequestId,
                        principalTable: "ResourceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceRequestSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequestSkills_ResourceRequestId",
                table: "ResourceRequestSkills",
                column: "ResourceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequestSkills_SkillId",
                table: "ResourceRequestSkills",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceRequestSkills");
        }
    }
}
