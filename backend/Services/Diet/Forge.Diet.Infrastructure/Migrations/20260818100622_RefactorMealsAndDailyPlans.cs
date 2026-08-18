using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forge.Diet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMealsAndDailyPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'public'
          AND table_name = 'Meals'
          AND column_name = 'Date'
    ) THEN
        IF to_regclass('"LegacyMealMealItems"') IS NULL AND to_regclass('"MealMealItems"') IS NOT NULL THEN
            ALTER TABLE "MealMealItems" RENAME TO "LegacyMealMealItems";
        END IF;

        IF to_regclass('"LegacyMeals"') IS NULL THEN
            ALTER TABLE "Meals" RENAME TO "LegacyMeals";
        END IF;
    END IF;
END $$;
""");

            migrationBuilder.Sql("""
CREATE TABLE IF NOT EXISTS "Meals" (
    "Id" uuid NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Time" time without time zone NOT NULL,
    "IsSystem" boolean NOT NULL,
    CONSTRAINT "PK_Meals" PRIMARY KEY ("Id")
);
""");

            migrationBuilder.Sql("""
CREATE TABLE IF NOT EXISTS "DailyMeals" (
    "Id" uuid NOT NULL,
    "Date" date NOT NULL,
    "MealId" uuid NOT NULL,
    "IsSkipped" boolean NOT NULL,
    CONSTRAINT "PK_DailyMeals" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DailyMeals_Meals_MealId" FOREIGN KEY ("MealId") REFERENCES "Meals" ("Id") ON DELETE CASCADE
);
""");

            migrationBuilder.Sql("""
CREATE TABLE IF NOT EXISTS "DailyMealMealItems" (
    "Id" uuid NOT NULL,
    "DailyMealId" uuid NOT NULL,
    "MealItemId" uuid NOT NULL,
    "Servings" numeric(18,4) NOT NULL,
    CONSTRAINT "PK_DailyMealMealItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DailyMealMealItems_DailyMeals_DailyMealId" FOREIGN KEY ("DailyMealId") REFERENCES "DailyMeals" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_DailyMealMealItems_MealItems_MealItemId" FOREIGN KEY ("MealItemId") REFERENCES "MealItems" ("Id") ON DELETE RESTRICT
);
""");

            migrationBuilder.Sql("""
CREATE INDEX IF NOT EXISTS "IX_DailyMealMealItems_DailyMealId" ON "DailyMealMealItems" ("DailyMealId");
CREATE INDEX IF NOT EXISTS "IX_DailyMealMealItems_MealItemId" ON "DailyMealMealItems" ("MealItemId");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_DailyMeals_Date_MealId" ON "DailyMeals" ("Date", "MealId");
CREATE INDEX IF NOT EXISTS "IX_DailyMeals_MealId" ON "DailyMeals" ("MealId");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Meals_Name" ON "Meals" ("Name");
""");

            migrationBuilder.Sql("""
DO $$
BEGIN
    IF to_regclass('"LegacyMeals"') IS NOT NULL THEN
        INSERT INTO "Meals" ("Id", "Name", "Time", "IsSystem")
        SELECT mapped_id, "Name", "Time", is_system
        FROM (
            SELECT
                CASE
                    WHEN lower("Name") = 'breakfast' THEN '11111111-1111-1111-1111-111111111111'::uuid
                    WHEN lower("Name") = 'lunch' THEN '22222222-2222-2222-2222-222222222222'::uuid
                    WHEN lower("Name") = 'dinner' THEN '33333333-3333-3333-3333-333333333333'::uuid
                    ELSE min("Id") OVER (PARTITION BY lower("Name"))
                END AS mapped_id,
                "Name",
                "Time",
                CASE
                    WHEN lower("Name") IN ('breakfast', 'lunch', 'dinner') THEN TRUE
                    ELSE bool_or("IsDefault") OVER (PARTITION BY lower("Name"))
                END AS is_system,
                row_number() OVER (PARTITION BY lower("Name") ORDER BY "Date", "Time", "Id") AS rn
            FROM "LegacyMeals"
        ) deduped
        WHERE rn = 1
        ON CONFLICT ("Id") DO NOTHING;

        INSERT INTO "DailyMeals" ("Id", "Date", "MealId", "IsSkipped")
        SELECT
            legacy."Id",
            legacy."Date",
            CASE
                WHEN lower(legacy."Name") = 'breakfast' THEN '11111111-1111-1111-1111-111111111111'::uuid
                WHEN lower(legacy."Name") = 'lunch' THEN '22222222-2222-2222-2222-222222222222'::uuid
                WHEN lower(legacy."Name") = 'dinner' THEN '33333333-3333-3333-3333-333333333333'::uuid
                ELSE defs."Id"
            END,
            FALSE
        FROM "LegacyMeals" legacy
        JOIN "Meals" defs ON lower(defs."Name") = lower(legacy."Name")
        ON CONFLICT ("Id") DO NOTHING;

        IF to_regclass('"LegacyMealMealItems"') IS NOT NULL THEN
            INSERT INTO "DailyMealMealItems" ("Id", "DailyMealId", "MealItemId", "Servings")
            SELECT "Id", "MealId", "MealItemId", "Servings"
            FROM "LegacyMealMealItems"
            ON CONFLICT ("Id") DO NOTHING;
        END IF;

        DROP TABLE IF EXISTS "LegacyMealMealItems";
        DROP TABLE IF EXISTS "LegacyMeals";
    END IF;
END $$;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMealMealItems");

            migrationBuilder.DropTable(
                name: "DailyMeals");

            migrationBuilder.DropTable(
                name: "Meals");
        }
    }
}
