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
        private readonly ApiService _apiService;
        private readonly JsonMovieService _jsonMovieService;

        public MoviesListViewModel(ApiService apiService, JsonMovieService jsonMovieService)
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
                    GetMoviesCommand?.Execute(null);
                }
            }
        }

        private ObservableCollection<Genres> genreOptions = new ObservableCollection<Genres>(Enum.GetValues(typeof(Genres)).Cast<Genres>());
        public ObservableCollection<Genres> GenreOptions
        {
            get { return genreOptions; }
        }

        private Genres selectedGenre;
        public Genres SelectedGenre
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
            int limit = SelectedMoviesPerPage;
            if (limit == 50)
                limit = 48;
            else
                limit = limit + (3 - limit % 3);

            Movies = new ObservableCollection<Movie>(await _apiService.GetMoviesAsync(limit, CurrentPage, SelectedGenre.ToString()));

            HasNextPage = (limit * currentPage) < await _apiService.GetMovieCount();
            HasPreviousPage = currentPage > 1;
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
