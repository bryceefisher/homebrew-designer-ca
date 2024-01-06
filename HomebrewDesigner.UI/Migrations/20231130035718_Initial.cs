using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC_Homebrew_Recipe_Designer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fermentables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Origin = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Color = table.Column<double>(type: "double", nullable: false),
                    PotentialGravity = table.Column<double>(type: "double", nullable: false),
                    MaxInBatch = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fermentables", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlphaAcid = table.Column<double>(type: "double", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hops", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Yeasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lab = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Form = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Flocculation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yeasts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Style = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalGravity = table.Column<double>(type: "double", nullable: true),
                    FinalGravity = table.Column<double>(type: "double", nullable: false),
                    IBU = table.Column<double>(type: "double", nullable: false),
                    ABV = table.Column<double>(type: "double", nullable: false),
                    YeastId = table.Column<int>(type: "int", nullable: false),
                    YeastAmount = table.Column<double>(type: "double", nullable: true),
                    YeastViability = table.Column<double>(type: "double", nullable: true),
                    MashTemp = table.Column<int>(type: "int", nullable: true),
                    WaterRatio = table.Column<double>(type: "double", nullable: true),
                    AmountOfWater = table.Column<double>(type: "double", nullable: true),
                    Color = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Yeasts_YeastId",
                        column: x => x.YeastId,
                        principalTable: "Yeasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FermentablePair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    FermentableId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FermentablePair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FermentablePair_Fermentables_FermentableId",
                        column: x => x.FermentableId,
                        principalTable: "Fermentables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FermentablePair_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HopAddition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Use = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BoilTime = table.Column<int>(type: "int", nullable: true),
                    DryHopDays = table.Column<int>(type: "int", nullable: true),
                    Form = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<double>(type: "double", nullable: true),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    HopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopAddition_Hops_HopId",
                        column: x => x.HopId,
                        principalTable: "Hops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopAddition_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Fermentables",
                columns: new[] { "Id", "Color", "MaxInBatch", "Name", "Origin", "PotentialGravity", "Type" },
                values: new object[,]
                {
                    { 1, 2.2000000000000002, 100.0, "2-row", "UnitedStates", 1.036, "Grain" },
                    { 2, 3.5, 80.0, "Maris Otter", "UnitedKingdom", 1.038, "Grain" },
                    { 3, 20.0, 15.0, "Caramel/Crystal Malt - 20L", "UnitedStates", 1.0349999999999999, "Grain" },
                    { 4, 2.0, 80.0, "Pilsner Malt", "Belgium", 1.036, "Grain" },
                    { 5, 8.0, 15.0, "Munich Malt", "Germany", 1.0369999999999999, "Grain" }
                });

            migrationBuilder.InsertData(
                table: "Hops",
                columns: new[] { "Id", "AlphaAcid", "Name" },
                values: new object[,]
                {
                    { 1, 12.0, "Citra" },
                    { 2, 13.0, "Mosaic" },
                    { 3, 14.0, "Motueka" },
                    { 4, 12.0, "Simcoe" },
                    { 5, 7.0, "Cascade" },
                    { 6, 9.0, "Amarillo" },
                    { 7, 3.0, "Saaz" },
                    { 8, 5.0, "Styrian Goldings" }
                });

            migrationBuilder.InsertData(
                table: "Yeasts",
                columns: new[] { "Id", "Code", "Flocculation", "Form", "Lab", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "US-05", "Medium", "Dry", "Fermentis", "Cal Ale", "Ale" },
                    { 2, "1056", "Medium", "Liquid", "Wyeast", "American Ale", "Ale" },
                    { 3, "S-04", "Low", "Dry", "Fermentis", "English Ale", "Ale" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "ABV", "AmountOfWater", "Color", "FinalGravity", "IBU", "MashTemp", "Name", "OriginalGravity", "Style", "WaterRatio", "YeastAmount", "YeastId", "YeastViability" },
                values: new object[,]
                {
                    { 1, 7.2000000000000002, 6.0, 12.0, 1.01, 60.0, 152, "Classic IPA", 1.0649999999999999, "AmericanIPA", 1.25, 1.0, 1, 95.0 },
                    { 2, 5.2000000000000002, 5.5, 8.0, 1.012, 35.0, 150, "Hoppy Pale Ale", 1.052, "AmericanPaleAle", 1.2, 1.0, 2, 95.0 },
                    { 3, 7.5, 6.5, 18.0, 1.018, 25.0, 154, "Belgian Dubbel", 1.0720000000000001, "BelgianDubbel", 1.3, 1.0, 3, 90.0 }
                });

            migrationBuilder.InsertData(
                table: "FermentablePair",
                columns: new[] { "Id", "FermentableId", "RecipeId", "Weight" },
                values: new object[,]
                {
                    { 1, 1, 1, 10.0 },
                    { 2, 1, 2, 8.0 },
                    { 3, 2, 2, 4.0 },
                    { 4, 4, 3, 7.0 },
                    { 5, 5, 3, 5.0 }
                });

            migrationBuilder.InsertData(
                table: "HopAddition",
                columns: new[] { "Id", "Amount", "BoilTime", "DryHopDays", "Form", "HopId", "RecipeId", "Use" },
                values: new object[,]
                {
                    { 1, 25.0, 60, 0, "Pellet", 1, 1, "Boil" },
                    { 2, 25.0, 60, 0, "Pellet", 2, 1, "Boil" },
                    { 3, 25.0, 60, 0, "Pellet", 3, 1, "Boil" },
                    { 4, 20.0, 60, 5, "Pellet", 1, 2, "Boil" },
                    { 5, 15.0, 15, 3, "Pellet", 2, 2, "Boil" },
                    { 6, 30.0, 0, 7, "Pellet", 5, 2, "Dry Hop" },
                    { 7, 20.0, 60, 0, "Pellet", 7, 3, "Boil" },
                    { 8, 15.0, 15, 0, "Pellet", 8, 3, "Boil" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FermentablePair_FermentableId",
                table: "FermentablePair",
                column: "FermentableId");

            migrationBuilder.CreateIndex(
                name: "IX_FermentablePair_RecipeId",
                table: "FermentablePair",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_HopAddition_HopId",
                table: "HopAddition",
                column: "HopId");

            migrationBuilder.CreateIndex(
                name: "IX_HopAddition_RecipeId",
                table: "HopAddition",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_YeastId",
                table: "Recipes",
                column: "YeastId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FermentablePair");

            migrationBuilder.DropTable(
                name: "HopAddition");

            migrationBuilder.DropTable(
                name: "Fermentables");

            migrationBuilder.DropTable(
                name: "Hops");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Yeasts");
        }
    }
}
