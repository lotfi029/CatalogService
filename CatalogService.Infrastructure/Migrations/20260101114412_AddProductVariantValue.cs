using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVariantValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customization_options",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "variant_attributes",
                table: "product_variants");

            migrationBuilder.CreateTable(
                name: "ProductVariantValue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_attribute_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantValue", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductVariantValue_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantValue_variant_attribute_definitions_product_v~",
                        column: x => x.product_variant_id,
                        principalTable: "variant_attribute_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValue_product_variant_id",
                table: "ProductVariantValue",
                column: "product_variant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantValue");

            migrationBuilder.AddColumn<string>(
                name: "customization_options",
                table: "product_variants",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "variant_attributes",
                table: "product_variants",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
