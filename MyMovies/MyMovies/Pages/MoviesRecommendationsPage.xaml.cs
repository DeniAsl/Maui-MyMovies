using MyMovies.Core.Models;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MoviesRecommendationsPage : ContentPage
{
	MoviesRecommendationsViewModel _viewModel;
    public MoviesRecommendationsPage(MoviesRecommendationsViewModel viewModel)
	{
		_viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        if (_viewModel.Movies.Count == 0)
            _viewModel.GetRandomMoviesCommand?.Execute(null);
        base.OnAppearing();
    }
}