namespace CarWash.Mobile.Models;

public class SesionUsuario
{
    public int IdUser { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int PerfilId { get; set; }
}
