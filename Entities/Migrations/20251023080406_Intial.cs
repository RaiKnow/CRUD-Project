using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("0c5b9025-a178-422f-9772-b83cf42f3fba"), "USA" },
                    { new Guid("148fecc0-f5aa-43a1-9744-1da4289c7a5a"), "Australia" },
                    { new Guid("206418e0-f767-4689-bac9-b0aa7696e60a"), "Canada" },
                    { new Guid("b7d4a259-c1df-42a1-b9cb-1d833d1580a3"), "UK" },
                    { new Guid("f7f1e057-cc8d-4c34-b03f-32387397daa2"), "India" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("0ecabf09-8d29-4cab-9593-ffda5c1016f0"), "PO Box 53534", new Guid("148fecc0-f5aa-43a1-9744-1da4289c7a5a"), new DateTime(1995, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "pharman4@github.com", "Male", "Peyter Harman", false },
                    { new Guid("1884cf5f-9cac-4d2e-8407-ae688f39adc8"), "Room 1638", new Guid("206418e0-f767-4689-bac9-b0aa7696e60a"), new DateTime(2006, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethoma6@mozilla.com", "Male", "Erroll Thoma", false },
                    { new Guid("2f4322b9-d231-4d6c-89da-eb0821467fbb"), "Apt 841", new Guid("0c5b9025-a178-422f-9772-b83cf42f3fba"), new DateTime(2009, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "toventon0@ucla.edu", "Female", "Tory Oventon", true },
                    { new Guid("3b372230-d9b4-4952-99f4-3e9369fa0db4"), "PO Box 5286", new Guid("f7f1e057-cc8d-4c34-b03f-32387397daa2"), new DateTime(1992, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "hfishlee2@ucoz.com", "Male", "Holly Fishlee", true },
                    { new Guid("3cd4ad95-b7d4-43a9-b1a8-c09a3ffe8612"), "Suite 59", new Guid("206418e0-f767-4689-bac9-b0aa7696e60a"), new DateTime(2024, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "nmccullen5@national.com", "Female", "Nicola McCullen", false },
                    { new Guid("47f724a7-7851-46f5-8b1c-c3f3b9d29eb9"), "5th Floor", new Guid("b7d4a259-c1df-42a1-b9cb-1d833d1580a3"), new DateTime(1990, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "bchiverton2@ucsd.edu", "Other", "Boothe Chiverton", true },
                    { new Guid("a116271c-7443-4323-84c5-d41ba2d96be9"), "6th Floor", new Guid("148fecc0-f5aa-43a1-9744-1da4289c7a5a"), new DateTime(1997, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "rhessay1@jalbum.net", "Female", "Rosy Hessay", false },
                    { new Guid("b0da6468-792d-4649-b1dd-62d6f9c22735"), "Room 861", new Guid("206418e0-f767-4689-bac9-b0aa7696e60a"), new DateTime(2015, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "tpalleske3@weibo.com", "Other", "Teresita Palleske", true },
                    { new Guid("b32c879f-c578-4f6f-b6a6-de5069ae2f4b"), "Apt 1481", new Guid("0c5b9025-a178-422f-9772-b83cf42f3fba"), new DateTime(2003, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "sewebank7@blogs.com", "Male", "Serge Ewebank", true },
                    { new Guid("b4e88afa-ba23-4861-bc66-2deebc899239"), "2nd Floor", new Guid("f7f1e057-cc8d-4c34-b03f-32387397daa2"), new DateTime(2013, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "byarrow4@utexas.edu", "Female", "Brana Yarrow", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
