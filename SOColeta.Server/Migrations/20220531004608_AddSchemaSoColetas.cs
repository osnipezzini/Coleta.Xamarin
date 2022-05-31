using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AddSchemaSoColetas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.EnsureSchema(
                name: "sotech");

            migrationBuilder.RenameTable(
                name: "sotech.socoleta_coleta",
                newName: "socoleta_coletas",
                newSchema: "sotech");

            migrationBuilder.RenameIndex(
                name: "ix_sotech_socoleta_coleta_produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                newName: "ix_socoleta_coletas_produto_id");

            migrationBuilder.RenameIndex(
                name: "ix_sotech_socoleta_coleta_inventario_id",
                schema: "sotech",
                table: "socoleta_coletas",
                newName: "ix_socoleta_coletas_inventario_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_socoleta_coletas",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_socoleta_coletas_inventarios_inventario_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "inventario_id",
                principalTable: "inventarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_socoleta_coletas_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "produto_id",
                principalTable: "products",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_socoleta_coletas_inventarios_inventario_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropForeignKey(
                name: "fk_socoleta_coletas_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_socoleta_coletas",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.RenameTable(
                name: "socoleta_coletas",
                schema: "sotech",
                newName: "sotech.socoleta_coleta");

            migrationBuilder.RenameIndex(
                name: "ix_socoleta_coletas_produto_id",
                table: "sotech.socoleta_coleta",
                newName: "ix_sotech_socoleta_coleta_produto_id");

            migrationBuilder.RenameIndex(
                name: "ix_socoleta_coletas_inventario_id",
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
    }
}
