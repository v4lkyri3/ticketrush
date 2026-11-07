using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketRush.Modules.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "venues",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VenueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_events_venues_VenueId",
                        column: x => x.VenueId,
                        principalSchema: "catalog",
                        principalTable: "venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venue_seats",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VenueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Section = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Row = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_venue_seats_venues_VenueId",
                        column: x => x.VenueId,
                        principalSchema: "catalog",
                        principalTable: "venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_Status_StartsAtUtc",
                schema: "catalog",
                table: "events",
                columns: new[] { "Status", "StartsAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_events_VenueId",
                schema: "catalog",
                table: "events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_venue_seats_VenueId",
                schema: "catalog",
                table: "venue_seats",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "venue_seats",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "venues",
                schema: "catalog");
        }
    }
}
