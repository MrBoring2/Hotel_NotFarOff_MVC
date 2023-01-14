using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_NotFarOff.Migrations
{
    public partial class PaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PaymentMethodId",
                table: "Booking",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_PaymentMethod",
                table: "Booking",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_PaymentMethod",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_Booking_PaymentMethodId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Booking");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Guest",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomCategoryId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Raiting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_RoomCategory",
                        column: x => x.RoomCategoryId,
                        principalTable: "RoomCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_RoomCategoryId",
                table: "Review",
                column: "RoomCategoryId");
        }
    }
}
