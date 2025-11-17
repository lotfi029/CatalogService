using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitSeedingEntitiesWithConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attributes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    is_filterable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_searchable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    options = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_updated_by = table.Column<string>(type: "text", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributes", x => x.id);
                    table.CheckConstraint("chk_attributes_type", "type > 0 AND type <= 6");
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    path = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    level = table.Column<short>(type: "smallint", nullable: false),
                    metadata = table.Column<string>(type: "jsonb", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_updated_by = table.Column<string>(type: "text", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.CheckConstraint("chk_categories_level", "level >= 0");
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    vendor_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_updated_by = table.Column<string>(type: "text", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.CheckConstraint("chk_products_status", "status IN (1, 2, 3, 4)");
                });

            migrationBuilder.CreateTable(
                name: "variant_attribute_definitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    data_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    affects_inventory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    affects_pricing = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    display_order = table.Column<short>(type: "smallint", nullable: false),
                    allowed_values = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    validation_rules = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_updated_by = table.Column<string>(type: "text", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attribute_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_attributes",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attribute_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_attributes", x => new { x.product_id, x.attribute_id });
                    table.ForeignKey(
                        name: "FK_product_attributes_attributes_attribute_id",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_attributes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => new { x.product_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_product_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_categories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    variant_attributes = table.Column<string>(type: "jsonb", nullable: false),
                    customization_options = table.Column<string>(type: "jsonb", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    price_currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    compare_at_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    compare_at_price_currency = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_updated_by = table.Column<string>(type: "text", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.id);
                    table.CheckConstraint("chk_products_compare_at_price", "compare_at_price IS NULL OR compare_at_price >= 0");
                    table.CheckConstraint("chk_products_price", "price >= 0");
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category_variant_attributes",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_attribute_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    display_order = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_variant_attributes", x => new { x.category_id, x.variant_attribute_id });
                    table.ForeignKey(
                        name: "FK_category_variant_attributes_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_variant_attributes_variant_attribute_definitions_v~",
                        column: x => x.variant_attribute_id,
                        principalTable: "variant_attribute_definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_attributes_code",
                table: "attributes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_attributes_is_filterable",
                table: "attributes",
                column: "is_filterable",
                filter: "is_filterable = true");

            migrationBuilder.CreateIndex(
                name: "idx_attributes_is_searchable",
                table: "attributes",
                column: "is_searchable",
                filter: "is_searchable = true");

            migrationBuilder.CreateIndex(
                name: "idx_attributes_type",
                table: "attributes",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_is_active",
                table: "attributes",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_categories_level",
                table: "categories",
                column: "level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_categories_slug",
                table: "categories",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_is_active",
                table: "categories",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_variant_attributes_variant_attribute_id",
                table: "category_variant_attributes",
                column: "variant_attribute_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_attributes_value",
                table: "product_attributes",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "IX_product_attributes_attribute_id",
                table: "product_attributes",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_category_id",
                table: "product_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "idx_is_active",
                table: "product_variants",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_price_amount",
                table: "product_variants",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_sku",
                table: "product_variants",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_is_active",
                table: "products",
                column: "is_active");

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

            migrationBuilder.CreateIndex(
                name: "IX_products_description",
                table: "products",
                column: "description")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "IX_products_name",
                table: "products",
                column: "name")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "idx_is_active",
                table: "variant_attribute_definitions",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_variant_attribute_definition_code",
                table: "variant_attribute_definitions",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_variant_attributes");

            migrationBuilder.DropTable(
                name: "product_attributes");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "variant_attribute_definitions");

            migrationBuilder.DropTable(
                name: "attributes");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
