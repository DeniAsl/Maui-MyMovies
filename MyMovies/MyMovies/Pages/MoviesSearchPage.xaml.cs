using MyMovies.Core.Models;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MoviesSearchPage : ContentPage
{
    private MoviesSearchViewModel _viewModel;

    public MoviesSearchPage(MoviesSearchViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        if (_viewModel.Movies.Count == 0)
            _viewModel.SearchMoviesCommand?.Execute(null);
        base.OnAppearing();
    }
}