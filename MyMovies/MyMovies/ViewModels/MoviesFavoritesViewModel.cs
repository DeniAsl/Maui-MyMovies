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
    public class MoviesFavoritesViewModel : ObservableObject
    {
        private JsonMovieService _jsonMovieService;
        private ObservableCollection<Movie> movies;

        public MoviesFavoritesViewModel(JsonMovieService jsonMovieService)
        {
            _jsonMovieService = jsonMovieService;
        }

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

        private int selectedMoviesPerPage = 20;
        public int SelectedMoviesPerPage
        {
            get { return selectedMoviesPerPage; }
            set
            {
                if (SetProperty(ref selectedMoviesPerPage, value))
                {
                    GetFavMoviesCommand.Execute(null);
                    CurrentPage = 1;
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
                    GetFavMoviesCommand?.Execute(null);
            }
        }

        private void CheckPaging()
        {
            HasNextPage = (SelectedMoviesPerPage * currentPage) < _jsonMovieService.TotalNumberOfFavMovies();
            HasPreviousPage = currentPage > 1;
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
            CheckPaging();
            Movies = new ObservableCollection<Movie>(await _jsonMovieService.GetAll(SelectedMoviesPerPage, CurrentPage));
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
