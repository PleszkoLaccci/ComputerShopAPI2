using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerShopAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEverything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OSystems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OSystems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Comps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Brand = table.Column<string>(type: "varchar(37)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Display = table.Column<float>(type: "float", nullable: false),
                    Memory = table.Column<int>(type: "int(7)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    OsId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comps_OSystems_OsId",
                        column: x => x.OsId,
                        principalTable: "OSystems",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Comps_OsId",
                table: "Comps",
                column: "OsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comps");

            migrationBuilder.DropTable(
                name: "OSystems");
        }
    }
}
