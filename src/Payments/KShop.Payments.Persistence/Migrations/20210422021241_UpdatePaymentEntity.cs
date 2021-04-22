using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Payments.Persistence.Migrations
{
    public partial class UpdatePaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Payments",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Payments",
                nullable: true,
                defaultValue: "RUB");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "PaymentLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "PaymentLogs");
        }
    }
}
