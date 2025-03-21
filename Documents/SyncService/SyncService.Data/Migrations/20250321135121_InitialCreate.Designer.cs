﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SyncService.Data.DataContext;

#nullable disable

namespace SyncService.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250321135121_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SyncService.Core.Models.BusinessHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ClientSiteId")
                        .HasColumnType("text");

                    b.Property<string>("Day")
                        .HasColumnType("text");

                    b.Property<string>("End")
                        .HasColumnType("text");

                    b.Property<string>("Start")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClientSiteId");

                    b.ToTable("BusinessHours");
                });

            modelBuilder.Entity("SyncService.Core.Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("EmailDomains")
                        .HasColumnType("text[]");

                    b.Property<string>("ExactId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Stage")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SyncService.Core.Models.ClientSite", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
                        .HasColumnType("text");

                    b.Property<List<string>>("HolidayList")
                        .HasColumnType("text[]");

                    b.Property<string>("Line1")
                        .HasColumnType("text");

                    b.Property<string>("Line2")
                        .HasColumnType("text");

                    b.Property<string>("Line3")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text");

                    b.Property<string>("StateCode")
                        .HasColumnType("text");

                    b.Property<string>("TimezoneCode")
                        .HasColumnType("text");

                    b.Property<bool?>("Working24x7")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientSites");
                });

            modelBuilder.Entity("SyncService.Core.Models.ExactClient", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("ExactClients");
                });

            modelBuilder.Entity("SyncService.Core.Models.BusinessHour", b =>
                {
                    b.HasOne("SyncService.Core.Models.ClientSite", null)
                        .WithMany("BusinessHour")
                        .HasForeignKey("ClientSiteId");
                });

            modelBuilder.Entity("SyncService.Core.Models.Client", b =>
                {
                    b.OwnsOne("SyncService.Core.Models.AccountManager", "AccountManager", b1 =>
                        {
                            b1.Property<Guid>("ClientId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .HasColumnType("text");

                            b1.Property<string>("UserId")
                                .HasColumnType("text");

                            b1.HasKey("ClientId");

                            b1.ToTable("AccountManagers");

                            b1.WithOwner()
                                .HasForeignKey("ClientId");
                        });

                    b.OwnsOne("SyncService.Core.Models.HqSite", "HqSite", b1 =>
                        {
                            b1.Property<Guid>("ClientId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ClientId");

                            b1.ToTable("HqSites");

                            b1.WithOwner()
                                .HasForeignKey("ClientId");
                        });

                    b.OwnsOne("SyncService.Core.Models.PrimaryContact", "PrimaryContact", b1 =>
                        {
                            b1.Property<Guid>("ClientId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .HasColumnType("text");

                            b1.Property<string>("UserId")
                                .HasColumnType("text");

                            b1.HasKey("ClientId");

                            b1.ToTable("PrimaryContacts");

                            b1.WithOwner()
                                .HasForeignKey("ClientId");
                        });

                    b.OwnsOne("SyncService.Core.Models.SecondaryContact", "SecondaryContact", b1 =>
                        {
                            b1.Property<Guid>("ClientId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .HasColumnType("text");

                            b1.Property<string>("UserId")
                                .HasColumnType("text");

                            b1.HasKey("ClientId");

                            b1.ToTable("SecondaryContacts");

                            b1.WithOwner()
                                .HasForeignKey("ClientId");
                        });

                    b.Navigation("AccountManager");

                    b.Navigation("HqSite");

                    b.Navigation("PrimaryContact");

                    b.Navigation("SecondaryContact");
                });

            modelBuilder.Entity("SyncService.Core.Models.ClientSite", b =>
                {
                    b.HasOne("SyncService.Core.Models.Client", "Client")
                        .WithMany("ClientSites")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("SyncService.Core.Models.ExactClient", b =>
                {
                    b.HasOne("SyncService.Core.Models.Client", "Client")
                        .WithOne()
                        .HasForeignKey("SyncService.Core.Models.ExactClient", "Code")
                        .HasPrincipalKey("SyncService.Core.Models.Client", "ExactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("SyncService.Core.Models.Client", b =>
                {
                    b.Navigation("ClientSites");
                });

            modelBuilder.Entity("SyncService.Core.Models.ClientSite", b =>
                {
                    b.Navigation("BusinessHour");
                });
#pragma warning restore 612, 618
        }
    }
}
