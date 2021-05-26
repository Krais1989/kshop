using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Shipments.Persistence.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    OrderID = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    PendingDate = table.Column<DateTime>(nullable: false),
                    CompleteDate = table.Column<DateTime>(nullable: false),
                    LastCheckingDate = table.Column<DateTime>(nullable: false),
                    ExternalID = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
