using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Enums;
using MyMovies.Core.Interfaces;
using MyMovies.Core.Models;
using MyMovies.Core.Services;
using MyMovies.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMovies.ViewModels
{
    public class MoviesListViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly IJsonMovieService _jsonMovieService;

        public MoviesListViewModel(IApiService apiService, IJsonMovieService jsonMovieService)
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
                {
                    CurrentPage = 1;
                    GetMoviesCommand.Execute(null);
                }
            }
        }

        private int selectedMoviesPerRow = 3;
        public int SelectedMoviesPerRow
        {
            get { return selectedMoviesPerRow; }
            set
            {
                if (SetProperty(ref selectedMoviesPerRow, value))
                    GetMoviesCommand.Execute(null);
            }
        }

        private Genre selectedGenre;
        public Genre SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                if (SetProperty(ref selectedGenre, value))
                {
                    CurrentPage = 1;
                    GetMoviesCommand.Execute(null);
                }
            }
        }

        private SortBy selectedSortBy = SortBy.DateAdded;
        public SortBy SelectedSortBy
        {
            get { return selectedSortBy; }
            set
            {
                if (SetProperty(ref selectedSortBy, value))
                    GetMoviesCommand?.Execute(null);
            }
        }

        private OrderBy selectedOrderBy = OrderBy.Desc;
        public OrderBy SelectedOrderBy
        {
            get { return selectedOrderBy; }
            set
            {
                if (SetProperty(ref selectedOrderBy, value))
                    GetMoviesCommand?.Execute(null);
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

        private bool hasNextPage;
        public bool HasNextPage
        {
            get { return hasNextPage; }
            set
            {
                SetProperty(ref hasNextPage, value);
                OnPropertyChanged(nameof(IsNotLoadingAndHasNextPage));
            }
        }

        private bool hasPreviousPage;
        public bool HasPreviousPage
        {
            get { return hasPreviousPage; }
            set
            {
                SetProperty(ref hasPreviousPage, value);
            }
        }

        private int currentPage = 1;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (SetProperty(ref currentPage, value))
                {
                    GetMoviesCommand?.Execute(null);
                }
            }
        }

        private int totalPages;
        public int TotalPages
        {
            get { return totalPages; }
            set
            {
                SetProperty(ref totalPages, value);
                OnPropertyChanged(nameof(Metadata));
            }
        }

        private int moviesCount;

        public string Metadata
        {
            get
            {
                List<string> strings = new List<string>();
                strings.Add($"Found {moviesCount}");
                strings.Add(SelectedGenre == Genre.Genres ? "movies" : $"{SelectedGenre} movies");
                return string.Join(" ", strings);
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                SetProperty(ref isLoading, value);
                OnPropertyChanged(nameof(IsNotLoadingAndHasNextPage));
            }
        }

        public bool IsNotLoadingAndHasNextPage
        {
            get { return !IsLoading && HasNextPage; }
        }

        public ICommand NextPageCommand => new Command(() =>
        {
            CurrentPage++;
        });

        public ICommand PreviousPageCommand => new Command(() =>
        {
            CurrentPage--;
        });

        public ICommand GetMoviesCommand => new Command(async () =>
        {
            IsLoading = true;

            int limit = SelectedMoviesPerPage;
            limit = limit - (limit % SelectedMoviesPerRow);

            ApiResponse apiResponse = await _apiService.GetMoviesAsync(limit, CurrentPage, SelectedGenre.ToString(), SelectedSortBy.ToString(), selectedOrderBy.ToString());
            if (apiResponse.Status.ToLower() != "ok")
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"The following error occured: {apiResponse.StatusMessage}", "OK");
            }
            else
            {
                Movies = new ObservableCollection<Movie>(apiResponse.ApiData.Movies);
                IsLoading = false;
                moviesCount = apiResponse.ApiData.MovieCount;
                TotalPages = (int)Math.Ceiling((double)moviesCount / limit);
                HasNextPage = (limit * currentPage) < moviesCount;
                HasPreviousPage = currentPage > 1;
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
