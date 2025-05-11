using Microsoft.Extensions.Logging;
using MyMovies.Core.Interfaces;
using MyMovies.Core.Services;
using MyMovies.Pages;
using MyMovies.Platforms.Services;
using MyMovies.ViewModels;

namespace MyMovies;

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

        builder.Services.AddTransient<MoviesListPage>();
        builder.Services.AddTransient<MoviesListViewModel>();

        builder.Services.AddTransient<MoviesFavoritesPage>();
        builder.Services.AddTransient<MoviesFavoritesViewModel>();

		builder.Services.AddTransient<MovieDetailsPage>();
		builder.Services.AddTransient<MovieDetailsViewModel>();

        builder.Services.AddTransient<MoviesSearchPage>();
        builder.Services.AddTransient<MoviesSearchViewModel>();

        builder.Services.AddTransient<MoviesRelatedPage>();
        builder.Services.AddTransient<MoviesRelatedViewModel>();

        builder.Services.AddTransient<MoviesRecommendationsPage>();
        builder.Services.AddTransient<MoviesRecommendationsViewModel>();

        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();

        builder.Services.AddTransient<IApiService, ApiService>();
        builder.Services.AddTransient<IJsonMovieService, JsonMovieService>();
        builder.Services.AddTransient<ISettingsService, SettingsService>();

        builder.Services.AddSingleton<INativeAuthentication, NativeAuthentication>();

        Routing.RegisterRoute(nameof(MovieDetailsPage), typeof(MovieDetailsPage));
        Routing.RegisterRoute(nameof(MoviesRelatedPage), typeof(MoviesRelatedPage));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
