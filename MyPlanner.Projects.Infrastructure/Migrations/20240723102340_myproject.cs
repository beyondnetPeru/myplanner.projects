using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPlanner.Projects.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class myproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "myplanner-projects");

            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                schema: "myplanner-projects",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimesSent = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                schema: "myplanner-projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Track = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskLevel = table.Column<int>(type: "int", nullable: false),
                    BudgetAmount = table.Column<double>(type: "float", nullable: false),
                    BudgetSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetExtraSymbol = table.Column<double>(type: "float", nullable: false),
                    BudgetExtraAmount = table.Column<double>(type: "float", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "backlogs",
                schema: "myplanner-projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_backlogs_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "myplanner-projects",
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scopes",
                schema: "myplanner-projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scopes_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "myplanner-projects",
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stakeholders",
                schema: "myplanner-projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stakeholders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stakeholders_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "myplanner-projects",
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "features",
                schema: "myplanner-projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BacklogId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_features_backlogs_BacklogId",
                        column: x => x.BacklogId,
                        principalSchema: "myplanner-projects",
                        principalTable: "backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_backlogs_ProjectId",
                schema: "myplanner-projects",
                table: "backlogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_features_BacklogId",
                schema: "myplanner-projects",
                table: "features",
                column: "BacklogId");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_ProjectId",
                schema: "myplanner-projects",
                table: "scopes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_stakeholders_ProjectId",
                schema: "myplanner-projects",
                table: "stakeholders",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "features",
                schema: "myplanner-projects");

            migrationBuilder.DropTable(
                name: "IntegrationEventLog",
                schema: "myplanner-projects");

            migrationBuilder.DropTable(
                name: "scopes",
                schema: "myplanner-projects");

            migrationBuilder.DropTable(
                name: "stakeholders",
                schema: "myplanner-projects");

            migrationBuilder.DropTable(
                name: "backlogs",
                schema: "myplanner-projects");

            migrationBuilder.DropTable(
                name: "projects",
                schema: "myplanner-projects");
        }
    }
}
