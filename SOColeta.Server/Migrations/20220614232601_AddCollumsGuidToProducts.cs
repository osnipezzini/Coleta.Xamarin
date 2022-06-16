using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AddCollumsGuidToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "sotech",
                table: "socoleta_products",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guid",
                schema: "sotech",
                table: "socoleta_products");
        }
    }
}
