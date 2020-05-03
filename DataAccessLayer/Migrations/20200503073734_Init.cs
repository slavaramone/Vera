using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    Patronymic = table.Column<string>(maxLength: 255, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSmsSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(nullable: false),
                    SmsCode = table.Column<string>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSmsSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSmsSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "FirstName", "LastName", "Patronymic", "Phone", "Role" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin", "admin", "admin", "9876649344", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_UserSmsSessions_UserId",
                table: "UserSmsSessions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSmsSessions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
