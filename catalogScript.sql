CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE attributes (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    code character varying(100) NOT NULL,
    type smallint NOT NULL,
    is_filterable boolean NOT NULL DEFAULT FALSE,
    is_searchable boolean NOT NULL DEFAULT FALSE,
    options jsonb,
    created_by character varying(450) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    last_updated_by text,
    last_updated_at timestamp with time zone,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    deleted_by text,
    deleted_at timestamp with time zone,
    CONSTRAINT "PK_attributes" PRIMARY KEY (id),
    CONSTRAINT chk_attributes_type CHECK (type > 0 AND type <= 6)
);

CREATE TABLE categories (
    id uuid NOT NULL,
    parent_id uuid,
    name character varying(200) NOT NULL,
    description text,
    slug character varying(200) NOT NULL,
    path character varying(200),
    level smallint NOT NULL,
    metadata jsonb,
    created_by character varying(450) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    last_updated_by text,
    last_updated_at timestamp with time zone,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    deleted_by text,
    deleted_at timestamp with time zone,
    CONSTRAINT "PK_categories" PRIMARY KEY (id),
    CONSTRAINT chk_categories_level CHECK (level >= 0),
    CONSTRAINT "FK_categories_categories_parent_id" FOREIGN KEY (parent_id) REFERENCES categories (id) ON DELETE CASCADE
);

CREATE TABLE products (
    id uuid NOT NULL,
    name character varying(500) NOT NULL,
    description text,
    vendor_id character varying(450) NOT NULL,
    sku character varying(100),
    status integer NOT NULL,
    metadata jsonb,
    created_by character varying(450) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    last_updated_by text,
    last_updated_at timestamp with time zone,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    deleted_by text,
    deleted_at timestamp with time zone,
    CONSTRAINT "PK_products" PRIMARY KEY (id),
    CONSTRAINT chk_products_status CHECK (status IN (1, 2, 3, 4))
);

CREATE TABLE variant_attribute_definitions (
    id uuid NOT NULL,
    code character varying(100) NOT NULL,
    name character varying(100) NOT NULL,
    data_type character varying(200) NOT NULL,
    is_required boolean NOT NULL DEFAULT FALSE,
    affects_inventory boolean NOT NULL DEFAULT FALSE,
    affects_pricing boolean NOT NULL DEFAULT FALSE,
    display_order smallint NOT NULL,
    allowed_values jsonb NOT NULL,
    validation_rules jsonb NOT NULL,
    created_by character varying(450) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    last_updated_by text,
    last_updated_at timestamp with time zone,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    deleted_by text,
    deleted_at timestamp with time zone,
    CONSTRAINT "PK_variant_attribute_definitions" PRIMARY KEY (id)
);

