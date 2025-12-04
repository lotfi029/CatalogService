using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttributeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_attributes_is_filterable",
                table: "attributes");

            migrationBuilder.DropIndex(
                name: "idx_attributes_is_searchable",
                table: "attributes");

            migrationBuilder.DropIndex(
                name: "idx_attributes_type",
                table: "attributes");

            migrationBuilder.DropCheckConstraint(
                name: "chk_attributes_type",
                table: "attributes");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "attributes",
                newName: "options_type");

            migrationBuilder.AddColumn<string>(
                name: "options_type_name",
                table: "attributes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "idx_attributes_is_filterable",
                table: "attributes",
                column: "is_filterable");

            migrationBuilder.CreateIndex(
                name: "idx_attributes_is_searchable",
                table: "attributes",
                column: "is_searchable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_attributes_is_filterable",
                table: "attributes");

            migrationBuilder.DropIndex(
                name: "idx_attributes_is_searchable",
                table: "attributes");

            migrationBuilder.DropColumn(
                name: "options_type_name",
                table: "attributes");

            migrationBuilder.RenameColumn(
                name: "options_type",
                table: "attributes",
                newName: "type");

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

            migrationBuilder.AddCheckConstraint(
                name: "chk_attributes_type",
                table: "attributes",
                sql: "type > 0 AND type <= 6");
        }
    }
}
