using CarWash.Mobile.Services;

namespace CarWash.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

    public LoginPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
        string password = txtPassword.Text ?? string.Empty;

        if (string.IsNullOrEmpty(usuario))
        {
            MostrarError("Digite el usuario.");
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            MostrarError("Digite la contraseña.");
            return;
        }

        SetCargando(true);

        var respuesta = await _authService.LoginAsync(usuario, password);

        SetCargando(false);

        if (!respuesta.EsValido)
        {
            MostrarError(respuesta.Respuesta);
            return;
        }

        Preferences.Set("IdUser", respuesta.Usuario!.IdUser);
        Preferences.Set("DisplayName", respuesta.Usuario.DisplayName);
        Preferences.Set("Email", respuesta.Usuario.Email.ToLower());
        Preferences.Set("PerfilId", respuesta.Usuario.PerfilId);

        await Shell.Current.GoToAsync("//HomePage");
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.IsVisible = true;
    }

    private void SetCargando(bool cargando)
    {
        loader.IsRunning = cargando;
        loader.IsVisible = cargando;
        btnLogin.IsEnabled = !cargando;
        lblError.IsVisible = false;
    }
}
