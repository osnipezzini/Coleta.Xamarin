using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOColeta.Server.Migrations
{
    public partial class AddSchemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "pk_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inventarios",
                table: "inventarios");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "socoleta_products",
                newSchema: "sotech");

            migrationBuilder.RenameTable(
                name: "inventarios",
                newName: "socoleta_inventarios",
                newSchema: "sotech");

            migrationBuilder.AddPrimaryKey(
                name: "pk_socoleta_products",
                schema: "sotech",
                table: "socoleta_products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_socoleta_inventarios",
                schema: "sotech",
                table: "socoleta_inventarios",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_socoleta_coletas_socoleta_inventarios_inventario_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "inventario_id",
                principalSchema: "sotech",
                principalTable: "socoleta_inventarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_socoleta_coletas_socoleta_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas",
                column: "produto_id",
                principalSchema: "sotech",
                principalTable: "socoleta_products",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_socoleta_coletas_socoleta_inventarios_inventario_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropForeignKey(
                name: "fk_socoleta_coletas_socoleta_products_produto_id",
                schema: "sotech",
                table: "socoleta_coletas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_socoleta_products",
                schema: "sotech",
                table: "socoleta_products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_socoleta_inventarios",
                schema: "sotech",
                table: "socoleta_inventarios");

            migrationBuilder.RenameTable(
                name: "socoleta_products",
                schema: "sotech",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "socoleta_inventarios",
                schema: "sotech",
                newName: "inventarios");

            migrationBuilder.AddPrimaryKey(
                name: "pk_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inventarios",
                table: "inventarios",
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
    }
}
