using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Linky.Migrations
{
    /// <inheritdoc />
    public partial class Aliases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aliases",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", maxLength: 22, nullable: false),
                    LinkUid = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aliases", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Aliases_Links_LinkUid",
                        column: x => x.LinkUid,
                        principalTable: "Links",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aliases_LinkUid",
                table: "Aliases",
                column: "LinkUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aliases");
        }
    }
}
