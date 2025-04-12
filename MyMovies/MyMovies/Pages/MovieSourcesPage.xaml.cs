using MyMovies.Core.Models;
using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MovieSourcesPage : ContentPage
{
    private MovieSourcesViewModel _viewModel;

    public MovieSourcesPage(MovieSourcesViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        _viewModel.GetSourcesCommand?.Execute(null);
        base.OnAppearing();
    }
    private void LstSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Source source)
        {
            _viewModel.OpenTrailerCommand?.Execute(source.InfoHash);
        }
    }
}