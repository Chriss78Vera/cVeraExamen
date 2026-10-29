namespace cVeraExamen.Views;

public partial class vLogin : ContentPage
{
    // Matriz solicitada: cada fila almacena usuario y contraseña válidos.
    private readonly string[,] _credenciales = {
        { "recepcion", "gym2026" }, 
        { "entrenador", "fit_01" },
        { "uisrael", "2026" }, 
        { "gerente", "admin123" } 
    };
    public vLogin() => InitializeComponent();

    private async void IniciarSesion_Clicked(object? sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
        string contrasena = ContrasenaEntry.Text ?? string.Empty;
        if (!CredencialesValidas(usuario, contrasena)) { 
            await DisplayAlertAsync("Datos incorrectos", "El usuario o la contraseña no son válidos.", "Aceptar"); 
            return;
        }
        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private bool CredencialesValidas(string usuario, string contrasena)
    {
        for (int fila = 0; fila < _credenciales.GetLength(0); fila++)
            if (_credenciales[fila, 0] == usuario && _credenciales[fila, 1] == contrasena) return true;
        return false;
    }
    private async void AcercaDe_Clicked(object? sender, EventArgs e) => await Navigation.PushAsync(new vAcercaDe());
}
