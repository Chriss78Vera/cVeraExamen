using System.Globalization;
using cVeraExamen.Models;

namespace cVeraExamen.Views;

public partial class vRegistro : ContentPage
{
    private const decimal CostoMembresia = 1200m;
    private const decimal InteresPorCuota = 0.02m * CostoMembresia;
    private const decimal CargoCasilleroPorCuota = 10m;
    private readonly string _usuarioConectado;
    private decimal? _pagoMensualCalculado;

    public vRegistro(string usuarioConectado)
    {
        InitializeComponent(); _usuarioConectado = usuarioConectado;
        UsuarioConectadoLabel.Text = $"Usuario conectado: {_usuarioConectado}";
        FechaInicioPicker.MinimumDate = DateTime.Today; FechaInicioPicker.Date = DateTime.Today;
    }

    private async void CalcularPagoMensual_Clicked(object? sender, EventArgs e)
    {
        if (!TryObtenerMontoInicial(out decimal montoInicial, out string error)) { 
            await DisplayAlertAsync("Monto inicial inválido", error, "Aceptar");
            return; 
        }
        // El saldo se divide en cinco cuotas y cada una recibe el 2 % del costo anual.
        decimal pagoMensual = ((CostoMembresia - montoInicial) / 5m) + InteresPorCuota;
        if (CasilleroSwitch.IsToggled) pagoMensual += CargoCasilleroPorCuota;
        _pagoMensualCalculado = pagoMensual;
        PagoMensualEntry.Text = pagoMensual.ToString("F2", CultureInfo.CurrentCulture);
    }

    private async void VerResumen_Clicked(object? sender, EventArgs e)
    {
        if (!TryCrearSocio(out Socio? socio, out string error)) { 
            await DisplayAlertAsync("Datos incompletos", error, "Aceptar"); 
            return; 
        }
        await Navigation.PushAsync(new vResumen(_usuarioConectado, socio!));
    }

    private bool TryCrearSocio(out Socio? socio, out string error)
    {
        socio = null;
        // Validaciones de campos de entradas nombres apellido, edad, fecha de inicio, plan, monto inicial y pago mensual.
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(ApellidoEntry.Text)) {
            error = "Ingrese el nombre y el apellido.";
            return false; 
        }

        if (!int.TryParse(EdadEntry.Text, out int edad) || edad < 16) {
            error = "La edad debe ser un número entero mayor o igual a 16.";
            return false; 
        }

        if (!FechaInicioPicker.Date.HasValue || FechaInicioPicker.Date.Value.Date < DateTime.Today) {
            error = "La fecha de inicio no puede ser pasada."; 
            return false; 
        }

        if (PlanPicker.SelectedIndex < 0) {
            error = "Seleccione un plan.";
            return false; 
        
        }
        if (!TryObtenerMontoInicial(out decimal montoInicial, out error)) return false;

        if (!_pagoMensualCalculado.HasValue) {
            error = "Primero calcule el pago mensual.";
            return false; 
        }

        socio = new Socio { 
            Nombre = NombreEntry.Text.Trim(),
            Apellido = ApellidoEntry.Text.Trim(),
            Edad = edad,
            FechaInicio = FechaInicioPicker.Date.Value,
            Plan = PlanPicker.SelectedItem?.ToString() ?? string.Empty,
            Casillero = CasilleroSwitch.IsToggled,
            MontoInicial = montoInicial,
            PagoMensual = _pagoMensualCalculado.Value,
            PagoTotal = montoInicial + (_pagoMensualCalculado.Value * 5m) 
        };
        
        error = string.Empty; return true;
    }

    private bool TryObtenerMontoInicial(out decimal montoInicial, out string error)
    {
        if (!decimal.TryParse(MontoInicialEntry.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out montoInicial) && !decimal.TryParse(MontoInicialEntry.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out montoInicial)) { error = "Ingrese un monto numérico."; return false; }
        if (montoInicial < 120m || montoInicial > 600m)
        {
            error = "El monto inicial debe estar entre 120 y 600.";
            return false; 
        }
        error = string.Empty; return true;
    }

    // Si cambia el monto o el casillero, el valor mostrado deja de ser válido.
    private void DatosDePago_Cambiaron(object? sender, EventArgs e)
    {
        _pagoMensualCalculado = null;
        PagoMensualEntry.Text = string.Empty;
    }
}
