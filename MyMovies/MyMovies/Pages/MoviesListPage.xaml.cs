using MyMovies.Core.Models;
using MyMovies.Core.Services;
using MyMovies.ViewModels;
using System.Runtime.CompilerServices;

namespace MyMovies.Pages;

public partial class MoviesListPage : ContentPage
{
    private MoviesListViewModel _viewModel;

    public MoviesListPage(MoviesListViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        _viewModel.GetMoviesCommand?.Execute(null);
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