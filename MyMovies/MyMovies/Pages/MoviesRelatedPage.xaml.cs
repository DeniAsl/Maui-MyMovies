using MyMovies.Core.Models;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MoviesRelatedPage : ContentPage
{
    private MoviesRelatedViewModel _viewModel;
    public MoviesRelatedPage(MoviesRelatedViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}

    private void LstMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            _viewModel.ShowMovieCommand?.Execute(movie);
        }
    }
}