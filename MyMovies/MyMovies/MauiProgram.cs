using Microsoft.Extensions.Logging;
using MyMovies.Core.Interfaces;
using MyMovies.Core.Services;
using MyMovies.Pages;
using MyMovies.ViewModels;

namespace MyMovies;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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

        builder.Services.AddTransient<ApiService>();
        builder.Services.AddTransient<JsonMovieService>();
        builder.Services.AddTransient<SettingsService>();

        Routing.RegisterRoute(nameof(MovieDetailsPage), typeof(MovieDetailsPage));
        Routing.RegisterRoute(nameof(MoviesRelatedPage), typeof(MoviesRelatedPage));

        builder
            .UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
