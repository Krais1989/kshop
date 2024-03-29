﻿// <auto-generated />
using System;
using KShop.Identities.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KShop.Identities.Persistence.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("KShop.Identities.Persistence.Role", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<uint>("RoleId")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.User", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<sbyte>("Status")
                        .HasColumnType("TINYINT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = 1u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dc617a37-1f7a-4779-b5de-8648715473c2",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 2u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2390b6ec-eec5-48cc-aa93-c6eb100499cf",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 3u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0738d785-076f-4586-8064-8d44bffae525",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 4u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c8e3fcd8-aed8-4712-809a-8e5831bac073",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 5u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "613aeb00-bebb-4aa0-af88-dc62935999af",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 6u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0b165973-90e4-4f72-afee-b5b540000e9d",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 7u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "89bf2d76-d0f0-4671-b932-508e8ccea2a4",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 8u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "17f81736-97c0-461a-a05c-528bc6990af1",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 9u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "209207a3-d82f-4d6d-b805-9558e3b58c94",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        },
                        new
                        {
                            Id = 10u,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0638960e-85fe-43cf-95e7-87a8b5590f78",
                            Email = "asd@asd.ru",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            PhoneNumber = "123123123",
                            PhoneNumberConfirmed = false,
                            Status = (sbyte)0,
                            TwoFactorEnabled = false,
                            UserName = "asd"
                        });
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.Property<uint?>("UserId1")
                        .HasColumnType("int unsigned");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserRole", b =>
                {
                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("RoleId")
                        .HasColumnType("int unsigned");

                    b.Property<uint?>("UserId1")
                        .HasColumnType("int unsigned");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId1");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserToken", b =>
                {
                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<uint?>("UserId1")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.HasIndex("UserId1");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.RoleClaim", b =>
                {
                    b.HasOne("KShop.Identities.Persistence.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserClaim", b =>
                {
                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserLogin", b =>
                {
                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserRole", b =>
                {
                    b.HasOne("KShop.Identities.Persistence.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.UserToken", b =>
                {
                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KShop.Identities.Persistence.User", null)
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("KShop.Identities.Persistence.User", b =>
                {
                    b.Navigation("UserClaims");

                    b.Navigation("UserLogins");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
