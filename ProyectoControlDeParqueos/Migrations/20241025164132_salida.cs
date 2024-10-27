using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoControlDeParqueos.Migrations
{
    /// <inheritdoc />
    public partial class salida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticketSalidas",
                columns: table => new
                {
                    idTicketSalida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    RegistroVehiculoId = table.Column<int>(type: "int", nullable: false),
                    ParqueoId = table.Column<int>(type: "int", nullable: false),
                    TarifaId = table.Column<int>(type: "int", nullable: false),
                    fechaSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    costoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketSalidas", x => x.idTicketSalida);
                    table.ForeignKey(
                        name: "FK_ticketSalidas_Parqueos_ParqueoId",
                        column: x => x.ParqueoId,
                        principalTable: "Parqueos",
                        principalColumn: "idParqueo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketSalidas_RegistroVehiculos_RegistroVehiculoId",
                        column: x => x.RegistroVehiculoId,
                        principalTable: "RegistroVehiculos",
                        principalColumn: "idRegistroVehiculo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketSalidas_Tarifas_TarifaId",
                        column: x => x.TarifaId,
                        principalTable: "Tarifas",
                        principalColumn: "idTarifa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticketSalidas_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "clientes",
                        principalColumn: "idCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ticketSalidas_ClienteId",
                table: "ticketSalidas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketSalidas_ParqueoId",
                table: "ticketSalidas",
                column: "ParqueoId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketSalidas_RegistroVehiculoId",
                table: "ticketSalidas",
                column: "RegistroVehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketSalidas_TarifaId",
                table: "ticketSalidas",
                column: "TarifaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticketSalidas");
        }
    }
}
