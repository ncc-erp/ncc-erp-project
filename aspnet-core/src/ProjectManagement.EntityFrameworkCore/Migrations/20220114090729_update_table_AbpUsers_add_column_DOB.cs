using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class update_table_AbpUsers_add_column_DOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "AbpUsers");
        }
    }
}
