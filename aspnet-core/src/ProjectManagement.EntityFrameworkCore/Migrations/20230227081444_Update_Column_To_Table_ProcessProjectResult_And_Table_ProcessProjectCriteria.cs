using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Update_Column_To_Table_ProcessProjectResult_And_Table_ProcessProjectCriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditDate",
                table: "ProjectProcessResults");

            migrationBuilder.DropColumn(
                name: "IsApply",
                table: "ProjectProcessCriterias");

            migrationBuilder.AddColumn<long>(
                name: "PMId",
                table: "ProjectProcessResults",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PMId",
                table: "ProjectProcessResults");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuditDate",
                table: "ProjectProcessResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApply",
                table: "ProjectProcessCriterias",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
