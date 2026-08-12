using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSAM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialWithUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                schema: "public",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                schema: "public",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameInArabic",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameInEnglish",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "public",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastNameInArabic",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastNameInEnglish",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleNameInArabic",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleNameInEnglish",
                schema: "public",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonInfo_Address_City",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonInfo_Address_Country",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonInfo_Address_State",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonInfo_Address_Street",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstNameInArabic",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstNameInEnglish",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastNameInArabic",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastNameInEnglish",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleNameInArabic",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleNameInEnglish",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonInfo_Address_City",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonInfo_Address_Country",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonInfo_Address_State",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonInfo_Address_Street",
                schema: "public",
                table: "Users");
        }
    }
}
