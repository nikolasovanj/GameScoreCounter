using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    match_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    played_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_matches", x => x.match_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "match_results",
                columns: table => new
                {
                    match_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    match_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_winner = table.Column<bool>(type: "boolean", nullable: false),
                    total_score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_match_results", x => x.match_result_id);
                    table.ForeignKey(
                        name: "fk_match_results_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "matches",
                        principalColumn: "match_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_match_results_users_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "score_components",
                columns: table => new
                {
                    componet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    match_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_score_components", x => x.componet_id);
                    table.ForeignKey(
                        name: "fk_score_components_match_results_match_result_id",
                        column: x => x.match_result_id,
                        principalTable: "match_results",
                        principalColumn: "match_result_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_match_results_match_id_user_id",
                table: "match_results",
                columns: new[] { "match_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_match_results_user_id",
                table: "match_results",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_game_played_at",
                table: "matches",
                columns: new[] { "game", "played_at" });

            migrationBuilder.CreateIndex(
                name: "ix_score_components_match_result_id_order",
                table: "score_components",
                columns: new[] { "match_result_id", "order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "score_components");

            migrationBuilder.DropTable(
                name: "match_results");

            migrationBuilder.DropTable(
                name: "matches");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
