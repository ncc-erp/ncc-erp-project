using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Delete_IsTempRequestId_TblProjectUserBill_AddBillAcountId_BillStartDate_TblResourceRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUserBills_ResourceRequests_ResourceRequestId",
                table: "ProjectUserBills");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUserBills_ResourceRequestId",
                table: "ProjectUserBills");

            migrationBuilder.DropColumn(
                name: "IsTemp",
                table: "ProjectUserBills");

            migrationBuilder.DropColumn(
                name: "ResourceRequestId",
                table: "ProjectUserBills");

            migrationBuilder.AddColumn<long>(
                name: "BillAccountId",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BillStartDate",
                table: "ResourceRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillAccountId",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "BillStartDate",
                table: "ResourceRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsTemp",
                table: "ProjectUserBills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ResourceRequestId",
                table: "ProjectUserBills",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserBills_ResourceRequestId",
                table: "ProjectUserBills",
                column: "ResourceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUserBills_ResourceRequests_ResourceRequestId",
                table: "ProjectUserBills",
                column: "ResourceRequestId",
                principalTable: "ResourceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
