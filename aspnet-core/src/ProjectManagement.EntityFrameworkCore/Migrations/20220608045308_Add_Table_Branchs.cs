using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_Branchs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RenameColumn(
                name: "Branch",
                table: "AbpUsers",
                newName: "BranchOld");

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branchs",
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
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branchs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_BranchId",
                table: "AbpUsers",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Branchs_BranchId",
                table: "AbpUsers",
                column: "BranchId",
                principalTable: "Branchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branchs");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_BranchId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
               name: "BranchOld",
               table: "AbpUsers",
               newName: "Branch");
        }
    }
}
