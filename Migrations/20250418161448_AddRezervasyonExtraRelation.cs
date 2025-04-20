using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentacar.Migrations
{
    /// <inheritdoc />
    public partial class AddRezervasyonExtraRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RezervasyonExtralar",
                columns: table => new
                {
                    RezervasyonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervasyonExtralar", x => new { x.RezervasyonId, x.ExtraId });
                    table.ForeignKey(
                        name: "FK_RezervasyonExtralar_Extralar_ExtraId",
                        column: x => x.ExtraId,
                        principalTable: "Extralar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervasyonExtralar_Rezervasyon_RezervasyonId",
                        column: x => x.RezervasyonId,
                        principalTable: "Rezervasyon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RezervasyonExtralar_ExtraId",
                table: "RezervasyonExtralar",
                column: "ExtraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RezervasyonExtralar");

        }
    }
}
