using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jaiz_CSAT_Survey.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    SurveyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Staff = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceSatisfaction = table.Column<byte>(type: "tinyint", nullable: true),
                    StaffProfessionalism = table.Column<byte>(type: "tinyint", nullable: true),
                    BranchAmbience = table.Column<byte>(type: "tinyint", nullable: true),
                    RecommendationLikelihood = table.Column<byte>(type: "tinyint", nullable: true),
                    WebRating = table.Column<byte>(type: "tinyint", nullable: true),
                    IssueResolution = table.Column<byte>(type: "tinyint", nullable: true),
                    TransactionEase = table.Column<byte>(type: "tinyint", nullable: true),
                    ProductRating = table.Column<byte>(type: "tinyint", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurveyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.SurveyId);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyAlerts_SurveyResponses_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "SurveyResponses",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAlerts_SurveyId",
                table: "SurveyAlerts",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyAlerts");

            migrationBuilder.DropTable(
                name: "SurveyResponses");
        }
    }
}
