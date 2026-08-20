using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Diet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackedMealItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPacked",
                table: "DailyMealMealItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PackedWeight",
                table: "DailyMealMealItems",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCookedWeight",
                table: "DailyMealMealItems",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPacked",
                table: "DailyMealMealItems");

            migrationBuilder.DropColumn(
                name: "PackedWeight",
                table: "DailyMealMealItems");

            migrationBuilder.DropColumn(
                name: "TotalCookedWeight",
                table: "DailyMealMealItems");
        }
    }
}
