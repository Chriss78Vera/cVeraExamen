using System.Globalization;
using System.Text;
using cVeraExamen.Models;

namespace cVeraExamen.Views;

public partial class vResumen : ContentPage
{
    private readonly string _usuarioConectado;
    private readonly Socio _socio;

    public vResumen(string usuarioConectado, Socio socio)
    {
        InitializeComponent(); _usuarioConectado = usuarioConectado; _socio = socio;
        UsuarioConectadoLabel.Text = $"Usuario conectado: {_usuarioConectado}";
        NombreLabel.Text = socio.Nombre; ApellidoLabel.Text = socio.Apellido; EdadLabel.Text = socio.Edad.ToString();
        FechaLabel.Text = socio.FechaInicio.ToString("d", CultureInfo.CurrentCulture); PlanLabel.Text = socio.Plan;
        CasilleroLabel.Text = socio.Casillero ? "Sí" : "No";
        MontoInicialLabel.Text = socio.MontoInicial.ToString("F2", CultureInfo.CurrentCulture);
        PagoMensualLabel.Text = socio.PagoMensual.ToString("F2", CultureInfo.CurrentCulture);
        PagoTotalLabel.Text = socio.PagoTotal.ToString("F2", CultureInfo.CurrentCulture);
    }

    private async void ExportarDatos_Clicked(object? sender, EventArgs e)
    {
        string nombreArchivo = $"socio_{_socio.Nombre}_{_socio.Apellido}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        foreach (char caracter in Path.GetInvalidFileNameChars()) nombreArchivo = nombreArchivo.Replace(caracter, '_');
        string ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);
        string contenido = $"Usuario conectado: {_usuarioConectado}{Environment.NewLine}Nombre: {_socio.Nombre}{Environment.NewLine}Apellido: {_socio.Apellido}{Environment.NewLine}Edad: {_socio.Edad}{Environment.NewLine}Fecha de inicio: {_socio.FechaInicio:d}{Environment.NewLine}Plan: {_socio.Plan}{Environment.NewLine}Casillero: {(_socio.Casillero ? "Sí" : "No")}{Environment.NewLine}Monto inicial: {_socio.MontoInicial:F2}{Environment.NewLine}Pago mensual: {_socio.PagoMensual:F2}{Environment.NewLine}Pago total: {_socio.PagoTotal:F2}";
        await File.WriteAllTextAsync(ruta, contenido, Encoding.UTF8);
        await DisplayAlertAsync("Archivo generado", $"Nombre: {nombreArchivo}{Environment.NewLine}Ruta: {ruta}", "Aceptar");
    }

    private async void NuevoRegistro_Clicked(object? sender, EventArgs e)
    {
        // Se limpia la pila hasta Login y se crea un formulario sin datos anteriores.
        await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new vRegistro(_usuarioConectado));
    }

    private async void CerrarSesion_Clicked(object? sender, EventArgs e) => await Navigation.PopToRootAsync();
}
