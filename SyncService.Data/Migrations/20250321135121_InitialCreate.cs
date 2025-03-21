using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    Stage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    EmailDomains = table.Column<List<string>>(type: "text[]", nullable: true),
                    ExactId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.UniqueConstraint("AK_Clients_ExactId", x => x.ExactId);
                });

            migrationBuilder.CreateTable(
                name: "AccountManagers",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountManagers", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_AccountManagers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSites",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    ContactNumber = table.Column<string>(type: "text", nullable: true),
                    StateCode = table.Column<string>(type: "text", nullable: true),
                    Line1 = table.Column<string>(type: "text", nullable: true),
                    Line2 = table.Column<string>(type: "text", nullable: true),
                    Line3 = table.Column<string>(type: "text", nullable: true),
                    HolidayList = table.Column<List<string>>(type: "text[]", nullable: true),
                    TimezoneCode = table.Column<string>(type: "text", nullable: true),
                    Working24x7 = table.Column<bool>(type: "boolean", nullable: true),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExactClients",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExactClients", x => x.Code);
                    table.ForeignKey(
                        name: "FK_ExactClients_Clients_Code",
                        column: x => x.Code,
                        principalTable: "Clients",
                        principalColumn: "ExactId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HqSites",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HqSites", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_HqSites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryContacts",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryContacts", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_PrimaryContacts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecondaryContacts",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryContacts", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_SecondaryContacts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<string>(type: "text", nullable: true),
                    Start = table.Column<string>(type: "text", nullable: true),
                    End = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    ClientSiteId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessHours_ClientSites_ClientSiteId",
                        column: x => x.ClientSiteId,
                        principalTable: "ClientSites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_ClientSiteId",
                table: "BusinessHours",
                column: "ClientSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSites_ClientId",
                table: "ClientSites",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountManagers");

            migrationBuilder.DropTable(
                name: "BusinessHours");

            migrationBuilder.DropTable(
                name: "ExactClients");

            migrationBuilder.DropTable(
                name: "HqSites");

            migrationBuilder.DropTable(
                name: "PrimaryContacts");

            migrationBuilder.DropTable(
                name: "SecondaryContacts");

            migrationBuilder.DropTable(
                name: "ClientSites");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
