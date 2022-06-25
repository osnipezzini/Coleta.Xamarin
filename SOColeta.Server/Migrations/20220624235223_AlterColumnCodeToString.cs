using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AlterColumnCodeToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                schema: "sotech",
                table: "socoleta_products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                schema: "sotech",
                table: "socoleta_products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "integer");
        }
    }
}
