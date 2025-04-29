using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Interfaces;
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
    public class MoviesSearchViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly IJsonMovieService _jsonMovieService;
        private string oldSearchQuery = string.Empty;

        public MoviesSearchViewModel(IApiService apiService, IJsonMovieService jsonMovieService)
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

        private int selectedMoviesPerPage = 20;
        public int SelectedMoviesPerPage
        {
            get { return selectedMoviesPerPage; }
            set
            {
                if (SetProperty(ref selectedMoviesPerPage, value))
                {
                    CurrentPage = 1;
                    SearchMoviesCommand.Execute(null);
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
                    SearchMoviesCommand.Execute(null);
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

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                SetProperty(ref searchQuery, value);
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
                SetProperty(ref currentPage, value);
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
            SearchMoviesCommand?.Execute(null);
        });

        public ICommand PreviousPageCommand => new Command(() =>
        {
            CurrentPage--;
            SearchMoviesCommand?.Execute(null);
        });

        public ICommand SearchMoviesCommand => new Command(async () =>
        {
            IsLoading = true;

            if (oldSearchQuery != SearchQuery)
            {
                CurrentPage = 1;
            }
            oldSearchQuery = SearchQuery;

            int limit = SelectedMoviesPerPage;
            limit = limit - (limit % SelectedMoviesPerRow);

            ApiResponse apiResponse = await _apiService.GetMoviesByQueryAsync(SearchQuery, limit, CurrentPage);
            if (apiResponse.Status.ToLower() != "ok")
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"The following error occured: {apiResponse.StatusMessage}", "OK");
            }
            else
            {
                if (apiResponse.ApiData.Movies == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "No movies found.", "OK");
                }
                else
                {
                    Movies = new ObservableCollection<Movie>(apiResponse.ApiData.Movies);
                    moviesCount = apiResponse.ApiData.MovieCount;
                    TotalPages = (int)Math.Ceiling((double)moviesCount / limit);
                    HasNextPage = (limit * currentPage) < moviesCount;
                    HasPreviousPage = currentPage > 1;

                    IsLoading = false;
                }
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
