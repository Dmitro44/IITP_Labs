using MauiLab1.Pages.Calculator;
using MauiLab1.Pages.CurrencyConverter;
using MauiLab1.Pages.TouristRoutes;
using MauiLab1.Services;
using Microsoft.Extensions.Logging;
using ProgressBar = MauiLab1.Pages.ProgressBar.ProgressBar;

namespace MauiLab1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                // fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                // fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Segoe-UI-Variable-Static-Text.ttf", "SegoeUIVariableStaticText");
                fonts.AddFont("Segoe-UI-Variable-Static-Text-Semibold.ttf", "SegoeUIVariableStaticTextSemibold");
            });

        builder.Services.AddTransient<Calculator>();
        builder.Services.AddTransient<ProgressBar>();
        builder.Services.AddTransient<TouristRoutesPage>();
        builder.Services.AddTransient<CurrencyConverterPage>();

        builder.Services.AddTransient<IDbService, SqLiteService>();
            //builder.Services.AddTransient<IRateService, RateService>();
        
        builder.Services.AddHttpClient<IRateService, RateService>("Nbrb_api",opt => 
            opt.BaseAddress = new Uri("https://www.nbrb.by/api/exrates/rates"));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}