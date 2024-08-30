﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.Services;

#nullable disable

namespace Portfolio.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240807125141_Firstmigrations")]
    partial class Firstmigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Portfolio.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountID"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalMarketValue")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)");

                    b.HasKey("AccountID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Portfolio.Models.Security", b =>
                {
                    b.Property<int>("SecurityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SecurityId"));

                    b.Property<DateTime>("Lastupdate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MarketValue")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)");

                    b.Property<string>("SecurityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SecurityType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("currentPrice")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)");

                    b.Property<string>("symbol")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SecurityId");

                    b.ToTable("securities");
                });

            modelBuilder.Entity("Portfolio.Models.SecurityShare", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("SecurityId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantite")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)");

                    b.HasKey("AccountId", "SecurityId");

                    b.HasIndex("SecurityId");

                    b.ToTable("SecurityShares");
                });

            modelBuilder.Entity("Portfolio.Models.SecurityShare", b =>
                {
                    b.HasOne("Portfolio.Models.Account", "Account")
                        .WithMany("SecurityShares")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Portfolio.Models.Security", "Security")
                        .WithMany("SecurityShares")
                        .HasForeignKey("SecurityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Security");
                });

            modelBuilder.Entity("Portfolio.Models.Account", b =>
                {
                    b.Navigation("SecurityShares");
                });

            modelBuilder.Entity("Portfolio.Models.Security", b =>
                {
                    b.Navigation("SecurityShares");
                });
#pragma warning restore 612, 618
        }
    }
}
