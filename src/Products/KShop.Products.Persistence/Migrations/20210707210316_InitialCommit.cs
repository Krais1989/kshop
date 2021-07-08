using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Products.Persistence.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currency = table.Column<string>(type: "longtext", nullable: true, defaultValue: "RUB")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true, defaultValue: 0m),
                    Discount = table.Column<uint>(type: "int unsigned", nullable: false),
                    CategoryID = table.Column<uint>(type: "int unsigned", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    ProductID = table.Column<uint>(type: "int unsigned", nullable: false),
                    AttributeID = table.Column<uint>(type: "int unsigned", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => new { x.ProductID, x.AttributeID });
                    table.ForeignKey(
                        name: "FK_ProductAttribute_Attributes_AttributeID",
                        column: x => x.AttributeID,
                        principalTable: "Attributes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductBookmarks",
                columns: table => new
                {
                    ProductID = table.Column<uint>(type: "int unsigned", nullable: false),
                    CustomerID = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBookmarks", x => new { x.ProductID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_ProductBookmarks_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductPositions",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<uint>(type: "int unsigned", nullable: false),
                    Quantity = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPositions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductPositions_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductReserves",
                columns: table => new
                {
                    ID = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<uint>(type: "int unsigned", nullable: false),
                    Quantity = table.Column<uint>(type: "int unsigned", nullable: false),
                    OrderID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerID = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReserves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductReserves_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Attributes",
                columns: new[] { "ID", "Title" },
                values: new object[,]
                {
                    { 1u, "Attribute #1" },
                    { 9u, "Attribute #9" },
                    { 8u, "Attribute #8" },
                    { 7u, "Attribute #7" },
                    { 6u, "Attribute #6" },
                    { 10u, "Attribute #10" },
                    { 4u, "Attribute #4" },
                    { 3u, "Attribute #3" },
                    { 2u, "Attribute #2" },
                    { 5u, "Attribute #5" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 9u, "Category #9" },
                    { 1u, "Category #1" },
                    { 2u, "Category #2" },
                    { 3u, "Category #3" },
                    { 4u, "Category #4" },
                    { 5u, "Category #5" },
                    { 6u, "Category #6" },
                    { 7u, "Category #7" },
                    { 8u, "Category #8" },
                    { 10u, "Category #10" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "CategoryID", "Description", "Discount", "Image", "Title", "Currency", "Price" },
                values: new object[,]
                {
                    { 1u, 1u, "Description of product 1", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 1", "RUB", 100m },
                    { 19u, 9u, "Description of product 19", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 19", "RUB", 100m },
                    { 9u, 9u, "Description of product 9", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 9", "RUB", 100m },
                    { 18u, 8u, "Description of product 18", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 18", "RUB", 100m },
                    { 8u, 8u, "Description of product 8", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 8", "RUB", 100m },
                    { 17u, 7u, "Description of product 17", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 17", "RUB", 100m },
                    { 7u, 7u, "Description of product 7", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 7", "RUB", 100m },
                    { 16u, 6u, "Description of product 16", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 16", "RUB", 100m },
                    { 6u, 6u, "Description of product 6", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 6", "RUB", 100m },
                    { 15u, 5u, "Description of product 15", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 15", "RUB", 100m },
                    { 5u, 5u, "Description of product 5", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 5", "RUB", 100m },
                    { 14u, 4u, "Description of product 14", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 14", "RUB", 100m },
                    { 4u, 4u, "Description of product 4", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 4", "RUB", 100m },
                    { 13u, 3u, "Description of product 13", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 13", "RUB", 100m },
                    { 3u, 3u, "Description of product 3", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 3", "RUB", 100m },
                    { 22u, 2u, "Description of product 22", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 22", "RUB", 100m },
                    { 12u, 2u, "Description of product 12", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 12", "RUB", 100m },
                    { 2u, 2u, "Description of product 2", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 2", "RUB", 100m },
                    { 21u, 1u, "Description of product 21", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 21", "RUB", 100m },
                    { 11u, 1u, "Description of product 11", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 11", "RUB", 100m },
                    { 10u, 10u, "Description of product 10", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 10", "RUB", 100m },
                    { 20u, 10u, "Description of product 20", 0u, "https://cdn1.ozone.ru/multimedia/wc250/1022638096.jpg", "Product 20", "RUB", 100m }
                });

            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "AttributeID", "ProductID", "Value" },
                values: new object[,]
                {
                    { 1u, 1u, "Value of attribute 1" },
                    { 9u, 6u, "Value of attribute 9" },
                    { 10u, 6u, "Value of attribute 10" },
                    { 9u, 10u, "Value of attribute 9" },
                    { 1u, 16u, "Value of attribute 1" },
                    { 2u, 16u, "Value of attribute 2" },
                    { 3u, 16u, "Value of attribute 3" },
                    { 4u, 16u, "Value of attribute 4" },
                    { 5u, 16u, "Value of attribute 5" },
                    { 6u, 16u, "Value of attribute 6" },
                    { 7u, 16u, "Value of attribute 7" },
                    { 8u, 16u, "Value of attribute 8" },
                    { 9u, 16u, "Value of attribute 9" },
                    { 10u, 16u, "Value of attribute 10" },
                    { 8u, 10u, "Value of attribute 8" },
                    { 1u, 7u, "Value of attribute 1" },
                    { 2u, 7u, "Value of attribute 2" },
                    { 3u, 7u, "Value of attribute 3" },
                    { 4u, 7u, "Value of attribute 4" },
                    { 5u, 7u, "Value of attribute 5" },
                    { 6u, 7u, "Value of attribute 6" },
                    { 7u, 7u, "Value of attribute 7" },
                    { 8u, 7u, "Value of attribute 8" },
                    { 9u, 7u, "Value of attribute 9" },
                    { 8u, 6u, "Value of attribute 8" },
                    { 7u, 6u, "Value of attribute 7" },
                    { 6u, 6u, "Value of attribute 6" },
                    { 5u, 6u, "Value of attribute 5" },
                    { 3u, 5u, "Value of attribute 3" },
                    { 4u, 5u, "Value of attribute 4" },
                    { 5u, 5u, "Value of attribute 5" },
                    { 6u, 5u, "Value of attribute 6" },
                    { 7u, 5u, "Value of attribute 7" },
                    { 8u, 5u, "Value of attribute 8" },
                    { 9u, 5u, "Value of attribute 9" },
                    { 10u, 5u, "Value of attribute 10" },
                    { 10u, 20u, "Value of attribute 10" },
                    { 1u, 15u, "Value of attribute 1" },
                    { 2u, 15u, "Value of attribute 2" },
                    { 10u, 7u, "Value of attribute 10" },
                    { 3u, 15u, "Value of attribute 3" },
                    { 5u, 15u, "Value of attribute 5" },
                    { 6u, 15u, "Value of attribute 6" },
                    { 7u, 15u, "Value of attribute 7" },
                    { 8u, 15u, "Value of attribute 8" },
                    { 9u, 15u, "Value of attribute 9" },
                    { 10u, 15u, "Value of attribute 10" },
                    { 10u, 10u, "Value of attribute 10" },
                    { 1u, 6u, "Value of attribute 1" },
                    { 2u, 6u, "Value of attribute 2" },
                    { 3u, 6u, "Value of attribute 3" },
                    { 4u, 6u, "Value of attribute 4" },
                    { 4u, 15u, "Value of attribute 4" },
                    { 7u, 10u, "Value of attribute 7" },
                    { 1u, 17u, "Value of attribute 1" },
                    { 2u, 17u, "Value of attribute 2" },
                    { 9u, 18u, "Value of attribute 9" },
                    { 10u, 18u, "Value of attribute 10" },
                    { 4u, 10u, "Value of attribute 4" },
                    { 1u, 9u, "Value of attribute 1" },
                    { 2u, 9u, "Value of attribute 2" },
                    { 3u, 9u, "Value of attribute 3" },
                    { 4u, 9u, "Value of attribute 4" },
                    { 5u, 9u, "Value of attribute 5" },
                    { 6u, 9u, "Value of attribute 6" },
                    { 7u, 9u, "Value of attribute 7" },
                    { 8u, 9u, "Value of attribute 8" },
                    { 8u, 18u, "Value of attribute 8" },
                    { 9u, 9u, "Value of attribute 9" },
                    { 3u, 10u, "Value of attribute 3" },
                    { 1u, 19u, "Value of attribute 1" },
                    { 2u, 19u, "Value of attribute 2" },
                    { 3u, 19u, "Value of attribute 3" },
                    { 4u, 19u, "Value of attribute 4" },
                    { 5u, 19u, "Value of attribute 5" },
                    { 6u, 19u, "Value of attribute 6" },
                    { 7u, 19u, "Value of attribute 7" },
                    { 8u, 19u, "Value of attribute 8" },
                    { 9u, 19u, "Value of attribute 9" },
                    { 10u, 19u, "Value of attribute 10" },
                    { 10u, 9u, "Value of attribute 10" },
                    { 2u, 5u, "Value of attribute 2" },
                    { 7u, 18u, "Value of attribute 7" },
                    { 5u, 18u, "Value of attribute 5" },
                    { 3u, 17u, "Value of attribute 3" },
                    { 4u, 17u, "Value of attribute 4" },
                    { 5u, 17u, "Value of attribute 5" },
                    { 6u, 17u, "Value of attribute 6" },
                    { 7u, 17u, "Value of attribute 7" },
                    { 8u, 17u, "Value of attribute 8" },
                    { 9u, 17u, "Value of attribute 9" },
                    { 10u, 17u, "Value of attribute 10" },
                    { 6u, 10u, "Value of attribute 6" },
                    { 1u, 8u, "Value of attribute 1" },
                    { 2u, 8u, "Value of attribute 2" },
                    { 6u, 18u, "Value of attribute 6" },
                    { 3u, 8u, "Value of attribute 3" },
                    { 5u, 8u, "Value of attribute 5" },
                    { 6u, 8u, "Value of attribute 6" },
                    { 7u, 8u, "Value of attribute 7" },
                    { 8u, 8u, "Value of attribute 8" },
                    { 9u, 8u, "Value of attribute 9" },
                    { 10u, 8u, "Value of attribute 10" },
                    { 5u, 10u, "Value of attribute 5" },
                    { 1u, 18u, "Value of attribute 1" },
                    { 2u, 18u, "Value of attribute 2" },
                    { 3u, 18u, "Value of attribute 3" },
                    { 4u, 18u, "Value of attribute 4" },
                    { 4u, 8u, "Value of attribute 4" },
                    { 2u, 10u, "Value of attribute 2" },
                    { 1u, 5u, "Value of attribute 1" },
                    { 10u, 14u, "Value of attribute 10" },
                    { 8u, 21u, "Value of attribute 8" },
                    { 9u, 21u, "Value of attribute 9" },
                    { 10u, 21u, "Value of attribute 10" },
                    { 7u, 20u, "Value of attribute 7" },
                    { 1u, 2u, "Value of attribute 1" },
                    { 2u, 2u, "Value of attribute 2" },
                    { 3u, 2u, "Value of attribute 3" },
                    { 4u, 2u, "Value of attribute 4" },
                    { 5u, 2u, "Value of attribute 5" },
                    { 6u, 2u, "Value of attribute 6" },
                    { 7u, 2u, "Value of attribute 7" },
                    { 8u, 2u, "Value of attribute 8" },
                    { 9u, 2u, "Value of attribute 9" },
                    { 10u, 2u, "Value of attribute 10" },
                    { 6u, 20u, "Value of attribute 6" },
                    { 1u, 12u, "Value of attribute 1" },
                    { 2u, 12u, "Value of attribute 2" },
                    { 3u, 12u, "Value of attribute 3" },
                    { 4u, 12u, "Value of attribute 4" },
                    { 5u, 12u, "Value of attribute 5" },
                    { 6u, 12u, "Value of attribute 6" },
                    { 7u, 12u, "Value of attribute 7" },
                    { 8u, 12u, "Value of attribute 8" },
                    { 7u, 21u, "Value of attribute 7" },
                    { 6u, 21u, "Value of attribute 6" },
                    { 5u, 21u, "Value of attribute 5" },
                    { 4u, 21u, "Value of attribute 4" },
                    { 2u, 1u, "Value of attribute 2" },
                    { 3u, 1u, "Value of attribute 3" },
                    { 4u, 1u, "Value of attribute 4" },
                    { 5u, 1u, "Value of attribute 5" },
                    { 6u, 1u, "Value of attribute 6" },
                    { 7u, 1u, "Value of attribute 7" },
                    { 8u, 1u, "Value of attribute 8" },
                    { 9u, 1u, "Value of attribute 9" },
                    { 10u, 1u, "Value of attribute 10" },
                    { 9u, 20u, "Value of attribute 9" },
                    { 1u, 11u, "Value of attribute 1" },
                    { 9u, 12u, "Value of attribute 9" },
                    { 2u, 11u, "Value of attribute 2" },
                    { 4u, 11u, "Value of attribute 4" },
                    { 5u, 11u, "Value of attribute 5" },
                    { 6u, 11u, "Value of attribute 6" },
                    { 7u, 11u, "Value of attribute 7" },
                    { 8u, 11u, "Value of attribute 8" },
                    { 9u, 11u, "Value of attribute 9" },
                    { 10u, 11u, "Value of attribute 10" },
                    { 8u, 20u, "Value of attribute 8" },
                    { 1u, 21u, "Value of attribute 1" },
                    { 2u, 21u, "Value of attribute 2" },
                    { 3u, 21u, "Value of attribute 3" },
                    { 3u, 11u, "Value of attribute 3" },
                    { 10u, 12u, "Value of attribute 10" },
                    { 5u, 20u, "Value of attribute 5" },
                    { 1u, 22u, "Value of attribute 1" },
                    { 9u, 13u, "Value of attribute 9" },
                    { 10u, 13u, "Value of attribute 10" },
                    { 2u, 20u, "Value of attribute 2" },
                    { 1u, 4u, "Value of attribute 1" },
                    { 2u, 4u, "Value of attribute 2" },
                    { 3u, 4u, "Value of attribute 3" },
                    { 4u, 4u, "Value of attribute 4" },
                    { 5u, 4u, "Value of attribute 5" },
                    { 6u, 4u, "Value of attribute 6" },
                    { 7u, 4u, "Value of attribute 7" },
                    { 8u, 4u, "Value of attribute 8" },
                    { 9u, 4u, "Value of attribute 9" },
                    { 10u, 4u, "Value of attribute 10" },
                    { 1u, 20u, "Value of attribute 1" },
                    { 1u, 14u, "Value of attribute 1" },
                    { 2u, 14u, "Value of attribute 2" },
                    { 3u, 14u, "Value of attribute 3" },
                    { 4u, 14u, "Value of attribute 4" },
                    { 5u, 14u, "Value of attribute 5" },
                    { 6u, 14u, "Value of attribute 6" },
                    { 7u, 14u, "Value of attribute 7" },
                    { 8u, 14u, "Value of attribute 8" },
                    { 9u, 14u, "Value of attribute 9" },
                    { 8u, 13u, "Value of attribute 8" },
                    { 7u, 13u, "Value of attribute 7" },
                    { 6u, 13u, "Value of attribute 6" },
                    { 3u, 3u, "Value of attribute 3" },
                    { 2u, 22u, "Value of attribute 2" },
                    { 3u, 22u, "Value of attribute 3" },
                    { 4u, 22u, "Value of attribute 4" },
                    { 5u, 22u, "Value of attribute 5" },
                    { 6u, 22u, "Value of attribute 6" },
                    { 7u, 22u, "Value of attribute 7" },
                    { 8u, 22u, "Value of attribute 8" },
                    { 9u, 22u, "Value of attribute 9" },
                    { 10u, 22u, "Value of attribute 10" },
                    { 4u, 20u, "Value of attribute 4" },
                    { 1u, 3u, "Value of attribute 1" },
                    { 2u, 3u, "Value of attribute 2" },
                    { 1u, 10u, "Value of attribute 1" },
                    { 4u, 3u, "Value of attribute 4" },
                    { 5u, 3u, "Value of attribute 5" },
                    { 6u, 3u, "Value of attribute 6" },
                    { 7u, 3u, "Value of attribute 7" },
                    { 8u, 3u, "Value of attribute 8" },
                    { 9u, 3u, "Value of attribute 9" },
                    { 10u, 3u, "Value of attribute 10" },
                    { 3u, 20u, "Value of attribute 3" },
                    { 1u, 13u, "Value of attribute 1" },
                    { 2u, 13u, "Value of attribute 2" },
                    { 3u, 13u, "Value of attribute 3" },
                    { 4u, 13u, "Value of attribute 4" },
                    { 5u, 13u, "Value of attribute 5" }
                });

            migrationBuilder.InsertData(
                table: "ProductPositions",
                columns: new[] { "ID", "CreateDate", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 10u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4271), 10u, 100u },
                    { 5u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3840), 5u, 100u },
                    { 9u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4233), 9u, 100u },
                    { 1u, new DateTime(2021, 7, 7, 21, 3, 12, 100, DateTimeKind.Utc).AddTicks(4584), 1u, 100u },
                    { 11u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4302), 11u, 100u },
                    { 21u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4815), 21u, 100u },
                    { 2u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3631), 2u, 100u },
                    { 12u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4335), 12u, 100u },
                    { 22u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4850), 22u, 100u },
                    { 3u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3695), 3u, 100u },
                    { 13u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4429), 13u, 100u },
                    { 4u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3805), 4u, 100u },
                    { 14u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4463), 14u, 100u },
                    { 15u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4497), 15u, 100u },
                    { 6u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3879), 6u, 100u },
                    { 16u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4592), 16u, 100u },
                    { 7u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(3911), 7u, 100u },
                    { 17u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4623), 17u, 100u },
                    { 8u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4150), 8u, 100u },
                    { 18u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4659), 18u, 100u },
                    { 19u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4690), 19u, 100u },
                    { 20u, new DateTime(2021, 7, 7, 21, 3, 12, 101, DateTimeKind.Utc).AddTicks(4722), 20u, 100u }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_AttributeID",
                table: "ProductAttribute",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPositions_ProductID",
                table: "ProductPositions",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReserves_ProductID",
                table: "ProductReserves",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "ProductBookmarks");

            migrationBuilder.DropTable(
                name: "ProductPositions");

            migrationBuilder.DropTable(
                name: "ProductReserves");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
