using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsyncInn.Migrations
{
    /// <inheritdoc />
    public partial class SeedySeedySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomAmenities",
                columns: new[] { "Id", "AmenitiesID", "RoomID" },
                values: new object[,]
                {
                    { 74, 54, 201 },
                    { 75, 55, 301 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumn: "Id",
                keyValue: 75);
        }
    }
}
