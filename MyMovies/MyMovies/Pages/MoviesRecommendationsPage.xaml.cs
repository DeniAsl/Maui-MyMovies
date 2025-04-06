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
}