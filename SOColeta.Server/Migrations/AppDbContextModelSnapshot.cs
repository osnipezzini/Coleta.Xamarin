﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SOColeta.Domain.Data;

#nullable disable

namespace SOColeta.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("SOColeta.Common.Models.Coleta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("codigo");

                    b.Property<DateTime>("HoraColeta")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("hora_coleta");

                    b.Property<int>("InventarioId")
                        .HasColumnType("integer")
                        .HasColumnName("inventario_id");

                    b.Property<int?>("ProdutoId")
                        .HasColumnType("integer")
                        .HasColumnName("produto_id");

                    b.Property<double>("Quantidade")
                        .HasColumnType("double precision")
                        .HasColumnName("quantidade");

                    b.HasKey("Id")
                        .HasName("pk_socoleta_coletas");

                    b.HasIndex("InventarioId")
                        .HasDatabaseName("ix_socoleta_coletas_inventario_id");

                    b.HasIndex("ProdutoId")
                        .HasDatabaseName("ix_socoleta_coletas_produto_id");

                    b.ToTable("socoleta_coletas", "sotech");
                });

            modelBuilder.Entity("SOColeta.Common.Models.Inventario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_criacao");

                    b.Property<string>("Device")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("device");

                    b.Property<long>("Empresa")
                        .HasColumnType("bigint")
                        .HasColumnName("empresa");

                    b.Property<bool>("IsInserted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_inserted");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean")
                        .HasColumnName("is_valid");

                    b.Property<string>("NomeArquivo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nome_arquivo");

                    b.HasKey("Id")
                        .HasName("pk_socoleta_inventarios");

                    b.ToTable("socoleta_inventarios", "sotech");
                });

            modelBuilder.Entity("SOColeta.Common.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("barcode");

                    b.Property<int>("Code")
                        .HasColumnType("integer")
                        .HasColumnName("code");

                    b.Property<double?>("CostPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("cost_price");

                    b.Property<long>("Deposito")
                        .HasColumnType("bigint")
                        .HasColumnName("deposito");

                    b.Property<long>("Grid")
                        .HasColumnType("bigint")
                        .HasColumnName("grid");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("group_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision")
                        .HasColumnName("quantity");

                    b.Property<double?>("SalePrice")
                        .HasColumnType("double precision")
                        .HasColumnName("sale_price");

                    b.HasKey("Id")
                        .HasName("pk_socoleta_products");

                    b.ToTable("socoleta_products", "sotech");
                });

            modelBuilder.Entity("SOColeta.Common.Models.Coleta", b =>
                {
                    b.HasOne("SOColeta.Common.Models.Inventario", "Inventario")
                        .WithMany("ProdutosColetados")
                        .HasForeignKey("InventarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_socoleta_coletas_socoleta_inventarios_inventario_id");

                    b.HasOne("SOColeta.Common.Models.Product", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .HasConstraintName("fk_socoleta_coletas_socoleta_products_produto_id");

                    b.Navigation("Inventario");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("SOColeta.Common.Models.Inventario", b =>
                {
                    b.Navigation("ProdutosColetados");
                });
#pragma warning restore 612, 618
        }
    }
}
