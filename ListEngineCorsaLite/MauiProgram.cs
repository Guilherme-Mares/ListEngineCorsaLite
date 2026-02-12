using ListEngineCorsaLite.Services;
using Microsoft.Extensions.Logging;

namespace ListEngineCorsaLite
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<DatabaseService>();

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetalhesMotorPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
