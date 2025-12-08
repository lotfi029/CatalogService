using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductVariantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_product_variants_is_active",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "last_updated_at",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "last_updated_by",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "product_categories");

            migrationBuilder.AlterColumn<string>(
                name: "customization_options",
                table: "product_variants",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<decimal>(
                name: "compare_at_price",
                table: "product_variants",
                type: "numeric(10,2)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldDefaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "customization_options",
                table: "product_variants",
                type: "jsonb",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "compare_at_price",
                table: "product_variants",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldNullable: true,
                oldDefaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "product_variants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "product_variants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "product_variants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "product_variants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_variants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "product_variants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated_at",
                table: "product_variants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_updated_by",
                table: "product_variants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "product_categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_is_active",
                table: "product_variants",
                column: "is_active");
        }
    }
}
