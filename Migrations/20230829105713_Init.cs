using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Med.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<int>(type: "int", nullable: false),
                    IsBlocked = table.Column<int>(type: "int", nullable: false),
                    IsEmailConfirmed = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Points = table.Column<int>(type: "int", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassedTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Tries = table.Column<int>(type: "int", nullable: false),
                    IsPassed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassedTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassedTests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassedTests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Option1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Option2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Option3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Drugs",
                columns: new[] { "Id", "Description", "ImageUrl", "Summary", "Title", "VideoUrl" },
                values: new object[,]
                {
                    { 1, "Spray for running nose.", "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0", "Nose spray", "Naphazalin", "https://youtu.be/M8QKjDzb-Os" },
                    { 2, "Tablets to drink when you catch cold.", "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0", "White tablets", "Remantadin", "https://youtu.be/M8QKjDzb-Os" },
                    { 3, "The best tablets when you have allergy", "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0", "Allergy tablets", "Parlazin-Neo", "https://youtu.be/M8QKjDzb-Os" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Category", "City", "Count", "Email", "FirstName", "IsBlocked", "IsEmailConfirmed", "IsVerified", "LastLoginDate", "LastName", "Password", "Region", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "Cardiologist", "Brest", 10, "julia.klepaski@gmail.com", "Julia", 0, 1, 0, new DateTime(2023, 8, 29, 13, 57, 13, 119, DateTimeKind.Local).AddTicks(6408), "Chistyakova", "1", "Brest region", new DateTime(2023, 8, 29, 13, 57, 13, 119, DateTimeKind.Local).AddTicks(6381), "doctor" },
                    { 2, "Hospital", "Brest", 134, "maxon@gmail.com", "Maxim", 0, 0, 1, new DateTime(2023, 8, 29, 13, 57, 13, 119, DateTimeKind.Local).AddTicks(6420), "Dulevich", "1", "Brest region", new DateTime(2023, 8, 29, 13, 57, 13, 119, DateTimeKind.Local).AddTicks(6418), "doctor" }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "DrugId", "Points" },
                values: new object[] { 1, 1, 10 });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Answer", "Option1", "Option2", "Option3", "QuestionText", "TestId" },
                values: new object[,]
                {
                    { 1, "It's safe if use according to instructions", "Yes, be careful!", "Haha, no, at all.", "It's safe if use according to instructions", "Is nose spray dangerous?", 1 },
                    { 2, "Yes, it's literally the best!", "Yes, but no very much...", "No, it's terrible!", "Yes, it's literally the best!", "Do you like naphazalin?", 1 },
                    { 3, "Up to 5 days", "Endlessly", "Never", "Up to 5 days", "How long can you take it?", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassedTests_TestId",
                table: "PassedTests",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_PassedTests_UserId",
                table: "PassedTests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_DrugId",
                table: "Tests",
                column: "DrugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassedTests");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Drugs");
        }
    }
}
