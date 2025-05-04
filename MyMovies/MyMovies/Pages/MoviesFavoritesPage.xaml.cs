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
        _viewModel.LoginCommand?.Execute(null);
        base.OnAppearing();
    }
}