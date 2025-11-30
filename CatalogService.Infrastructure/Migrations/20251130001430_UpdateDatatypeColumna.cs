using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatatypeColumna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
