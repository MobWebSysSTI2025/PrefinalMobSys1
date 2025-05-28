using Microsoft.Extensions.Logging;
using PrefinalMobSys1.Data;
using PrefinalMobSys1.Model; // Added for ThemeService

namespace PrefinalMobSys1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<AppShellContext>();
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddScoped<ThemeService>(); // Register ThemeService

            return builder.Build();
        }
    }
}