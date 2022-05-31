using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AddSchemaSoColeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coletas_inventarios_inventario_id",
                table: "coletas");

            migrationBuilder.DropForeignKey(
                name: "fk_coletas_products_produto_id",
                table: "coletas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_coletas",
                table: "coletas");

            migrationBuilder.RenameTable(
                name: "coletas",
                newName: "sotech.socoleta_coleta");

            migrationBuilder.RenameIndex(
                name: "ix_coletas_produto_id",
                table: "sotech.socoleta_coleta",
                newName: "ix_sotech_socoleta_coleta_produto_id");

            migrationBuilder.RenameIndex(
                name: "ix_coletas_inventario_id",
                table: "sotech.socoleta_coleta",
                newName: "ix_sotech_socoleta_coleta_inventario_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sotech_socoleta_coleta",
                table: "sotech.socoleta_coleta",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sotech_socoleta_coleta_inventarios_inventario_id",
                table: "sotech.socoleta_coleta",
                column: "inventario_id",
                principalTable: "inventarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sotech_socoleta_coleta_products_produto_id",
                table: "sotech.socoleta_coleta",
                column: "produto_id",
                principalTable: "products",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sotech_socoleta_coleta_inventarios_inventario_id",
                table: "sotech.socoleta_coleta");

            migrationBuilder.DropForeignKey(
                name: "fk_sotech_socoleta_coleta_products_produto_id",
                table: "sotech.socoleta_coleta");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sotech_socoleta_coleta",
                table: "sotech.socoleta_coleta");

            migrationBuilder.RenameTable(
                name: "sotech.socoleta_coleta",
                newName: "coletas");

            migrationBuilder.RenameIndex(
                name: "ix_sotech_socoleta_coleta_produto_id",
                table: "coletas",
                newName: "ix_coletas_produto_id");

            migrationBuilder.RenameIndex(
                name: "ix_sotech_socoleta_coleta_inventario_id",
                table: "coletas",
                newName: "ix_coletas_inventario_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_coletas",
                table: "coletas",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_coletas_inventarios_inventario_id",
                table: "coletas",
                column: "inventario_id",
                principalTable: "inventarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_coletas_products_produto_id",
                table: "coletas",
                column: "produto_id",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
