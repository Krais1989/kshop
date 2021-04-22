using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Payments.Persistence.Migrations
{
    public partial class RenameColumnPlatformType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformType",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentPlatformType",
                table: "Payments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentPlatformType",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PlatformType",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
