using CommunityToolkit.Mvvm.ComponentModel;
using MyMovies.Core.Enums;
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
    public class MoviesFavoritesViewModel : ObservableObject
    {
        private IJsonMovieService _jsonMovieService;

        public MoviesFavoritesViewModel(IJsonMovieService jsonMovieService)
        {
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

        private int[] moviesPerPageOptions = new[] { 10, 20, 50, };
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

        private int selectedMoviesPerPage = 20;
        public int SelectedMoviesPerPage
        {
            get { return selectedMoviesPerPage; }
            set
            {
                if (SetProperty(ref selectedMoviesPerPage, value))
                    CurrentPage = 1;
            }
        }

        private int selectedMoviesPerRow = 3;
        public int SelectedMoviesPerRow
        {
            get { return selectedMoviesPerRow; }
            set
            {
                if (SetProperty(ref selectedMoviesPerRow, value))
                    CurrentPage = 1;
            }
        }

        private Genre selectedGenre;
        public Genre SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                if (SetProperty(ref selectedGenre, value))
                    CurrentPage = 1;
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
                GetFavMoviesCommand?.Execute(null);
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

        public string Metadata
        {
            get
            {
                List<string> strings = new List<string>();
                strings.Add($"{_jsonMovieService.GetNumberOfFavMovies()}");
                strings.Add(SelectedGenre == Genre.Genres ? "" : $"{SelectedGenre}");
                strings.Add(_jsonMovieService.GetNumberOfFavMovies() == 1 ? "movie" : $"movies");
                return string.Join(" ", strings);
            }
        }

        private bool noFavMoviesYet;
        public bool NoFavMoviesYet
        {
            get { return noFavMoviesYet; }
            set
            {
                SetProperty(ref noFavMoviesYet, value);
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

        public ICommand GetFavMoviesCommand => new Command(async () =>
        {
            IsLoading = true;

            int limit = SelectedMoviesPerPage;
            limit = limit - (limit % SelectedMoviesPerRow);

            Movies = new ObservableCollection<Movie>(await _jsonMovieService.GetAll(limit, CurrentPage, SelectedGenre.ToString()));
            IsLoading = false;
            TotalPages = (int)Math.Ceiling((double)_jsonMovieService.GetNumberOfFavMovies() / limit);
            HasNextPage = (limit * currentPage) < _jsonMovieService.GetNumberOfFavMovies();
            HasPreviousPage = currentPage > 1;

            if (Movies.Count == 0)
                NoFavMoviesYet = true;
            else
                NoFavMoviesYet = false;
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
