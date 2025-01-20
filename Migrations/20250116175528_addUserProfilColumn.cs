using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Rentacar.Models;

#nullable disable

namespace Rentacar.Migrations
{
    /// <inheritdoc />
    public partial class addUserProfilColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                     name: "Profil",
                     table: "User",
                     type: "TEXT",
                     nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
         name: "Profil",
         table: "User");
        }
    }
}
