using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TINColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("0ecabf09-8d29-4cab-9593-ffda5c1016f0"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("1884cf5f-9cac-4d2e-8407-ae688f39adc8"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("2f4322b9-d231-4d6c-89da-eb0821467fbb"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("3b372230-d9b4-4952-99f4-3e9369fa0db4"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("3cd4ad95-b7d4-43a9-b1a8-c09a3ffe8612"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("47f724a7-7851-46f5-8b1c-c3f3b9d29eb9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("a116271c-7443-4323-84c5-d41ba2d96be9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("b0da6468-792d-4649-b1dd-62d6f9c22735"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("b32c879f-c578-4f6f-b6a6-de5069ae2f4b"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonID",
                keyValue: new Guid("b4e88afa-ba23-4861-bc66-2deebc899239"),
                column: "TIN",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TIN",
                table: "Persons");
        }
    }
}
