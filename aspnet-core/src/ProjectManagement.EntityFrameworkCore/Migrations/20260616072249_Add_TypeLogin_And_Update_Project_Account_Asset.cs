using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class Add_TypeLogin_And_Update_Project_Account_Asset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountAssets_AccountAssetCreators_AccountAssetCreatorId",
                table: "AccountAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountAssets_AccountTypes_AccountTypeId",
                table: "AccountAssets");

            migrationBuilder.DropIndex(
                name: "IX_AccountAssets_AccountAssetCreatorId",
                table: "AccountAssets");

            migrationBuilder.DropIndex(
                name: "IX_AccountAssets_AccountTypeId",
                table: "AccountAssets");

            migrationBuilder.DropColumn(
                name: "AccountAssetCreatorId",
                table: "AccountAssets");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "AccountAssets");

            migrationBuilder.DropColumn(
                name: "AssetName",
                table: "AccountAssets");

            migrationBuilder.DropColumn(
                name: "TypeLogin",
                table: "AccountAssets");

            migrationBuilder.AddColumn<long>(
                name: "AccountAssetCreatorId",
                table: "ProjectAssets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AccountTypeId",
                table: "ProjectAssets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TypeLoginId",
                table: "ProjectAssets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProjectAssetId",
                table: "AccountAssets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "TypeLogins",
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
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeLogins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssets_AccountAssetCreatorId",
                table: "ProjectAssets",
                column: "AccountAssetCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssets_AccountTypeId",
                table: "ProjectAssets",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssets_TypeLoginId",
                table: "ProjectAssets",
                column: "TypeLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAssets_ProjectAssetId",
                table: "AccountAssets",
                column: "ProjectAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountAssets_ProjectAssets_ProjectAssetId",
                table: "AccountAssets",
                column: "ProjectAssetId",
                principalTable: "ProjectAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAssets_AccountAssetCreators_AccountAssetCreatorId",
                table: "ProjectAssets",
                column: "AccountAssetCreatorId",
                principalTable: "AccountAssetCreators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAssets_AccountTypes_AccountTypeId",
                table: "ProjectAssets",
                column: "AccountTypeId",
                principalTable: "AccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAssets_TypeLogins_TypeLoginId",
                table: "ProjectAssets",
                column: "TypeLoginId",
                principalTable: "TypeLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountAssets_ProjectAssets_ProjectAssetId",
                table: "AccountAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAssets_AccountAssetCreators_AccountAssetCreatorId",
                table: "ProjectAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAssets_AccountTypes_AccountTypeId",
                table: "ProjectAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAssets_TypeLogins_TypeLoginId",
                table: "ProjectAssets");

            migrationBuilder.DropTable(
                name: "TypeLogins");

            migrationBuilder.DropIndex(
                name: "IX_ProjectAssets_AccountAssetCreatorId",
                table: "ProjectAssets");

            migrationBuilder.DropIndex(
                name: "IX_ProjectAssets_AccountTypeId",
                table: "ProjectAssets");

            migrationBuilder.DropIndex(
                name: "IX_ProjectAssets_TypeLoginId",
                table: "ProjectAssets");

            migrationBuilder.DropIndex(
                name: "IX_AccountAssets_ProjectAssetId",
                table: "AccountAssets");

            migrationBuilder.DropColumn(
                name: "AccountAssetCreatorId",
                table: "ProjectAssets");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "ProjectAssets");

            migrationBuilder.DropColumn(
                name: "TypeLoginId",
                table: "ProjectAssets");

            migrationBuilder.DropColumn(
                name: "ProjectAssetId",
                table: "AccountAssets");

            migrationBuilder.AddColumn<long>(
                name: "AccountAssetCreatorId",
                table: "AccountAssets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AccountTypeId",
                table: "AccountAssets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AssetName",
                table: "AccountAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeLogin",
                table: "AccountAssets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountAssets_AccountAssetCreatorId",
                table: "AccountAssets",
                column: "AccountAssetCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAssets_AccountTypeId",
                table: "AccountAssets",
                column: "AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountAssets_AccountAssetCreators_AccountAssetCreatorId",
                table: "AccountAssets",
                column: "AccountAssetCreatorId",
                principalTable: "AccountAssetCreators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountAssets_AccountTypes_AccountTypeId",
                table: "AccountAssets",
                column: "AccountTypeId",
                principalTable: "AccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
