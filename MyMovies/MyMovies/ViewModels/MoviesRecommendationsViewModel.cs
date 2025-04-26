using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Enums;
using MyMovies.Core.Models;
using MyMovies.Core.Services;
using MyMovies.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMovies.ViewModels
{
    public class MoviesRecommendationsViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly JsonMovieService _jsonMovieService;

        public MoviesRecommendationsViewModel(ApiService apiService, JsonMovieService jsonMovieService)
        {
            _apiService = apiService;
            _jsonMovieService = jsonMovieService;
        }

        private ObservableCollection<Movie> movies = new();
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set
            {
                SetProperty(ref movies, value);
            }
        }

        private int[] moviesPerPageOptions = new[] { 10, 20, 50 };
        public int[] MoviesPerPageOptions
        {
            get { return moviesPerPageOptions; }
        }

        private int[] moviesPerRowOptions = new[] { 2, 3, 4, 5, };
        public int[] MoviesPerRowOptions
        {
            get { return moviesPerRowOptions; }
        }

        private ObservableCollection<Genre> genreOptions = new ObservableCollection<Genre>(Enum.GetValues(typeof(Genre)).Cast<Genre>());
        public ObservableCollection<Genre> GenreOptions
        {
            get { return genreOptions; }
        }

        private ObservableCollection<SortBy> sortByOptions = new ObservableCollection<SortBy>(Enum.GetValues(typeof(SortBy)).Cast<SortBy>());
        public ObservableCollection<SortBy> SortByOptions
        {
            get { return sortByOptions; }
        }

        private ObservableCollection<OrderBy> orderByOptions = new ObservableCollection<OrderBy>(Enum.GetValues(typeof(OrderBy)).Cast<OrderBy>());
        public ObservableCollection<OrderBy> OrderByOptions
        {
            get { return orderByOptions; }
        }

        private int selectedMoviesPerPage = 20;
        public int SelectedMoviesPerPage
        {
            get { return selectedMoviesPerPage; }
            set
            {
                if (SetProperty(ref selectedMoviesPerPage, value))
                    GetRandomMoviesCommand?.Execute(null);
            }
        }

        private int selectedMoviesPerRow = 3;
        public int SelectedMoviesPerRow
        {
            get { return selectedMoviesPerRow; }
            set
            {
                if (SetProperty(ref selectedMoviesPerRow, value))
                    GetRandomMoviesCommand?.Execute(null);
            }
        }

        private Genre selectedGenre = Genre.None;
        public Genre SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                if (SetProperty(ref selectedGenre, value))
                    GetRandomMoviesCommand?.Execute(null);
            }
        }

        private SortBy selectedSortBy = SortBy.Rating;
        public SortBy SelectedSortBy
        {
            get { return selectedSortBy; }
            set
            {
                if (SetProperty(ref selectedSortBy, value))
                    GetRandomMoviesCommand?.Execute(null);
            }
        }

        private OrderBy selectedOrderBy = OrderBy.Desc;
        public OrderBy SelectedOrderBy
        {
            get { return selectedOrderBy; }
            set
            {
                if (SetProperty(ref selectedOrderBy, value))
                    GetRandomMoviesCommand?.Execute(null);
            }
        }

        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                SetProperty(ref selectedMovie, value);
                if (selectedMovie != null)
                    ShowMovieCommand.Execute(null);
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                SetProperty(ref isLoading, value);
            }
        }

        public ICommand GetRandomMoviesCommand => new Command(async () =>
        {
            IsLoading = true;

            int limit = SelectedMoviesPerPage;
            limit = limit - (limit % SelectedMoviesPerRow);

            ApiResponse apiResponse = await _apiService.GetRandomMoviesAsync(limit, SelectedGenre.ToString(), SelectedSortBy.ToString(), SelectedOrderBy.ToString());
            if (apiResponse.Status.ToLower() != "ok")
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"The following error occured: {apiResponse.StatusMessage}", "OK");
            }
            else
            {
                Movies = new ObservableCollection<Movie>(apiResponse.ApiData.Movies);
                IsLoading = false;
            }
        });

        public ICommand ShowMovieCommand => new Command(async () =>
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(MovieDetailsViewModel.SelectedMovie), SelectedMovie },
                { nameof(MovieDetailsViewModel.IsFavorite), await _jsonMovieService.IsFavorite(SelectedMovie.Id) }
            };

            SelectedMovie = null;

            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}", navigationParameter);
        });
    }
}
