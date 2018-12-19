﻿// <auto-generated />
using System;
using EFImplicitOperator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFImplicitOperator.Migrations
{
    [DbContext(typeof(TestContext))]
    partial class TestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFImplicitOperator.Child", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Childen");
                });

            modelBuilder.Entity("EFImplicitOperator.Parent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("OptionalChildId");

                    b.Property<Guid>("RequiredChildId");

                    b.HasKey("Id");

                    b.HasIndex("OptionalChildId");

                    b.HasIndex("RequiredChildId");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("EFImplicitOperator.Parent", b =>
                {
                    b.HasOne("EFImplicitOperator.Child", "OptionalChild")
                        .WithMany()
                        .HasForeignKey("OptionalChildId");

                    b.HasOne("EFImplicitOperator.Child", "RequiredChild")
                        .WithMany()
                        .HasForeignKey("RequiredChildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
