using MyMovies.Core.Models;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MovieDetailsPage : ContentPage
{
    MovieDetailsViewModel _viewModel;

    public MovieDetailsPage(MovieDetailsViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Movie tempMovie = _viewModel.SelectedMovie;
        _viewModel.SelectedMovie = null;
        _viewModel.SelectedMovie = tempMovie;
    }
}