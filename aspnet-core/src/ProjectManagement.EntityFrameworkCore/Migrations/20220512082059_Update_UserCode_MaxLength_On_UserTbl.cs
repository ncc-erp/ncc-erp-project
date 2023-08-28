using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Update_UserCode_MaxLength_On_UserTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AbpUsers",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserCode",
                table: "AbpUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
