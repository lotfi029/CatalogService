using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVendorIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE products
                ALTER COLUMN vendor_id TYPE uuid
                USING vendor_id::uuid;
                """);

            migrationBuilder.DropIndex(
                name: "idx_products_vendor_id",
                table: "products");

            migrationBuilder.AlterColumn<Guid>(
                name: "vendor_id",
                table: "products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "vendor_id",
                table: "products",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "idx_products_vendor_id",
                table: "products",
                column: "vendor_id",
                unique: true);
        }
    }
}
