using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDigital.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "angajati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pozitie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salariu = table.Column<double>(type: "float", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirareContract = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_angajati", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacitate = table.Column<int>(type: "int", nullable: false),
                    AnFabricatie = table.Column<int>(type: "int", nullable: false),
                    DataAchizitie = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PretAchizitie = table.Column<double>(type: "float", nullable: false),
                    UltimaMentenanta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipAtasament = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResetPaswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordExpiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "utilaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PutereNecesara = table.Column<int>(type: "int", nullable: false),
                    Greutate = table.Column<int>(type: "int", nullable: false),
                    AnFabricatie = table.Column<int>(type: "int", nullable: false),
                    OreFunctionare = table.Column<int>(type: "int", nullable: false),
                    DataAchizitie = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PretAchizitie = table.Column<double>(type: "float", nullable: false),
                    UltimaMentenanta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilaje", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehicule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Putere = table.Column<int>(type: "int", nullable: false),
                    OreFunctionare = table.Column<int>(type: "int", nullable: false),
                    AnFabricatie = table.Column<int>(type: "int", nullable: false),
                    DataAchizitie = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PretAchizitie = table.Column<double>(type: "float", nullable: false),
                    UltimaMentenanta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drawings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StareTeren = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipCultura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<double>(type: "float", nullable: false),
                    DateAcquired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaCultura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProprietarArenda = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drawings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drawings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_drawings_UserId",
                table: "drawings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "angajati");

            migrationBuilder.DropTable(
                name: "drawings");

            migrationBuilder.DropTable(
                name: "Transport");

            migrationBuilder.DropTable(
                name: "utilaje");

            migrationBuilder.DropTable(
                name: "vehicule");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
