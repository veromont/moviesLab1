using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace moviesAPI.Migrations.Cinema
{
    public partial class addMoviePlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "halls",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    isAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_halls", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client_genre",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_genre", x => x.id);
                    table.ForeignKey(
                        name: "FK_client_genre_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    director = table.Column<string>(type: "text", nullable: false),
                    releaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: false),
                    duration = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    genreId = table.Column<int>(type: "integer", nullable: true),
                    plot = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.id);
                    table.ForeignKey(
                        name: "FK_movies_genres_genreId",
                        column: x => x.genreId,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    movieId = table.Column<Guid>(type: "uuid", nullable: false),
                    hallId = table.Column<int>(type: "integer", nullable: false),
                    startTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_halls_hallId",
                        column: x => x.hallId,
                        principalTable: "halls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessions_movies_movieId",
                        column: x => x.movieId,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    seatNumber = table.Column<int>(type: "integer", nullable: false),
                    USerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_sessions_sessionId",
                        column: x => x.sessionId,
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_genre_genre_id",
                table: "client_genre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_movies_genreId",
                table: "movies",
                column: "genreId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_hallId",
                table: "sessions",
                column: "hallId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_movieId",
                table: "sessions",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_sessionId",
                table: "tickets",
                column: "sessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_genre");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "halls");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "genres");
        }
    }
}
