using Microsoft.EntityFrameworkCore.Migrations;
using Rentacar.Models;

#nullable disable

namespace Rentacar.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tables = new List<string>
            {
                "Araclar", "Blog", "Extralar", "Kaskolar", "Lokasyonlar", "Resim", "Rezervasyon", "Role", "User", "Yorumlar"
            };

            foreach (var table in tables)
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "CreateDate",
                    table: table,
                    type: "TEXT",
                    nullable: false,
                    defaultValue: DateTime.UtcNow); // Sabit bir varsayılan değer kullanıldı

                migrationBuilder.AddColumn<DateTime>(
                    name: "UpdateDate",
                    table: table,
                    type: "TEXT",
                    nullable: false,
                    defaultValue: DateTime.UtcNow); // Sabit bir varsayılan değer kullanıldı
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var tables = new List<string>
            {
                "Araclar", "Blog", "Extralar", "Kaskolar", "Lokasyonlar", "Resim", "Rezervasyon", "Role", "User", "Yorumlar"
            };

            foreach (var table in tables)
            {
                migrationBuilder.DropColumn(
                    name: "CreateDate",
                    table: table);

                migrationBuilder.DropColumn(
                    name: "UpdateDate",
                    table: table);
            }
        }
    }
}
