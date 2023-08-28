using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class add_timesheetId_into_ProjectTimesheetBill_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimesheetId",
                table: "TimesheetProjectBills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjectBills_TimesheetId",
                table: "TimesheetProjectBills",
                column: "TimesheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetProjectBills_Timesheets_TimesheetId",
                table: "TimesheetProjectBills",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetProjectBills_Timesheets_TimesheetId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetProjectBills_TimesheetId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "TimesheetId",
                table: "TimesheetProjectBills");
        }
    }
}
