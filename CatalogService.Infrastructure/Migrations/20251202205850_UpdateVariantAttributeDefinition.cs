using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVariantAttributeDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "affects_pricing",
                table: "variant_attribute_definitions");

            migrationBuilder.DropColumn(
                name: "display_order",
                table: "variant_attribute_definitions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "affects_pricing",
                table: "variant_attribute_definitions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "display_order",
                table: "variant_attribute_definitions",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
