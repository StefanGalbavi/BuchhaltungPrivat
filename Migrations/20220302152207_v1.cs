using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuchhaltungPrivat.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Konto",
                columns: table => new
                {
                    KontoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KontoAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konto", x => x.KontoId);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandingOrder",
                columns: table => new
                {
                    StandingOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OderName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandingOrderDateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StandingOrderDateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Repetition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KontoId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandingOrder", x => x.StandingOrderId);
                    table.ForeignKey(
                        name: "FK_StandingOrder_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_StandingOrder_Konto_KontoId",
                        column: x => x.KontoId,
                        principalTable: "Konto",
                        principalColumn: "KontoId");
                    table.ForeignKey(
                        name: "FK_StandingOrder_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "SubCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    KontoId = table.Column<int>(type: "int", nullable: true),
                    StandingOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Booking_Konto_KontoId",
                        column: x => x.KontoId,
                        principalTable: "Konto",
                        principalColumn: "KontoId");
                    table.ForeignKey(
                        name: "FK_Booking_StandingOrder_StandingOrderId",
                        column: x => x.StandingOrderId,
                        principalTable: "StandingOrder",
                        principalColumn: "StandingOrderId");
                    table.ForeignKey(
                        name: "FK_Booking_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "SubCategoryId");
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Einnahme" },
                    { 2, "Ausgabe" },
                    { 3, "Umbuchung" }
                });

            migrationBuilder.InsertData(
                table: "Konto",
                columns: new[] { "KontoId", "KontoAmount", "KontoName" },
                values: new object[,]
                {
                    { 1, 0m, "Bank" },
                    { 2, 0m, "Bar" }
                });

            migrationBuilder.InsertData(
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "SubCategoryName" },
                values: new object[] { 1, 1, "Lohn" });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CategoryId",
                table: "Booking",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_KontoId",
                table: "Booking",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StandingOrderId",
                table: "Booking",
                column: "StandingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SubCategoryId",
                table: "Booking",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StandingOrder_CategoryId",
                table: "StandingOrder",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StandingOrder_KontoId",
                table: "StandingOrder",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_StandingOrder_SubCategoryId",
                table: "StandingOrder",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "StandingOrder");

            migrationBuilder.DropTable(
                name: "Konto");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
