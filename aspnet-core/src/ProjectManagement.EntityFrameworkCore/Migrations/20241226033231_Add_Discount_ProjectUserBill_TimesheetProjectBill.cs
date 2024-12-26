using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Discount_ProjectUserBill_TimesheetProjectBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "TimesheetProjectBills",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "ProjectUserBills",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ProjectUserBills");
        }
    }
}
