﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Vet.Data.Migrations
{
    public partial class AddingAppointmentResign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsResigned",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResigned",
                table: "Appointments");
        }
    }
}
