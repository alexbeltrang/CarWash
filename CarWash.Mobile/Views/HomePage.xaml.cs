namespace CarWash.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblBienvenida.Text = $"Bienvenido, {Preferences.Get("DisplayName", "Usuario")}";
        lblEmail.Text = Preferences.Get("Email", string.Empty);
    }

    private async void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
