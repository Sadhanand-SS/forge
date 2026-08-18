using System;
using Forge.Diet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forge.Diet.Infrastructure.Migrations
{
    [DbContext(typeof(DietDbContext))]
    [Migration("20260818123000_AddMeals")]
    public partial class AddMeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Kept as a no-op because the later meal refactor migration now owns
            // both fresh-database creation and legacy-meals migration.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty.
        }
    }
}
