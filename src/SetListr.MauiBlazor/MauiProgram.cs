using CommunityToolkit.Maui;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

using SetListr.Data;

namespace SetListr.MauiBlazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SegoeUI-Regular.ttf", "Segoe UI");
			});

        builder.Services.AddFluentUIComponents();
		builder.Services.AddMauiBlazorWebView();
        builder.Services.AddDataGridEntityFrameworkAdapter();

        builder.Services.AddDbContext<SetListrDb>(options => options.UseSqlite($"Data Source=setlistr.db"));

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

#if false
        app.Services.GetRequiredService<SetListrDb>().Database.EnsureDeleted();
#endif

        app.Services.GetRequiredService<SetListrDb>().Database.EnsureCreated();
        return app;
    }
}
