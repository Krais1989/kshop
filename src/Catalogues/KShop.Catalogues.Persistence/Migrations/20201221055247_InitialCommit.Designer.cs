﻿// <auto-generated />
using System;
using KShop.Catalogues.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KShop.Catalogues.Persistence.Migrations
{
    [DbContext(typeof(CatalogueContext))]
    [Migration("20201221055247_InitialCommit")]
    partial class InitialCommit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.ProductPosition", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductPositions");
                });

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.ProductReserve", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("char(36)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReserveDate")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductReserves");
                });

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.ProductPosition", b =>
                {
                    b.HasOne("KShop.Catalogues.Persistence.Entities.Product", "Product")
                        .WithMany("Positions")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.ProductReserve", b =>
                {
                    b.HasOne("KShop.Catalogues.Persistence.Entities.Product", "Product")
                        .WithMany("Reserves")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("KShop.Catalogues.Persistence.Entities.Product", b =>
                {
                    b.Navigation("Positions");

                    b.Navigation("Reserves");
                });
#pragma warning restore 612, 618
        }
    }
}