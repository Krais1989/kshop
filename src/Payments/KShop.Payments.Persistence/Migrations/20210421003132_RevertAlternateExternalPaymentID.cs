using Microsoft.EntityFrameworkCore.Migrations;

namespace KShop.Payments.Persistence.Migrations
{
    public partial class RevertAlternateExternalPaymentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Payments_ExternalPaymentID",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentID",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentID",
                table: "Payments",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Payments_ExternalPaymentID",
                table: "Payments",
                column: "ExternalPaymentID");
        }
    }
}
