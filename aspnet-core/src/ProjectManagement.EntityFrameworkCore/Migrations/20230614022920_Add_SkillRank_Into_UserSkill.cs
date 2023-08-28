using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_SkillRank_Into_UserSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillRank",
                table: "UserSkills",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillRank",
                table: "UserSkills");
        }
    }
}
