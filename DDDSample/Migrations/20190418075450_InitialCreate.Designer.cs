﻿// <auto-generated />
using System;
using DDDSample.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDSample.Migrations
{
    [DbContext(typeof(UserRepository))]
    [Migration("20190418075450_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DDDSample.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MyId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("RegDate");

                    b.HasKey("UserId");

                    b.ToTable("user");
                });
#pragma warning restore 612, 618
        }
    }
}
