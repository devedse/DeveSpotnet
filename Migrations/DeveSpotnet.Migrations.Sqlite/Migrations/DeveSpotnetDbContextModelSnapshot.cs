﻿// <auto-generated />
using DeveSpotnet.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeveSpotnet.Migrations.Sqlite.Migrations
{
    [DbContext(typeof(DeveSpotnetDbContext))]
    partial class DeveSpotnetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("DeveSpotnet.Db.DbModels.DbSpotHeader", b =>
                {
                    b.Property<int>("ArticleNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Bytes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Code")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("From")
                        .HasColumnType("TEXT");

                    b.Property<int>("Lines")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageID")
                        .HasColumnType("TEXT");

                    b.Property<string>("References")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.HasKey("ArticleNumber");

                    b.ToTable("SpotHeaders");
                });
#pragma warning restore 612, 618
        }
    }
}
