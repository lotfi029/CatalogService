using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatatypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE variant_attribute_definitions
                ALTER COLUMN data_type TYPE integer
                USING CASE
                    WHEN data_type = 'Select' THEN 1
                    WHEN data_type = 'Text' THEN 2
                    WHEN data_type = 'Boolean' THEN 3
                    ELSE 0
                END;
                ");
            migrationBuilder.DropIndex(
                name: "idx_products_status",
                table: "products");

            migrationBuilder.AlterColumn<short>(
                name: "data_type",
                table: "variant_attribute_definitions",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "data_type_name",
                table: "variant_attribute_definitions",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "idx_products_status",
                table: "products",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_products_status",
                table: "products");

            migrationBuilder.DropColumn(
                name: "data_type_name",
                table: "variant_attribute_definitions");

            migrationBuilder.AlterColumn<string>(
                name: "data_type",
                table: "variant_attribute_definitions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.CreateIndex(
                name: "idx_products_status",
                table: "products",
                column: "status",
                unique: true);
        }
    }
}
