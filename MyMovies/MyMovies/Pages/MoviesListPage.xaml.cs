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
        if (_viewModel.Movies.Count == 0)
            _viewModel.GetMoviesCommand?.Execute(null);
        base.OnAppearing();
    }
}