CREATE TABLE product_attributes (
    product_id uuid NOT NULL,
    attribute_id uuid NOT NULL,
    value character varying(200) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "PK_product_attributes" PRIMARY KEY (product_id, attribute_id),
    CONSTRAINT "FK_product_attributes_attributes_attribute_id" FOREIGN KEY (attribute_id) REFERENCES attributes (id) ON DELETE CASCADE,
    CONSTRAINT "FK_product_attributes_products_product_id" FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE TABLE product_categories (
    product_id uuid NOT NULL,
    category_id uuid NOT NULL,
    is_primary boolean NOT NULL DEFAULT FALSE,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    created_by uuid,
    CONSTRAINT "PK_product_categories" PRIMARY KEY (product_id, category_id),
    CONSTRAINT "FK_product_categories_categories_category_id" FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE RESTRICT,
    CONSTRAINT "FK_product_categories_products_product_id" FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE TABLE product_variants (
    id uuid NOT NULL,
    product_id uuid NOT NULL,
    sku character varying(100) NOT NULL,
    variant_attributes jsonb NOT NULL,
    customization_options jsonb NOT NULL,
    price numeric(10,2) NOT NULL DEFAULT 0.0,
    price_currency character varying(5) NOT NULL,
    compare_at_price numeric(10,2) NOT NULL DEFAULT 0.0,
    compare_at_price_currency character varying(5),
    created_by character varying(450) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    last_updated_by text,
    last_updated_at timestamp with time zone,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    deleted_by text,
    deleted_at timestamp with time zone,
    CONSTRAINT "PK_product_variants" PRIMARY KEY (id),
    CONSTRAINT chk_products_compare_at_price CHECK (compare_at_price IS NULL OR compare_at_price >= 0),
    CONSTRAINT chk_products_price CHECK (price >= 0),
    CONSTRAINT "FK_product_variants_products_product_id" FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE TABLE category_variant_attributes (
    category_id uuid NOT NULL,
    variant_attribute_id uuid NOT NULL,
    is_required boolean NOT NULL DEFAULT FALSE,
    display_order smallint NOT NULL DEFAULT 0,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    created_by character varying(450),
    CONSTRAINT "PK_category_variant_attributes" PRIMARY KEY (category_id, variant_attribute_id),
    CONSTRAINT "FK_category_variant_attributes_categories_category_id" FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE,
    CONSTRAINT "FK_category_variant_attributes_variant_attribute_definitions_v~" FOREIGN KEY (variant_attribute_id) REFERENCES variant_attribute_definitions (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX idx_attributes_code ON attributes (code);

CREATE INDEX idx_attributes_is_active ON attributes (is_active);

CREATE INDEX idx_attributes_is_filterable ON attributes (is_filterable) WHERE is_filterable = true;

CREATE INDEX idx_attributes_is_searchable ON attributes (is_searchable) WHERE is_searchable = true;

CREATE INDEX idx_attributes_type ON attributes (type);

CREATE INDEX idx_categories_is_active ON categories (is_active);

CREATE UNIQUE INDEX idx_categories_level ON categories (level);

CREATE UNIQUE INDEX idx_categories_slug ON categories (slug);

CREATE INDEX "IX_categories_parent_id" ON categories (parent_id);

CREATE INDEX "IX_category_variant_attributes_variant_attribute_id" ON category_variant_attributes (variant_attribute_id);

CREATE INDEX idx_product_attributes_value ON product_attributes (value);

CREATE INDEX "IX_product_attributes_attribute_id" ON product_attributes (attribute_id);

CREATE INDEX "IX_product_categories_category_id" ON product_categories (category_id);

CREATE INDEX idx_product_variants_is_active ON product_variants (is_active);

CREATE INDEX idx_product_variants_price_amount ON product_variants (price);

CREATE UNIQUE INDEX idx_product_variants_sku ON product_variants (sku);

CREATE INDEX "IX_product_variants_product_id" ON product_variants (product_id);

CREATE INDEX idx_products_is_active ON products (is_active);

CREATE UNIQUE INDEX idx_products_sku ON products (sku);

CREATE UNIQUE INDEX idx_products_status ON products (status);

CREATE UNIQUE INDEX idx_products_vendor_id ON products (vendor_id);

CREATE INDEX "IX_products_description" ON products USING gin (to_tsvector('english', coalesce(description, '')));

CREATE INDEX "IX_products_name" ON products USING gin (to_tsvector('english', name));

CREATE UNIQUE INDEX idx_variant_attribute_definition_code ON variant_attribute_definitions (code);

CREATE INDEX idx_variant_attribute_definitions_is_active ON variant_attribute_definitions (is_active);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251119135107_SeedingEntitiesWithConfiguration', '10.0.0');

COMMIT;

START TRANSACTION;
ALTER TABLE categories DROP COLUMN metadata;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251123200255_UpdateCategorySchema', '10.0.0');

COMMIT;

START TRANSACTION;
DROP INDEX idx_categories_level;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251124124732_RemoveUniqueConstrainFromLevelColumn', '10.0.0');

COMMIT;

