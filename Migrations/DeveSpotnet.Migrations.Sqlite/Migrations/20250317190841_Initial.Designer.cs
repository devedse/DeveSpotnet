﻿// <auto-generated />
using System;
using DeveSpotnet.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeveSpotnet.Migrations.Sqlite.Migrations
{
    [DbContext(typeof(DeveSpotnetDbContext))]
    [Migration("20250317190841_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("ParsedHeader_Category")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParsedHeader_FileSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ParsedHeader_Header")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_HeaderSign")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParsedHeader_KeyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ParsedHeader_MessageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_Poster")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SelfSignedPubKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SpotterId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ParsedHeader_Stamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SubCatA")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SubCatB")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SubCatC")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SubCatD")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_SubCatZ")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_Tag")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_UserKey_Exponent")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_UserKey_Modulo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParsedHeader_UserSignature")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ParsedHeader_Valid")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("ParsedHeader_Verified")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("ParsedHeader_WasSigned")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ParsedHeader_XmlSignature")
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
