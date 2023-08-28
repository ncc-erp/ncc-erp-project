using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_column_Phase_CheckPointUserResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Phases");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Phases",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Phases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCriteria",
                table: "Phases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ExamScore",
                table: "CheckPointUserResults",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PMId",
                table: "CheckPointUserResults",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "IsCriteria",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "ExamScore",
                table: "CheckPointUserResults");

            migrationBuilder.DropColumn(
                name: "PMId",
                table: "CheckPointUserResults");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Phases",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Phases",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
