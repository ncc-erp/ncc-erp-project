using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_Table_TimesheetProjectBillOtTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimesheetProjectBillOtTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ProjectOtTypeId = table.Column<long>(nullable: false),
                    TimesheetProjectBillId = table.Column<long>(nullable: false),
                    Hours = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetProjectBillOtTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesheetProjectBillOtTypes_ProjectOtTypes_ProjectOtTypeId",
                        column: x => x.ProjectOtTypeId,
                        principalTable: "ProjectOtTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimesheetProjectBillOtTypes_TimesheetProjectBills_TimesheetProjectBillId",
                        column: x => x.TimesheetProjectBillId,
                        principalTable: "TimesheetProjectBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjectBillOtTypes_ProjectOtTypeId",
                table: "TimesheetProjectBillOtTypes",
                column: "ProjectOtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjectBillOtTypes_TimesheetProjectBillId",
                table: "TimesheetProjectBillOtTypes",
                column: "TimesheetProjectBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimesheetProjectBillOtTypes");
        }
    }
}
