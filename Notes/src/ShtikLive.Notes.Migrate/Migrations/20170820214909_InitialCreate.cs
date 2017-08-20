using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShtikLive.Notes.Migrate.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NoteText = table.Column<string>(type: "text", nullable: true),
                    Public = table.Column<bool>(type: "bool", nullable: false),
                    ShowId = table.Column<int>(type: "int4", nullable: false),
                    SlideNumber = table.Column<int>(type: "int4", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UserHandle = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");
        }
    }
}
