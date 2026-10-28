using CarWash.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public AuthService(HttpClient httpClient, string apiBaseUrl)
    {
        _httpClient = httpClient;
        _apiBaseUrl = apiBaseUrl;
    }

    public async Task<LoginResponse> LoginAsync(string usuario, string password)
    {
        try
        {
            string usrCif = CifradoService.Cifrar(usuario);
            string pwdCif = CifradoService.Cifrar(password);

            var body = JsonConvert.SerializeObject(new { UserName = usrCif, Password = pwdCif });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(json)
                   ?? new LoginResponse { EsValido = false, Respuesta = "Respuesta vacía del servidor." };
        }
        catch (Exception ex)
        {
            return new LoginResponse { EsValido = false, Respuesta = "Error: " + ex.Message };
        }
    }
}
