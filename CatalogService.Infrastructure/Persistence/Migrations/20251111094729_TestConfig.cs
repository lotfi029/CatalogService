using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TestConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    vendor_id = table.Column<string>(type: "text", nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    price_currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_products_price",
                table: "products",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "idx_products_sku",
                table: "products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_products_status",
                table: "products",
                column: "status",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_products_vendor_id",
                table: "products",
                column: "vendor_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
