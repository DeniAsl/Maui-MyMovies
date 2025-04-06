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

        public ICommand SearchMoviesCommand => new Command<string>(async (query) =>
        {
            var movies = await _apiService.GetMoviesByQueryAsync(query);
            if (movies == null)
            {
                await Application.Current.MainPage.DisplayAlert("Info", "No movies found.", "OK");
            }
            else
            {
                Movies = new ObservableCollection<Movie>(movies);
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
