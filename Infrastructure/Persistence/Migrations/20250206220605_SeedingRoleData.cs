using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRoleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019409cc-7157-71a4-99d5-e295c82679db", "019409cc-285c-796f-ba84-6d2d43a19e2e", "Admin", "ADMIN" },
                    { "019409cd-931b-7028-b668-bbc65d9213e0", "019409cd-7700-71c9-add3-699453281dc4", "NormalUser", "NORMALUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019409cc-7157-71a4-99d5-e295c82679db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019409cd-931b-7028-b668-bbc65d9213e0");
        }
    }
}
