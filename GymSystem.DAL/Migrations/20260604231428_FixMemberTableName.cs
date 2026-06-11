using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Member",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "Adress_Street",
                table: "Member",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Adress_City",
                table: "Member",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "Adress_BuildingNumber",
                table: "Member",
                newName: "BuildingNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Member",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Member",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Member",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Member",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_Member_Email",
                table: "Member",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_Phone",
                table: "Member",
                column: "Phone",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_EmailCheck",
                table: "Member",
                sql: "Email LIKE '_%@_%._%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_PhoneCheck",
                table: "Member",
                sql: "[Phone] LIKE '010%' OR [Phone] LIKE '011%' OR [Phone] LIKE '012%' OR [Phone] LIKE '015%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Member_Email",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_Phone",
                table: "Member");

            migrationBuilder.DropCheckConstraint(
                name: "GymUser_EmailCheck",
                table: "Member");

            migrationBuilder.DropCheckConstraint(
                name: "GymUser_PhoneCheck",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Member",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Member",
                newName: "Adress_Street");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Member",
                newName: "Adress_City");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "Member",
                newName: "Adress_BuildingNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Member",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Member",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Member",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Member",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Adress_Street",
                table: "Member",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Adress_City",
                table: "Member",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);
        }
    }
}
