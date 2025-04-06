using MyMovies.ViewModels;

namespace MyMovies.Pages;

public partial class MovieDetailsPage : ContentPage
{
	public MovieDetailsPage(MovieDetailsViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}