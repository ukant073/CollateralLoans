using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CollateralManagementApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collateral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialAssesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAssessDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collateral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Land",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AreaInSqFt = table.Column<double>(type: "float", nullable: false),
                    InitialPricePerSqFt = table.Column<double>(type: "float", nullable: false),
                    CurrentPricePerSqFt = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Land", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Land_Collateral_Id",
                        column: x => x.Id,
                        principalTable: "Collateral",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RealEstate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    YearBuilt = table.Column<int>(type: "int", nullable: false),
                    AreaInSqFt = table.Column<double>(type: "float", nullable: false),
                    InitialLandPriceInSqFt = table.Column<double>(type: "float", nullable: false),
                    CurrentLandPriceInSqFt = table.Column<double>(type: "float", nullable: false),
                    InitialStructurePrice = table.Column<double>(type: "float", nullable: false),
                    CurrentStructurePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealEstate_Collateral_Id",
                        column: x => x.Id,
                        principalTable: "Collateral",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Land");

            migrationBuilder.DropTable(
                name: "RealEstate");

            migrationBuilder.DropTable(
                name: "Collateral");
        }
    }
}
