using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    nome_arquivo = table.Column<string>(type: "text", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    is_inserted = table.Column<bool>(type: "boolean", nullable: false),
                    device = table.Column<string>(type: "text", nullable: false),
                    empresa = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    deposito = table.Column<long>(type: "bigint", nullable: false),
                    grid = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cost_price = table.Column<double>(type: "double precision", nullable: true),
                    sale_price = table.Column<double>(type: "double precision", nullable: true),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coletas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    quantidade = table.Column<double>(type: "double precision", nullable: false),
                    produto_id = table.Column<int>(type: "integer", nullable: true),
                    hora_coleta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    inventario_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coletas", x => x.id);
                    table.ForeignKey(
                        name: "fk_coletas_inventarios_inventario_id",
                        column: x => x.inventario_id,
                        principalTable: "inventarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_coletas_products_produto_id",
                        column: x => x.produto_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_coletas_inventario_id",
                table: "coletas",
                column: "inventario_id");

            migrationBuilder.CreateIndex(
                name: "ix_coletas_produto_id",
                table: "coletas",
                column: "produto_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coletas");

            migrationBuilder.DropTable(
                name: "inventarios");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
