using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class addTablePositionAndOldPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Positions",
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
                    ShortName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_PositionId",
                table: "AbpUsers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Positions_PositionId",
                table: "AbpUsers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Positions_PositionId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_PositionId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "AbpUsers");
        }
    }
}
