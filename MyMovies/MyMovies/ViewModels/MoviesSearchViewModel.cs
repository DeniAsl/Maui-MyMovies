using CommunityToolkit.Mvvm.ComponentModel;
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
        private readonly ApiService _apiService;
        private readonly JsonMovieService _jsonMovieService;

        public MoviesSearchViewModel(ApiService apiService, JsonMovieService jsonMovieService)
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
                    SearchMoviesCommand?.Execute(null);
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
            }
        }

        public ICommand NextPageCommand => new Command(() =>
        {
            CurrentPage++;
        });

        public ICommand PreviousPageCommand => new Command(() =>
        {
            CurrentPage--;
        });

        public ICommand SearchMoviesCommand => new Command(async () =>
        {
            IsLoading = true;

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
                    IsLoading = false;
                    moviesCount = apiResponse.ApiData.MovieCount;
                    TotalPages = (int)Math.Ceiling((double)moviesCount / limit);
                    HasNextPage = (limit * currentPage) < moviesCount;
                    HasPreviousPage = currentPage > 1;
                }
            }
        });

        public ICommand ShowMovieCommand => new Command<Movie>(async (movie) =>
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(MovieDetailsViewModel.SelectedMovie), movie },
                { nameof(MovieDetailsViewModel.IsFavorite), await _jsonMovieService.IsFavorite(movie.Id) }
            };

            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}", navigationParameter);
        });
    }
}
