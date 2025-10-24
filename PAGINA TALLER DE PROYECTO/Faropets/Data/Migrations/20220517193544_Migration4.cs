using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Faropets.Data.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_Clients_TClients_TClientsIdCliente",
                table: "TReports_Clients");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "TReports_Clients");

            migrationBuilder.RenameColumn(
                name: "DateDbt",
                table: "TReports_Clients",
                newName: "DateDebt");

            migrationBuilder.AlterColumn<int>(
                name: "TClientsIdCliente",
                table: "TReports_Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "TReports_Clients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "TPayments_clients",
                columns: table => new
                {
                    IdPayments = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Debt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentDebt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCliente = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPayments_clients", x => x.IdPayments);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_Clients_TClients_TClientsIdCliente",
                table: "TReports_Clients",
                column: "TClientsIdCliente",
                principalTable: "TClients",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_Clients_TClients_TClientsIdCliente",
                table: "TReports_Clients");

            migrationBuilder.DropTable(
                name: "TPayments_clients");

            migrationBuilder.RenameColumn(
                name: "DateDebt",
                table: "TReports_Clients",
                newName: "DateDbt");

            migrationBuilder.AlterColumn<int>(
                name: "TClientsIdCliente",
                table: "TReports_Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Deadline",
                table: "TReports_Clients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "IdClient",
                table: "TReports_Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_Clients_TClients_TClientsIdCliente",
                table: "TReports_Clients",
                column: "TClientsIdCliente",
                principalTable: "TClients",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
