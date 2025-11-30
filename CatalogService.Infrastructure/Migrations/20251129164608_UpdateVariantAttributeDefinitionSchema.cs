using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVariantAttributeDefinitionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_required",
                table: "variant_attribute_definitions");

            migrationBuilder.DropColumn(
                name: "validation_rules",
                table: "variant_attribute_definitions");

            migrationBuilder.AlterColumn<string>(
                name: "allowed_values",
                table: "variant_attribute_definitions",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Dictionary<string, object>),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Dictionary<string, object>>(
                name: "allowed_values",
                table: "variant_attribute_definitions",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_required",
                table: "variant_attribute_definitions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Dictionary<string, object>>(
                name: "validation_rules",
                table: "variant_attribute_definitions",
                type: "jsonb",
                nullable: false);
        }
    }
}
