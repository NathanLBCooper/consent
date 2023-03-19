﻿// <auto-generated />
using Consent.Storage.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Consent.Storage.Migrations.Workspaces
{
    [DbContext(typeof(WorkspaceDbContext))]
    partial class WorkspaceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("workspaces")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Consent.Domain.Workspaces.Membership", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("User")
                        .HasColumnType("int");

                    b.Property<int?>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Membership", "workspaces");
                });

            modelBuilder.Entity("Consent.Domain.Workspaces.Workspace", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Workspaces", "workspaces");
                });

            modelBuilder.Entity("Consent.Domain.Workspaces.Membership", b =>
                {
                    b.HasOne("Consent.Domain.Workspaces.Workspace", null)
                        .WithMany("Memberships")
                        .HasForeignKey("WorkspaceId");
                });

            modelBuilder.Entity("Consent.Domain.Workspaces.Workspace", b =>
                {
                    b.Navigation("Memberships");
                });
#pragma warning restore 612, 618
        }
    }
}
