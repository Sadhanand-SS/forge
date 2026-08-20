using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Diet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomizableMealItemIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyMealMealItemIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DailyMealMealItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMealMealItemIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyMealMealItemIngredients_DailyMealMealItems_DailyMealMe~",
                        column: x => x.DailyMealMealItemId,
                        principalTable: "DailyMealMealItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMealMealItemIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyMealMealItemIngredients_UnitsOfMeasure_UnitId",
                        column: x => x.UnitId,
                        principalTable: "UnitsOfMeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealMealItemIngredients_DailyMealMealItemId",
                table: "DailyMealMealItemIngredients",
                column: "DailyMealMealItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealMealItemIngredients_IngredientId",
                table: "DailyMealMealItemIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealMealItemIngredients_UnitId",
                table: "DailyMealMealItemIngredients",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMealMealItemIngredients");
        }
    }
}
