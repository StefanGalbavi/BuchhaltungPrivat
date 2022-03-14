using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuchhaltungPrivat.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Konto",
                keyColumn: "KontoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Konto",
                keyColumn: "KontoId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubCategory",
                keyColumn: "SubCategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Einnahme" },
                    { 2, "Ausgabe" }
                });

            migrationBuilder.InsertData(
                table: "Konto",
                columns: new[] { "KontoId", "KontoAmount", "KontoDate", "KontoName" },
                values: new object[,]
                {
                    { 1, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank" },
                    { 2, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bar" }
                });

            migrationBuilder.InsertData(
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "SubCategoryName" },
                values: new object[] { 1, 1, "Lohn" });
        }
    }
}
