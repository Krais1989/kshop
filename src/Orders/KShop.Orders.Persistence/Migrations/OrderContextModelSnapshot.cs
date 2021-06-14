﻿// <auto-generated />
using System;
using KShop.Orders.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KShop.Orders.Persistence.Migrations
{
    [DbContext(typeof(OrderContext))]
    partial class OrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint unsigned");

                    b.Property<DateTime>("StatusDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.OrderLog", b =>
                {
                    b.Property<uint>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<byte>("NewStatus")
                        .HasColumnType("tinyint unsigned");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StatusDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderLog");
                });

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.OrderPosition", b =>
                {
                    b.Property<uint>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("char(36)");

                    b.Property<uint>("ProductID")
                        .HasColumnType("bigint unsigned");

                    b.Property<uint>("Quantity")
                        .HasColumnType("int unsigned");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderPositions");
                });

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.OrderLog", b =>
                {
                    b.HasOne("KShop.Orders.Persistence.Entities.Order", "Order")
                        .WithMany("Logs")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.OrderPosition", b =>
                {
                    b.HasOne("KShop.Orders.Persistence.Entities.Order", "Order")
                        .WithMany("Positions")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("KShop.Orders.Persistence.Entities.Order", b =>
                {
                    b.Navigation("Logs");

                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
