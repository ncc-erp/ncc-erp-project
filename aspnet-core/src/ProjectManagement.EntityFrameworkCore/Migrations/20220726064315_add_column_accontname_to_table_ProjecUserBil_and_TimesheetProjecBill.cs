using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_column_accontname_to_table_ProjecUserBil_and_TimesheetProjecBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "TimesheetProjectBills",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "ProjectUserBills",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "ProjectUserBills");
        }
    }
}
