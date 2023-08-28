using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class init_DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditSessions",
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
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckListCategories",
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
                    Name = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
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
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaCategories",
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
                    table.PrimaryKey("PK_CriteriaCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phases",
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
                    Name = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PMReports",
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
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
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
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
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
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
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
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
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
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(maxLength: 10000, nullable: true),
                    AuditTarget = table.Column<string>(maxLength: 255, nullable: true),
                    PersonInCharge = table.Column<string>(maxLength: 255, nullable: true),
                    Note = table.Column<string>(maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItems_CheckListCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CheckListCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
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
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    ProjectType = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsCharge = table.Column<bool>(nullable: false),
                    PMId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_AbpUsers_PMId",
                        column: x => x.PMId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Criterias",
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
                    Name = table.Column<string>(nullable: true),
                    CriteriaCategoryId = table.Column<long>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criterias_CriteriaCategories_CriteriaCategoryId",
                        column: x => x.CriteriaCategoryId,
                        principalTable: "CriteriaCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckPointUserResults",
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
                    PhaseId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    UserNote = table.Column<string>(nullable: true),
                    PMNote = table.Column<string>(nullable: true),
                    FinalNote = table.Column<string>(nullable: true),
                    OldLevel = table.Column<byte>(nullable: false),
                    NewLevel = table.Column<byte>(nullable: false),
                    PMScore = table.Column<int>(nullable: true),
                    TeamScore = table.Column<int>(nullable: true),
                    ClientScore = table.Column<int>(nullable: true),
                    SelfScore = table.Column<int>(nullable: true),
                    FinalScore = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPointUserResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPointUserResults_Phases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPointUserResults_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckPointUsers",
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
                    PhaseId = table.Column<long>(nullable: false),
                    ReviewerId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPointUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPointUsers_Phases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPointUsers_AbpUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPointUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
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
                    SkillId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSkills_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItemMandatorys",
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
                    CheckListItemId = table.Column<long>(nullable: false),
                    ProjectType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItemMandatorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItemMandatorys_CheckListItems_CheckListItemId",
                        column: x => x.CheckListItemId,
                        principalTable: "CheckListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditResults",
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
                    AuditSessionId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    Note = table.Column<string>(maxLength: 10000, nullable: true),
                    PMId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResults_AuditSessions_AuditSessionId",
                        column: x => x.AuditSessionId,
                        principalTable: "AuditSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditResults_AbpUsers_PMId",
                        column: x => x.PMId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditResults_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMReportProjects",
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
                    PMReportId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProjectHealth = table.Column<int>(nullable: false),
                    PMId = table.Column<long>(nullable: false),
                    Note = table.Column<string>(maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMReportProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMReportProjects_AbpUsers_PMId",
                        column: x => x.PMId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMReportProjects_PMReports_PMReportId",
                        column: x => x.PMReportId,
                        principalTable: "PMReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PMReportProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCheckLists",
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
                    ProjectId = table.Column<long>(nullable: false),
                    CheckListItemId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCheckLists_CheckListItems_CheckListItemId",
                        column: x => x.CheckListItemId,
                        principalTable: "CheckListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectCheckLists_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMilestones",
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
                    ProjectId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 10000, nullable: true),
                    Flag = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UATTimeStart = table.Column<DateTime>(nullable: true),
                    UATTimeEnd = table.Column<DateTime>(nullable: true),
                    Note = table.Column<string>(maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMilestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMilestones_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserBills",
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
                    UserId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    BillRole = table.Column<string>(nullable: true),
                    BillRate = table.Column<float>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserBills_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUserBills_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRequests",
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
                    Name = table.Column<string>(maxLength: 1000, nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    TimeNeed = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TimeDone = table.Column<DateTime>(nullable: true),
                    Note = table.Column<string>(maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetProjects",
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
                    ProjectId = table.Column<long>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 1000, nullable: true),
                    TimesheetId = table.Column<long>(nullable: false),
                    Note = table.Column<string>(maxLength: 10000, nullable: true),
                    ProjectBillInfomation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesheetProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimesheetProjects_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckPointUserResultTags",
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
                    TagId = table.Column<long>(nullable: false),
                    CheckPointUserResultId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPointUserResultTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPointUserResultTags_CheckPointUserResults_CheckPointUserResultId",
                        column: x => x.CheckPointUserResultId,
                        principalTable: "CheckPointUserResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPointUserResultTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckPointUserDetails",
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
                    CheckPointUserId = table.Column<long>(nullable: false),
                    CriteriaId = table.Column<long>(nullable: false),
                    Score = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPointUserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPointUserDetails_CheckPointUsers_CheckPointUserId",
                        column: x => x.CheckPointUserId,
                        principalTable: "CheckPointUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckPointUserDetails_Criterias_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditResultPeoples",
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
                    AuditResultId = table.Column<long>(nullable: false),
                    CheckListItemId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Note = table.Column<string>(maxLength: 10000, nullable: true),
                    IsPass = table.Column<bool>(nullable: false),
                    CuratorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResultPeoples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResultPeoples_AuditResults_AuditResultId",
                        column: x => x.AuditResultId,
                        principalTable: "AuditResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditResultPeoples_CheckListItems_CheckListItemId",
                        column: x => x.CheckListItemId,
                        principalTable: "CheckListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditResultPeoples_AbpUsers_CuratorId",
                        column: x => x.CuratorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditResultPeoples_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PMReportProjectIssues",
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
                    PMReportProjectId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(maxLength: 10000, nullable: true),
                    Impact = table.Column<string>(maxLength: 10000, nullable: true),
                    Critical = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Solution = table.Column<string>(maxLength: 10000, nullable: true),
                    MeetingSolution = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMReportProjectIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMReportProjectIssues_PMReportProjects_PMReportProjectId",
                        column: x => x.PMReportProjectId,
                        principalTable: "PMReportProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
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
                    UserId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    ProjectRole = table.Column<int>(nullable: false),
                    AllocatePercentage = table.Column<byte>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsExpense = table.Column<bool>(nullable: false),
                    ResourceRequestId = table.Column<long>(nullable: true),
                    PMReportId = table.Column<long>(nullable: false),
                    IsFutureActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_PMReports_PMReportId",
                        column: x => x.PMReportId,
                        principalTable: "PMReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_ResourceRequests_ResourceRequestId",
                        column: x => x.ResourceRequestId,
                        principalTable: "ResourceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditResultPeoples_AuditResultId",
                table: "AuditResultPeoples",
                column: "AuditResultId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResultPeoples_CheckListItemId",
                table: "AuditResultPeoples",
                column: "CheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResultPeoples_CuratorId",
                table: "AuditResultPeoples",
                column: "CuratorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResultPeoples_UserId",
                table: "AuditResultPeoples",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResults_AuditSessionId",
                table: "AuditResults",
                column: "AuditSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResults_PMId",
                table: "AuditResults",
                column: "PMId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResults_ProjectId",
                table: "AuditResults",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItemMandatorys_CheckListItemId",
                table: "CheckListItemMandatorys",
                column: "CheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_CategoryId",
                table: "CheckListItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserDetails_CheckPointUserId",
                table: "CheckPointUserDetails",
                column: "CheckPointUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserDetails_CriteriaId",
                table: "CheckPointUserDetails",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserResults_PhaseId",
                table: "CheckPointUserResults",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserResults_UserId",
                table: "CheckPointUserResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserResultTags_CheckPointUserResultId",
                table: "CheckPointUserResultTags",
                column: "CheckPointUserResultId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUserResultTags_TagId",
                table: "CheckPointUserResultTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUsers_PhaseId",
                table: "CheckPointUsers",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUsers_ReviewerId",
                table: "CheckPointUsers",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPointUsers_UserId",
                table: "CheckPointUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Criterias_CriteriaCategoryId",
                table: "Criterias",
                column: "CriteriaCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PMReportProjectIssues_PMReportProjectId",
                table: "PMReportProjectIssues",
                column: "PMReportProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PMReportProjects_PMId",
                table: "PMReportProjects",
                column: "PMId");

            migrationBuilder.CreateIndex(
                name: "IX_PMReportProjects_PMReportId",
                table: "PMReportProjects",
                column: "PMReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PMReportProjects_ProjectId",
                table: "PMReportProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCheckLists_CheckListItemId",
                table: "ProjectCheckLists",
                column: "CheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCheckLists_ProjectId",
                table: "ProjectCheckLists",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMilestones_ProjectId",
                table: "ProjectMilestones",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PMId",
                table: "Projects",
                column: "PMId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserBills_ProjectId",
                table: "ProjectUserBills",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserBills_UserId",
                table: "ProjectUserBills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_PMReportId",
                table: "ProjectUsers",
                column: "PMReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ResourceRequestId",
                table: "ProjectUsers",
                column: "ResourceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRequests_ProjectId",
                table: "ResourceRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjects_ProjectId",
                table: "TimesheetProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetProjects_TimesheetId",
                table: "TimesheetProjects",
                column: "TimesheetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkills",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditResultPeoples");

            migrationBuilder.DropTable(
                name: "CheckListItemMandatorys");

            migrationBuilder.DropTable(
                name: "CheckPointUserDetails");

            migrationBuilder.DropTable(
                name: "CheckPointUserResultTags");

            migrationBuilder.DropTable(
                name: "PMReportProjectIssues");

            migrationBuilder.DropTable(
                name: "ProjectCheckLists");

            migrationBuilder.DropTable(
                name: "ProjectMilestones");

            migrationBuilder.DropTable(
                name: "ProjectUserBills");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "TimesheetProjects");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "AuditResults");

            migrationBuilder.DropTable(
                name: "CheckPointUsers");

            migrationBuilder.DropTable(
                name: "Criterias");

            migrationBuilder.DropTable(
                name: "CheckPointUserResults");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "PMReportProjects");

            migrationBuilder.DropTable(
                name: "CheckListItems");

            migrationBuilder.DropTable(
                name: "ResourceRequests");

            migrationBuilder.DropTable(
                name: "Timesheets");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "AuditSessions");

            migrationBuilder.DropTable(
                name: "CriteriaCategories");

            migrationBuilder.DropTable(
                name: "Phases");

            migrationBuilder.DropTable(
                name: "PMReports");

            migrationBuilder.DropTable(
                name: "CheckListCategories");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
