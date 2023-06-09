﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddLeadPriceAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Leads",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Leads",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Leads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Leads");
        }
    }
}
