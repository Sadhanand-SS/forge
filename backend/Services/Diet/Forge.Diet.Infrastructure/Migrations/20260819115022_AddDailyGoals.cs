using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Diet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Calories = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Protein = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Carbohydrates = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Fat = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Fiber = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGoals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyGoals_Date",
                table: "DailyGoals",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyGoals");
        }
    }
}
