using CarWash.Mobile.Services;
using CarWash.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace CarWash.Mobile;

public static class MauiProgram
{
    // Cambia esta URL por la IP real del servidor en producción
    private const string ApiBaseUrl = "http://10.0.2.2:5000"; // 10.0.2.2 = localhost desde emulador Android

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // HttpClient
        builder.Services.AddSingleton<HttpClient>();

        // Servicios
        builder.Services.AddSingleton(sp =>
            new AuthService(sp.GetRequiredService<HttpClient>(), ApiBaseUrl));

        // Vistas
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

