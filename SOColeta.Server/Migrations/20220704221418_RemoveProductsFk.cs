using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class RemoveProductsFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_socoleta_coletas_socoleta_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropTable(
                name: "socoleta_products",
                schema: "sotech");

            migrationBuilder.DropIndex(
                name: "ix_socoleta_coletas_produto_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.AlterColumn<long>(
                name: "produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "socoleta_products",
                schema: "sotech",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    cost_price = table.Column<double>(type: "double precision", nullable: true),
                    deposito = table.Column<long>(type: "bigint", nullable: false),
                    grid = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    guid = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    sale_price = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_socoleta_products", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_socoleta_coletas_produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "produto_id");

            migrationBuilder.AddForeignKey(
                name: "fk_socoleta_coletas_socoleta_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "produto_id",
                principalSchema: "sotech",
                principalTable: "socoleta_products",
                principalColumn: "id");
        }
    }
}
