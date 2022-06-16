using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AddCollumsGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "sotech",
                table: "socoleta_inventarios",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "sotech",
                table: "socoleta_inventarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "inventario_id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "inventario_guid",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guid",
                schema: "sotech",
                table: "socoleta_inventarios");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropColumn(
                name: "inventario_guid",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "sotech",
                table: "socoleta_inventarios",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "inventario_id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        }
    }
}
