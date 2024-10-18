using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

namespace TabBarSwitches.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Nunito-SemiBold.ttf", "RegularFont");
                fonts.AddFont("Nunito-Bold.ttf", "BoldFont");
            })
            .UseSimpleShell()
            .UseSimpleToolkit();

#if ANDROID || IOS
        builder.DisplayContentBehindBars();
#endif

#if ANDROID
        builder.SetDefaultNavigationBarAppearance(color: Colors.Transparent);
#endif

        return builder.Build();
    }
}