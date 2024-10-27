using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoControlDeParqueos.Migrations
{
    /// <inheritdoc />
    public partial class tickecingreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticketIngresos",
                columns: table => new
                {
                    idTicketIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    RegistroVehiculoId = table.Column<int>(type: "int", nullable: false),
                    idParqueo = table.Column<int>(type: "int", nullable: false),
                    fechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketIngresos", x => x.idTicketIngreso);
                    table.ForeignKey(
                        name: "FK_ticketIngresos_Parqueos_idParqueo",
                        column: x => x.idParqueo,
                        principalTable: "Parqueos",
                        principalColumn: "idParqueo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketIngresos_RegistroVehiculos_RegistroVehiculoId",
                        column: x => x.RegistroVehiculoId,
                        principalTable: "RegistroVehiculos",
                        principalColumn: "idRegistroVehiculo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketIngresos_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "clientes",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ticketIngresos_ClienteId",
                table: "ticketIngresos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketIngresos_idParqueo",
                table: "ticketIngresos",
                column: "idParqueo");

            migrationBuilder.CreateIndex(
                name: "IX_ticketIngresos_RegistroVehiculoId",
                table: "ticketIngresos",
                column: "RegistroVehiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticketIngresos");
        }
    }
}
