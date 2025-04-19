using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyMvcApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mission",
                columns: table => new
                {
                    Missioncode = table.Column<string>(type: "text", nullable: false),
                    Missiondate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mission", x => x.Missioncode);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Unitname = table.Column<string>(type: "text", nullable: false),
                    Unitlocation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Unitname);
                });

            migrationBuilder.CreateTable(
                name: "Personnel",
                columns: table => new
                {
                    Personnelid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Rank = table.Column<string>(type: "text", nullable: true),
                    Unitname = table.Column<string>(type: "text", nullable: true),
                    Contactnumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Joiningdate = table.Column<DateOnly>(type: "date", nullable: true),
                    Emergencycontact = table.Column<string>(type: "text", nullable: true),
                    Bloodgroup = table.Column<string>(type: "text", nullable: true),
                    Weaponassigned = table.Column<string>(type: "text", nullable: true),
                    Dutystatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnel", x => x.Personnelid);
                    table.ForeignKey(
                        name: "FK_Personnel_Unit_Unitname",
                        column: x => x.Unitname,
                        principalTable: "Unit",
                        principalColumn: "Unitname");
                });

            migrationBuilder.CreateTable(
                name: "MissionPersonnel",
                columns: table => new
                {
                    MissioncodesMissioncode = table.Column<string>(type: "text", nullable: false),
                    Personnelid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionPersonnel", x => new { x.MissioncodesMissioncode, x.Personnelid });
                    table.ForeignKey(
                        name: "FK_MissionPersonnel_Mission_MissioncodesMissioncode",
                        column: x => x.MissioncodesMissioncode,
                        principalTable: "Mission",
                        principalColumn: "Missioncode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionPersonnel_Personnel_Personnelid",
                        column: x => x.Personnelid,
                        principalTable: "Personnel",
                        principalColumn: "Personnelid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionPersonnel_Personnelid",
                table: "MissionPersonnel",
                column: "Personnelid");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_Unitname",
                table: "Personnel",
                column: "Unitname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionPersonnel");

            migrationBuilder.DropTable(
                name: "Mission");

            migrationBuilder.DropTable(
                name: "Personnel");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}
