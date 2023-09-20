using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Code_BillAccId_BillStartDate_TblResourceRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BillAccountId",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BillStartDate",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ResourceRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequests_BillAccountId",
                table: "ResourceRequests",
                column: "BillAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceRequests_AbpUsers_BillAccountId",
                table: "ResourceRequests",
                column: "BillAccountId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceRequests_AbpUsers_BillAccountId",
                table: "ResourceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ResourceRequests_BillAccountId",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TimesheetProjects");

            migrationBuilder.DropColumn(
                name: "BillAccountId",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "BillStartDate",
                table: "ResourceRequests");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ResourceRequests");
        }
    }
}
