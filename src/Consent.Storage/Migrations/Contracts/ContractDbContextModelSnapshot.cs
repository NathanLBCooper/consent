﻿// <auto-generated />
using Consent.Storage.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Consent.Storage.Migrations.Contracts
{
    [DbContext(typeof(ContractDbContext))]
    partial class ContractDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("contracts")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Consent.Domain.Contracts.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Contracts", "contracts");
                });

            modelBuilder.Entity("Consent.Domain.Contracts.Contract", b =>
                {
                    b.OwnsMany("Consent.Domain.Contracts.ContractVersion", "Versions", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<int>("ContractId")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Status")
                                .HasColumnType("int");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id");

                            b1.HasIndex("ContractId");

                            b1.ToTable("ContractVersion", "contracts");

                            b1.WithOwner()
                                .HasForeignKey("ContractId");

                            b1.OwnsMany("Consent.Domain.Contracts.Provision", "Provisions", b2 =>
                                {
                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int");

                                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b2.Property<int>("Id"));

                                    b2.Property<int>("ContractVersionId")
                                        .HasColumnType("int");

                                    b2.Property<string>("PurposeIds")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("Text")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("Id");

                                    b2.HasIndex("ContractVersionId");

                                    b2.ToTable("Provision", "contracts");

                                    b2.WithOwner("ContractVersion")
                                        .HasForeignKey("ContractVersionId");

                                    b2.Navigation("ContractVersion");
                                });

                            b1.Navigation("Provisions");
                        });

                    b.Navigation("Versions");
                });
#pragma warning restore 612, 618
        }
    }
}
