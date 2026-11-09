using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    follower_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_follows", x => new { x.follower_id, x.followee_id });
                    table.CheckConstraint("ck_follow_not_self", "\"follower_id\" <> \"followee_id\"");
                    table.ForeignKey(
                        name: "fk_follows_users_followee_id",
                        column: x => x.followee_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_follows_users_follower_id",
                        column: x => x.follower_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    match_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    match_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedules", x => x.schedule_id);
                    table.ForeignKey(
                        name: "fk_schedules_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "matches",
                        principalColumn: "match_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_schedules_users_organizer_id",
                        column: x => x.organizer_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invitations",
                columns: table => new
                {
                    invitation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invitations", x => x.invitation_id);
                    table.ForeignKey(
                        name: "fk_invitations_schedules_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedules",
                        principalColumn: "schedule_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invitations_users_player_id",
                        column: x => x.player_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_follows_followee_id",
                table: "follows",
                column: "followee_id");

            migrationBuilder.CreateIndex(
                name: "ix_invitations_player_id_status",
                table: "invitations",
                columns: new[] { "player_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_invitations_schedule_id_player_id",
                table: "invitations",
                columns: new[] { "schedule_id", "player_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_schedules_match_id",
                table: "schedules",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "ix_schedules_match_time",
                table: "schedules",
                column: "match_time");

            migrationBuilder.CreateIndex(
                name: "ix_schedules_organizer_id_match_time",
                table: "schedules",
                columns: new[] { "organizer_id", "match_time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "follows");

            migrationBuilder.DropTable(
                name: "invitations");

            migrationBuilder.DropTable(
                name: "schedules");
        }
    }
}
