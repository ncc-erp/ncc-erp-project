using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_MonthlyUserContribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyUserContributions",
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
                    UserId = table.Column<long>(nullable: false),
                    BranchId = table.Column<long>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    Contribute = table.Column<byte>(nullable: false),
                    UserLevel = table.Column<byte>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    MonthTime = table.Column<byte>(nullable: false),
                    YearTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyUserContributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyUserContributions_Branchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MonthlyUserContributions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyUserContributions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyUserContributions_BranchId",
                table: "MonthlyUserContributions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyUserContributions_ProjectId",
                table: "MonthlyUserContributions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyUserContributions_UserId",
                table: "MonthlyUserContributions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyUserContributions");
        }
    }
}
