using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class addFieldParentInvoiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentInvoiceId",
                table: "TimesheetProjects",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentInvoiceId",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentInvoiceId",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "ParentInvoiceId",
                table: "Projects");
        }
    }
}
