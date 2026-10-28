namespace CarWash.Mobile.Models;

public class LoginResponse
{
    public bool EsValido { get; set; }
    public string Respuesta { get; set; } = string.Empty;
    public UsuarioResponse? Usuario { get; set; }
}

public class UsuarioResponse
{
    public int IdUser { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int PerfilId { get; set; }
}
