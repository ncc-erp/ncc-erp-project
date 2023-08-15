using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Change_column_to_Client_Project_ProjectUserBill_TimesheetProject_TimesheetProjectBill_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ProjectUserBills");

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "TimesheetProjects",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceNumber",
                table: "TimesheetProjects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<float>(
                name: "TransferFee",
                table: "TimesheetProjects",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "WorkingDay",
                table: "TimesheetProjects",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ChargeType",
                table: "TimesheetProjectBills",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "TimesheetProjectBills",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "Projects",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<long>(
                name: "LastInvoiceNumber",
                table: "Projects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "InvoicePaymentInfo",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "InvoiceDateSetting",
                table: "Clients",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "PaymentDueBy",
                table: "Clients",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<float>(
                name: "TransferFee",
                table: "Clients",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjectBills_CurrencyId",
                table: "TimesheetProjectBills",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetProjectBills_Currencies_CurrencyId",
                table: "TimesheetProjectBills",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetProjectBills_Currencies_CurrencyId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropIndex(
                name: "IX_TimesheetProjectBills_CurrencyId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "TransferFee",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "WorkingDay",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "ChargeType",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "TimesheetProjectBills");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LastInvoiceNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InvoicePaymentInfo",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "InvoiceDateSetting",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PaymentDueBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TransferFee",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "TimesheetProjectBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "ProjectUserBills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
