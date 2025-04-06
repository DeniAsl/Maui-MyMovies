using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    [QueryProperty(nameof(SelectedMovie), nameof(SelectedMovie))]
    [QueryProperty(nameof(IsFavorite), nameof(IsFavorite))]
    public class MovieDetailsViewModel : ObservableObject
    {
        private ApiService _apiService;
        private JsonMovieService _jsonMovieService;

        public MovieDetailsViewModel(ApiService apiService, JsonMovieService jsonMovieService)
        {
            _apiService = apiService;
            _jsonMovieService = jsonMovieService;
        }

        private Movie selectedMovie;
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                SetProperty(ref selectedMovie, value);
            }
        }

        public ObservableCollection<MovieState> MovieStates { get; set; }
        public MovieState SelectedMovieState { get; set; }

        private ObservableCollection<MovieState> movieStateOptions = new ObservableCollection<MovieState>(Enum.GetValues(typeof(MovieState)).Cast<MovieState>());

        public ObservableCollection<MovieState> MovieStateOptions
        {
            get { return movieStateOptions; }
        }

        private bool isFavorite;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                 SetProperty(ref isFavorite, value);
            }
        }

        public ICommand ShowRelatedMovies => new Command(async () =>
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(MoviesRelatedViewModel.Movies), new ObservableCollection<Movie>(await _apiService.GetRelatedMoviesAsync(SelectedMovie.Id)) },
                { nameof(MoviesRelatedViewModel.SourceMovieTitle), SelectedMovie.Title }
            };

            await Shell.Current.GoToAsync($"{nameof(MoviesRelatedPage)}", navigationParameter);
        });

        public ICommand UpdateMovieState => new Command<Movie>(async (movie) =>
        {
            if (await _jsonMovieService.Update(movie))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Movie state updated.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update movie state (add movie to favorites first).", "OK");
            }
        });

        public ICommand OpenTrailerCommand => new Command(async () =>
        {
            await Launcher.OpenAsync(SelectedMovie.Trailer);
        });

        public ICommand AddToFavoriteCommand => new Command<Movie>(async (movie) =>
        {
            bool result = await _jsonMovieService.Add(movie);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Movie added to favorites.", "OK");
                IsFavorite = true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Movie is already in favorites.", "OK");
            }
        });

        public ICommand RemoveFromFavoriteCommand => new Command<Movie>(async (movie) =>
        {
            bool result = await _jsonMovieService.Remove(movie);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Movie removed from favorites.", "OK");
                IsFavorite = false;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Movie already removed from favorites.", "OK");
            }
        });
    }
}
