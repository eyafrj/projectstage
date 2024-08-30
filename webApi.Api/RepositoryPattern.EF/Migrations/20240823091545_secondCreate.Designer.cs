﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DAL.EF;

#nullable disable

namespace RepositoryPattern.EF.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240823091545_secondCreate")]
    partial class secondCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RepositoryPattern.core.Models.Account", b =>
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

            modelBuilder.Entity("RepositoryPattern.core.Models.Security", b =>
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

                    b.ToTable("Securities");
                });
#pragma warning restore 612, 618
        }
    }
}
