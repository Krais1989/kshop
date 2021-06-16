using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Shipments.Persistence
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PendingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastCheckingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExternalID = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
