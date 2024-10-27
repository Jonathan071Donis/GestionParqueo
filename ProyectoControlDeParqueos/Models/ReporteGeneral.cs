using System;
using System.Collections.Generic;
using ProyectoControlDeParqueos.Models;

namespace ProyectoControlDeParqueos.Models
{
	public class ReporteGeneral
	{
		public List<RegistroVehiculo>? Vehiculos { get; set; }
		public List<Tarifa>? Tarifas { get; set; }
		public List<Parqueo>? Parqueos { get; set; }

		// Reporte de ocupación total de los parqueos
		public decimal ObtenerOcupacionTotal()
		{
			decimal ocupacionTotal = 0;
			int totalCapacidad = 0;

			if (Parqueos != null)
			{
				foreach (var parqueo in Parqueos)
				{
					totalCapacidad += parqueo.capacidadTotal;
					ocupacionTotal += parqueo.ReporteOcupacion();
				}

				return ocupacionTotal / Parqueos.Count; // Porcentaje promedio de ocupación
			}

			return 0;
		}

		// Reporte de vehículos ingresados hoy
		public int ObtenerVehiculosIngresadosHoy()
		{
			int totalIngresadosHoy = 0;

			if (Vehiculos != null)
			{
				foreach (var vehiculo in Vehiculos)
				{
					if (vehiculo.fechaIngreso.Date == DateTime.Today)
					{
						totalIngresadosHoy++;
					}
				}
			}

			return totalIngresadosHoy;
		}

		// Reporte de tarifas activas
		public int ObtenerTarifasActivas()
		{
			int tarifasActivas = 0;

			if (Tarifas != null)
			{
				foreach (var tarifa in Tarifas)
				{
					if (tarifa.estado)
					{
						tarifasActivas++;
					}
				}
			}

			return tarifasActivas;
		}
	}
}
