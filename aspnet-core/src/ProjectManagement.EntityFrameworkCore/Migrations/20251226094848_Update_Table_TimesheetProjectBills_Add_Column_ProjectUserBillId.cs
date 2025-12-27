using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Update_Table_TimesheetProjectBills_Add_Column_ProjectUserBillId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProjectUserBillId",
                table: "TimesheetProjectBills",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjectBills_ProjectUserBillId",
                table: "TimesheetProjectBills",
                column: "ProjectUserBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetProjectBills_ProjectUserBills_ProjectUserBillId",
                table: "TimesheetProjectBills",
                column: "ProjectUserBillId",
                principalTable: "ProjectUserBills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetProjectBills_ProjectUserBills_ProjectUserBillId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetProjectBills_ProjectUserBillId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "ProjectUserBillId",
                table: "TimesheetProjectBills");
        }
    }
}
