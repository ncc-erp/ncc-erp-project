using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class addstatusPMReportandisPunishPMReportProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "PMReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte>(
                name: "PMReportStatus",
                table: "PMReports",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPunish",
                table: "PMReportProjects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PMReportStatus",
                table: "PMReports");

            migrationBuilder.DropColumn(
                name: "IsPunish",
                table: "PMReportProjects");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "PMReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
