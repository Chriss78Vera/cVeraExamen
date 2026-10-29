namespace cVeraExamen.Models;

// Modelo que concentra los datos para evitar enviar muchos parámetros entre vistas.
public class Socio
{
    public required string Nombre { get; init; }
    public required string Apellido { get; init; }
    public required int Edad { get; init; }
    public required DateTime FechaInicio { get; init; }
    public required string Plan { get; init; }
    public required bool Casillero { get; init; }
    public required decimal MontoInicial { get; init; }
    public required decimal PagoMensual { get; init; }
    public required decimal PagoTotal { get; init; }
}
