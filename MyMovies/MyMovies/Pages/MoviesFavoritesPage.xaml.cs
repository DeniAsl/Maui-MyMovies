using MyMovies.Core.Models;
using MyMovies.Core.Services;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MoviesFavoritesPage : ContentPage
{
	private MoviesFavoritesViewModel _viewModel;
	public MoviesFavoritesPage(MoviesFavoritesViewModel viewModel)
	{
        _viewModel = viewModel;
		BindingContext = viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        if (_viewModel.Movies.Count == 0)
            _viewModel.GetFavMoviesCommand?.Execute(null);
        base.OnAppearing();
    }

    private void LstMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            _viewModel.ShowMovieCommand?.Execute(movie);
        }
    }
}