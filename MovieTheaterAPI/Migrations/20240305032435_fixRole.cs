using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieTheaterAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "51ce62f3-44b5-44f0-b310-603bec517e4e");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "5c8fd367-5af9-48d2-80d6-832815186d00");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a4e78071-ce12-4bc3-a7b9-109678f0976d");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56adbadd-6784-4e03-bc2c-acef7734aadc", null, "Manager", "MANAGER" },
                    { "7af046cc-7454-45f1-ad8e-f574591d5976", null, "Staff", "STAFF" },
                    { "a03464d9-a4cb-47fa-ada6-4597aedcdf65", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "56adbadd-6784-4e03-bc2c-acef7734aadc");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "7af046cc-7454-45f1-ad8e-f574591d5976");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a03464d9-a4cb-47fa-ada6-4597aedcdf65");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "51ce62f3-44b5-44f0-b310-603bec517e4e", null, "Staff", "STAFF" },
                    { "5c8fd367-5af9-48d2-80d6-832815186d00", null, "Customer", "CUSTOMER" },
                    { "a4e78071-ce12-4bc3-a7b9-109678f0976d", null, "Manager", "MANAGER" }
                });
        }
    }
}
